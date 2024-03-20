using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine;

// Структура BattleMove, реализующая интерфейс ISystem, отвечает за движение юнитов в боевой сцене
[UpdateAfter(typeof(BattleController))] // Позиция обновления после выполнения системы BattleController
public partial struct BattleMove : ISystem
{
    // Метод OnUpdate вызывается при обновлении системы
    private void OnUpdate(ref SystemState state)
    {
        foreach (var(healthState, battleComponent, transform, moveComponents, attentionPoint, entity) in SystemAPI.Query<
            RefRO<HealthState>, // Ссылка на компонент состояния здоровья
            RefRW<BattleComponent>, // Ссылка на компонент боевых действий
            RefRO<LocalTransform>, // Ссылка на компонент местоположения в локальных координатах
            PackageOfMovableUnit, // Пакет компонентов для передвижения юнитов
            RefRW<AttentionPoint>>() // Ссылка на компонент точки внимания
            .WithEntityAccess())
        {
            // Проверка, является ли юнит мертвым
            if (healthState.ValueRO.IsDead)
            {
                // Сброс точки перемещения и переход к следующей итерации
                moveComponents.MovePoint.ValueRW.PointInWorld = null;
                continue;
            }

            // Получение целевой сущности для атаки
            Entity targetEntity = battleComponent.ValueRO.Target;

            if (targetEntity == Entity.Null)
                continue; // Пропуск текущей итерации, если целевая сущность не определена

            // Получение состояния здоровья целевой сущности
            HealthState targetHealthState = SystemAPI.GetComponent<HealthState>(targetEntity);

            // Проверка, является ли целевая сущность мертвой
            if (targetHealthState.IsDead)
            {
                // Сброс точки перемещения и переход к следующей итерации
                moveComponents.MovePoint.ValueRW.PointInWorld = null;
                continue;
            }

            // Получение местоположения текущего и целевого юнитов для определения точки внимания
            LocalTransform targetTransform = SystemAPI.GetComponent<LocalTransform>(targetEntity);
            float3 position = transform.ValueRO.Position;
            float3 targetPosition = targetTransform.Position;
            attentionPoint.ValueRW.Point = targetPosition; // Установка точки внимания на целевую позицию

            // Проверка, атакует ли юнит или дистанция нормальная для атаки
            if (SystemAPI.HasComponent<AttackingUnit>(entity) || DistanceIsNormal())
            {
                // Сброс точки перемещения, если условие выполнено
                moveComponents.MovePoint.ValueRW.PointInWorld = null;
                continue;
            }
            else
            {
                moveComponents.MovePoint.ValueRW.PointInWorld = targetPosition; // Установка точки перемещения на целевую позицию 
            }

            // Метод для проверки нормальной дистанции между юнитами
            bool DistanceIsNormal()
            {
                return math.distance(position, targetPosition) <= battleComponent.ValueRO.RangeOfAttack;
            }
        }
    }
}

//Комментарии подробно описывают функциональность структуры BattleMove, которая управляет движением юнитов в игровой сцене.
//Код работает с компонентами здоровья, боевых действий, местоположения, пакета для передвижения и точки внимания.
//В ходе выполнения проверяется состояние юнитов, определяется целевая сущность для атаки,
//проводится анализ дистанции до цели и устанавливается точка перемещения в зависимости от условий.
