using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;

[UpdateAfter(typeof(OrdersController))]
[UpdateAfter(typeof(EnemiesHealthControll))]
[UpdateAfter(typeof(InputHandler))]
public partial struct OrderToAttackc : ISystem
{
    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<OrderInformation>();
        state.RequireForUpdate<InformationAboutInputPlayer>();
        state.RequireForUpdate<GraphicSettingsComponent>();
    }
    private void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var input = SystemAPI.GetSingleton<InformationAboutInputPlayer>();
        var graphicSettings = SystemAPI.GetSingleton<GraphicSettingsComponent>();

        if (input.ClickDown)
        {
            if(SystemAPI.TryGetSingletonEntity<GraphicOfChooseEnemy>(out Entity graphic))
            {
                ecb.DestroyEntity(graphic);
            }
                
        }

        OrderInformation orderInformation = SystemAPI.GetSingleton<OrderInformation>();

        Entity targetEntity = orderInformation.TargetEntity;

        if (orderInformation.DuringOrder == Order.None)
        {
            ecb.Playback(state.EntityManager);
            return;
        }

        bool successfullOrder = orderInformation.DuringOrder == Order.Attack;

        foreach (var(battleComponent, healthState, choosed) in SystemAPI.Query<
            RefRW<BattleComponent>,
            RefRO<HealthState>,
            RefRO<ChoosedUnit>>())
        {
            if (healthState.ValueRO.IsDead)
            {
                battleComponent.ValueRW.Target = Entity.Null;
                continue;
            }
            if (!successfullOrder)
            {
                battleComponent.ValueRW.Target = Entity.Null;
                continue;
            }

            battleComponent.ValueRW.Target = targetEntity;

        }

        if (successfullOrder)
        {
            Entity createdGraphic = ecb.Instantiate(graphicSettings.GraphicOfChooseEnemy);
            ecb.AddComponent<GraphicOfChooseEnemy>(createdGraphic);
            ecb.AddComponent(createdGraphic, new Parent() { Value = targetEntity });
        }
        ecb.Playback(state.EntityManager);
    }
}