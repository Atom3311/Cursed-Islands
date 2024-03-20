using Unity.Entities;
[UpdateAfter(typeof(ConnectMainAnimatorWithEntity))]

// Объявление структуры AnimationForMovableUnits, реализующей интерфейс ISystem
public partial struct AttackAnimation : ISystem
{
    // Метод OnUpdate, вызываемый при обновлении системы
    private void OnUpdate(ref SystemState state)
    {
        // Перебор всех кортежей, включающих указанные компоненты
        foreach (var (healthState, battleComponent, animator, entity) in SystemAPI.Query<
            RefRO<HealthState>,
            RefRO<BattleComponent>,
            AnimatorComponent>()
            .WithEntityAccess())
        {
            // Проверка наличия основного аниматора в компоненте AnimatorComponent
            if (healthState.ValueRO.IsDead)
            {
                animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationAttack, false);
                continue; // Пропуск текущей итерации цикла, если аниматор не найден 
            }
            // Проверка условия для запуска анимации перемещения
            bool isAttacking = SystemAPI.HasComponent<AttackingUnit>(entity);
            // Установка значения параметра анимации для бега
            animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationAttack, isAttacking);
        }
    }
}

//Этот код описывает работу системы AnimationForMovableUnits, которая отвечает за управление анимациями движущихся юнитов в игре.
//В методе OnUpdate происходит перебор сущностей, содержащих компоненты MovableUnit, MovePoint, HealthState и AnimatorComponent.
//Для каждой сущности проверяется наличие основного аниматора, после чего устанавливается параметр анимации для бега в зависимости от условий движения
//(присутствие целевой точки и живой юнит). Если аниматор отсутствует, то происходит переход к следующей итерации цикла.


