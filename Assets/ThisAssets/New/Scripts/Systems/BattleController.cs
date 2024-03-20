using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

// Структура BattleController, реализующая интерфейс ISystem, отвечает за управление боевыми действиями в игре
[UpdateAfter(typeof(OrderToAttackc))]
[UpdateAfter(typeof(SearchPlayerUnits))]
public partial struct BattleController : ISystem
{
    // Метод OnUpdate вызывается при обновлении системы
    private void OnUpdate(ref SystemState state)
    {
        // Создание буфера команд для работы с Entity
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        // Перебор всех сущностей, содержащих компоненты BattleComponent, LocalTransform, HealthState
        foreach (var (battleComponent, transform, healthState, entity) in SystemAPI.Query<
            RefRW<BattleComponent>,
            RefRO<LocalTransform>,
            RefRO<HealthState>>()
            .WithEntityAccess())
        {
            // Проверка, является ли юнит мертвым, и продолжение цикла в этом случае
            if (healthState.ValueRO.IsDead)
                continue;

            // Получение целевой сущности из компонента BattleComponent
            Entity targetEntity = battleComponent.ValueRO.Target;
            if (targetEntity == Entity.Null)
            {
                // Удаление компонента AttackingUnit, сброс времени атаки и переход к следующей итерации
                ecb.RemoveComponent<AttackingUnit>(entity);
                battleComponent.ValueRW.DuringTime = 0;
                continue;
            }

            // Получение позиций текущей и целевой сущности для расчёта дистанции
            LocalTransform targetTransform = SystemAPI.GetComponent<LocalTransform>(targetEntity);
            float3 position = transform.ValueRO.Position;
            float3 targetPosition = targetTransform.Position;

            // Проверка, является ли юнит атакующим
            bool isAttacking = SystemAPI.HasComponent<AttackingUnit>(entity);

            // Установка компонента AttackingUnit в случае нормальной дистанции для атаки
            if (DistanceIsNormal())
            {
                ecb.AddComponent<AttackingUnit>(entity);
                isAttacking = true;
            }

            // Проверка состояния атаки и времени атаки
            if (!isAttacking)
            {
                battleComponent.ValueRW.DuringTime = 0;
                continue;
            }

            // Получение здоровья целевой сущности
            HealthState targetHealthState = SystemAPI.GetComponent<HealthState>(targetEntity);

            // Выполнение атаки, если прошло достаточное время с предыдущей атаки
            if (battleComponent.ValueRO.DuringTime >= battleComponent.ValueRO.TimeForAttack)
            {
                battleComponent.ValueRW.DuringTime = 0;

                // Уменьшение здоровья целевой сущности и обновление этого значения
                targetHealthState.Health -= battleComponent.ValueRO.Power;
                SystemAPI.SetComponent(targetEntity, targetHealthState);

                // Проверка дистанции и живости целевой сущности
                if (!DistanceIsNormal() || targetHealthState.IsDead)
                {
                    ecb.RemoveComponent<AttackingUnit>(entity);
                }
            }
            else
            {
                battleComponent.ValueRW.DuringTime += SystemAPI.Time.DeltaTime;
            }

            // Метод для проверки дистанции между текущей и целевой сущностями
            bool DistanceIsNormal()
            {
                return math.distance(position, targetPosition) <= battleComponent.ValueRO.RangeOfAttack;
            }
        }

        // Проигрывание накопленных команд в буфере для взаимодействия с Entity
        ecb.Playback(state.EntityManager);
    }
}

