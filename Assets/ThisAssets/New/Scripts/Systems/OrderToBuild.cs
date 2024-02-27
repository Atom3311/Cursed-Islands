using Unity.Entities;
using CursedIsland;
using Unity.Collections;
[UpdateAfter(typeof(OrdersController))]
public partial struct OrderToBuild : ISystem
{
    EntityQuery entityQuery;
    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<OrderInformation>();
        entityQuery = state.GetEntityQuery(typeof(ChoosedUnit), typeof(Builder));
    }
    private void OnUpdate(ref SystemState state)
    {
        OrderInformation orderInformation = SystemAPI.GetSingleton<OrderInformation>();

        if (orderInformation.DuringOrder == Order.None)
            return;
            


        if (orderInformation.DuringOrder != Order.Build)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (choosedTag, builder, entity) in SystemAPI.Query<
                ChoosedUnit,
                RefRW<Builder>>()
                .WithEntityAccess())
            {
                Entity builderFoundation = builder.ValueRW.TargetFoundation;
                if (builderFoundation == Entity.Null)
                    continue;

                builder.ValueRW.TargetFoundation = Entity.Null;
                ecb.RemoveComponent<Build>(entity);

                Foundation targetComponent = SystemAPI.GetComponent<Foundation>(builderFoundation);
                targetComponent.Builder = Entity.Null;
                SystemAPI.SetComponent(builderFoundation, targetComponent);
            }
            ecb.Playback(state.EntityManager);
            return;
        }

        Entity targetEntity = orderInformation.TargetEntity;
        Foundation targetFoundation = SystemAPI.GetComponent<Foundation>(targetEntity);

        if (targetFoundation.Builder != Entity.Null)
            return;

        int targetNumber = Random.GetRandomNumber(0, entityQuery.CalculateEntityCount());
        int duringNumber = 0;

        foreach (var (choosedUnit, builder , entity) in SystemAPI.Query<
            ChoosedUnit,
            RefRW<Builder>>()
            .WithEntityAccess())
        {
            if(duringNumber == targetNumber)
            {
                Entity oldFoundationEntity = builder.ValueRO.TargetFoundation;
                if(oldFoundationEntity != Entity.Null)
                {
                    RefRW<Foundation> oldFoundation = SystemAPI.GetComponentRW<Foundation>(oldFoundationEntity);
                    oldFoundation.ValueRW.Builder = Entity.Null;
                }

                builder.ValueRW.TargetFoundation = targetEntity;
                targetFoundation.Builder = entity;
                SystemAPI.SetComponent(targetEntity, targetFoundation);

                return;
            }
            else
            {
                duringNumber++;
            }
        }
    }
}