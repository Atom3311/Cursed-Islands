using Unity.Entities;

[UpdateAfter(typeof(ConnectMainAnimatorWithEntity))]
// Дополнительно к ссылке на ConnectMainAnimatorWithEntity обновляем сущности после его выполнения
partial struct BuildAnimation : ISystem
{
    // Метод OnUpdate вызывается при обновлении системы
    private void OnUpdate(ref SystemState state)
    {
        // Перебор всех сущностей, содержащих компоненты Builder, HealthState, AnimatorComponent
        foreach (var (builderTag, healthState, animator, entity) in SystemAPI.Query<
            Builder, // Компонент, отвечающий за постройку
            HealthState, // Компонент с информацией о здоровье
            AnimatorComponent>() // Компонент анимации
            .WithEntityAccess()) // Получение доступа к сущности
        {
            // Проверка, является ли целевая сущность мертвой
            if (healthState.IsDead)
            {
                // Установка значения анимации построения в false, если целевая сущность мертва
                animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationBuild, false);
                continue; // Переход к следующей итерации цикла
            }

            // Проверка наличия компонента Build у сущности
            bool build = SystemAPI.HasComponent<Build>(entity);
            // Установка значения анимации построения в зависимости от наличия компонента Build
            animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationBuild, build);
        }
    }
}


//Этот код описывает обновление анимации для объектов, задействованных в строительстве.
//Система BuildAnimation осуществляет проверку состояния здоровья целевой сущности и устанавливает соответствующую анимацию строительства
//в зависимости от наличия компонента Build. Комментарии подробно разъясняют каждый шаг выполнения метода OnUpdate и его влияние на анимацию в игровом процессе.
