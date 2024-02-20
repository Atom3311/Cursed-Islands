using Unity.Entities;
[UpdateAfter(typeof(OrdersController))]
[UpdateAfter(typeof(EnemiesHealthControll))]
public partial struct OrderToAttackc : ISystem
{
    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<OrderInformation>();
    }
    private void OnUpdate(ref SystemState state)
    {
        OrderInformation orderInformation = SystemAPI.GetSingleton<OrderInformation>();
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
            if (orderInformation.DuringOrder == Order.None)
                return;

            if (orderInformation.DuringOrder != Order.Attack)
            {
                battleComponent.ValueRW.Target = Entity.Null;
                continue;
            }

            Entity targetEntity = orderInformation.TargetEntity;
            battleComponent.ValueRW.Target = targetEntity;
        }
    }
}