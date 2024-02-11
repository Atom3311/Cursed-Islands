using Unity.Entities;
using Unity.Mathematics;
[UpdateAfter(typeof(OrdersController))]
partial struct PaddlerPlayerMovePoint : ISystem
{
    private EntityQuery _queryForChoosedUnits;
    private void OnCreate(ref SystemState state)
    {
        _queryForChoosedUnits = state.GetEntityQuery(typeof(ChoosedUnit));
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
            RefRO<ChoosedUnit> choosedUnit) in SystemAPI.Query<
                RefRW<MovePoint>,
                RefRO<ChoosedUnit>>())
        {
            if (countOfEntities == 1)
            {
                movePoint.ValueRW.PointInWorld = targetPoint;
            }
            else
            {
                float3 randomPoint = GetRandomPointFromRect.GetRandomPoint(targetPoint, math.sqrt(Constants.RectForUnit * countOfEntities));
                movePoint.ValueRW.PointInWorld = randomPoint;
            }
        }
    }
}