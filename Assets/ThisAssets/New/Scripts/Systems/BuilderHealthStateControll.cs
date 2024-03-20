using Unity.Entities;
using Unity.Collections;

public partial struct BuilderHealthStateControll : ISystem
{
    // Метод OnUpdate вызывается при обновлении системы
    private void OnUpdate(ref SystemState state)
    {
        // Создаем буфер команд для работы с Entity
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        // Перебираем все сущности с компонентами Builder, HealthState, AttentionPoint
        foreach (var (builder, healthState, attentionPoint, entity) in SystemAPI.Query <
            RefRW<Builder>, // Ссылка на компонент Builder
            RefRO<HealthState>, // Ссылка на компонент HealthState
            RefRW<AttentionPoint>>() // Ссылка на компонент AttentionPoint
            .WithEntityAccess()) // Получаем доступ к сущности
        {
            // Проверяем, жив ли строитель. Если нет, переходим к следующей итерации
            if (!healthState.ValueRO.IsDead)
                continue;

            // Сбрасываем точку внимания на null
            attentionPoint.ValueRW.Point = null;

            // Удаляем компонент Build у текущей сущности
            ecb.RemoveComponent<Build>(entity);

            // Получаем сущность фундамента, связанную с текущим строителем
            Entity foundationEntity = builder.ValueRO.TargetFoundation;
            if (foundationEntity == Entity.Null)
                continue;

            // Сбрасываем сущность фундамента у строителя и строителя у фундамента
            builder.ValueRW.TargetFoundation = Entity.Null;
            RefRW<Foundation> foundation = SystemAPI.GetComponentRW<Foundation>(foundationEntity);
            foundation.ValueRW.Builder = Entity.Null;
        }

        // Применяем все операции из буфера к EntityManager
        ecb.Playback(state.EntityManager);
    }
}

//Этот код представляет структуру BuilderHealthStateControll, которая управляет состоянием здоровья и поведением строителей в игре.
//Путем проверки состояния здоровья строителя система удаляет компоненты, сбрасывает точку внимания и разрывает связь между строителями и фундаментами.
//Комментарии подробно разъясняют каждую часть кода и последовательность действий в методе OnUpdate для данной системы ECS в Unity.
