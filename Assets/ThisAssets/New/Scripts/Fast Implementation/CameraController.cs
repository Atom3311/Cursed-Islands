using UnityEngine;

// Объявление класса Контроллера камеры, который управляет поведением камеры в Unity
public class CameraController : MonoBehaviour
{
    // Переменная для хранения позиции касания
    Vector3 touch;

    // Минимальный уровень масштабирования камеры
    public float zoomMin = 10;

    // Максимальный уровень масштабирования камеры
    public float zoomMax = 25;

    // Ссылка на компонент камеры
    [SerializeField] private Camera q;

    // Режим управления камерой
    public ControlMode DuringControlMode;

    // Метод обновления, вызывается каждый кадр
    private void Update()
    {
        // Проверка, находится ли камера в режиме просмотра
        if (DuringControlMode == ControlMode.Viewing)
        {
            // Обработка нажатия левой кнопки мыши
            if (Input.GetMouseButtonDown(0))
            {
                // Запоминаем позицию касания в мировых координатах
                touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            // Обработка мультитача (двойного касания)
            if (Input.touchCount == 2)
            {
                // Получаем информацию о двух сенсорах
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Вычисляем предыдущие позиции движения сенсоров
                Vector2 touchZeroLastPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOneLastPos = touchOne.position - touchOne.deltaPosition;

                // Вычисляем расстояние между сенсорами
                float distTouch = (touchZeroLastPos - touchOneLastPos).magnitude;
                float currentDistTouch = (touchZero.position - touchOne.position).magnitude;
                float difference = currentDistTouch - distTouch;

                // Масштабируем камеру на основе разницы в дистанции между сенсорами
                Zoom(difference * 0.01f);
            }
            else if (Input.GetMouseButton(0))
            {
                // Вычисляем направление движения камеры в ответ на движение мыши
                Vector3 direction = touch - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Camera.main.transform.position += direction;
            }

            // Масштабирование камеры с помощью колеса прокрутки мыши
            Zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    // Метод для изменения масштаба камеры
    void Zoom(float increment)
    {
        // Ограничиваем масштабирование камеры в пределах заданного диапазона
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomMin, zoomMax);
        q.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomMin, zoomMax);
    }
}


