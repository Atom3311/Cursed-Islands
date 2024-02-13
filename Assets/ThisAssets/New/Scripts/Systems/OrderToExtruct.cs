using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
[UpdateAfter(typeof(OrdersController))]
public partial struct OrderToExtruct : ISystem
{
    private EntityQuery _entityQuery;
    private void OnCreate(ref SystemState state)
    {
        _entityQuery = state.GetEntityQuery(typeof(ChoosedUnit));
        state.RequireForUpdate<OrderInformation>();
    }
    private void OnUpdate(ref SystemState state)
    {
        var orderInformation = SystemAPI.GetSingleton<OrderInformation>();

        if (orderInformation.DuringOrder != Order.Extruct)
            return;

        if (_entityQuery.CalculateEntityCount() == 0)
            return;

        Entity targetEntity = orderInformation.TargetEntity;
        LocalTransform transform = SystemAPI.GetComponent<LocalTransform>(targetEntity);

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach((
            PackageOfMovableUnit package,
            RefRO<Collector> collector,
            RefRO<ChoosedUnit> choosedUnit,
            Entity entity) in SystemAPI.Query<
                PackageOfMovableUnit,
                RefRO<Collector>,
                RefRO<ChoosedUnit>>().WithEntityAccess())
        {
            package.MovePoint.ValueRW.PointInWorld = transform.Position;
            ecb.AddComponent<ExtructAction>(entity);
            ecb.SetComponent(entity, new TargetResourceInformation() { TargetResource = targetEntity });
        }
        ecb.Playback(state.EntityManager);
    }
}