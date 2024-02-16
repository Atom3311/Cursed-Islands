using Unity.Entities;
using Unity.Mathematics;
[UpdateAfter(typeof(OrdersController))]
partial struct OrderToMove : ISystem
{
    private EntityQuery _queryForChoosedUnits;
    private void OnCreate(ref SystemState state)
    {
        _queryForChoosedUnits = state.GetEntityQuery(typeof(ChoosedUnit));
        state.RequireForUpdate<OrderInformation>();
    }
    private void OnUpdate(ref SystemState state)
    {
        var orderInformation = SystemAPI.GetSingleton<OrderInformation>();

        if (orderInformation.DuringOrder != Order.Move)
            return;

        float3 targetPoint = orderInformation.TargetPoint;

        int countOfEntities = _queryForChoosedUnits.CalculateEntityCount();

        foreach ((
            RefRW<MovePoint> movePoint,
            RefRO<ChoosedUnit> choosedUnit,
            RefRW<AttentionPoint> attentionPoint) in SystemAPI.Query<
                RefRW<MovePoint>,
                RefRO<ChoosedUnit>,
                RefRW<AttentionPoint>>())
        {
            if (countOfEntities == 1)
            {
                movePoint.ValueRW.PointInWorld = targetPoint;
                attentionPoint.ValueRW.Point = targetPoint;
            }
            else
            {
                float3 randomPoint = GetRandomPointFromRect.GetRandomPoint(targetPoint, math.sqrt(Constants.RectForUnit * countOfEntities));
                movePoint.ValueRW.PointInWorld = randomPoint;
                attentionPoint.ValueRW.Point = randomPoint;
            }
        }
    }
}