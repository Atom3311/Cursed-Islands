using Unity.Entities;
// Определение структуры AnimationForMovableUnits, реализующей интерфейс ISystem
public partial struct AnimationForMovableUnits : ISystem
{
    // Метод OnUpdate, вызываемый при обновлении системы
    private void OnUpdate(ref SystemState state)
    {
        // Перебор всех сущностей, содержащих компоненты MovableUnit, MovePoint, HealthState и AnimatorComponent
        foreach ((
            RefRO<MovableUnit> unit,
            RefRO<MovePoint> targetPoint,
            RefRO<HealthState> healthState,
            AnimatorComponent animator) in SystemAPI.Query<
                RefRO<MovableUnit>,
                RefRO<MovePoint>,
                RefRO<HealthState>,
                AnimatorComponent>())
        {
            // Проверка наличия основного аниматора в компоненте AnimatorComponent
            if (!animator.HasMainAnimator)
                continue; // Пропуск текущей итерации цикла, если аниматор не найден 

            // Проверка условия для запуска анимации перемещения
            bool isRunning = targetPoint.ValueRO.PointInWorld.HasValue && !healthState.ValueRO.IsDead;
            // Установка значения параметра анимации для бега
            animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationRun, isRunning);
        }
    }
}




//Данный сценарий определяет обновление анимаций для двигающихся юнитов в игре.
//Он перебирает все сущности, содержащие необходимые компоненты, и на основе условий определяет,
//должна ли запускаться анимация бега для соответствующего юнита.