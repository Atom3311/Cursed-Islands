using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
[UpdateAfter(typeof(OrdersController))]
public partial struct OrderToExtruct : ISystem
{
    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<OrderInformation>();
    }
    private void OnUpdate(ref SystemState state)
    {
        //var orderInformation = SystemAPI.GetSingleton<OrderInformation>();
        //EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        //if(orderInformation.DuringOrder != Order.Extruct)
        //{
        //    if (orderInformation.DuringOrder != Order.None)
        //    {
        //        UnityEngine.Debug.Log(1);
        //        foreach ((
        //        RefRO<Collector> collector,
        //        RefRO<ChoosedUnit> choosedUnit,
        //        Entity entity) in SystemAPI.Query<
        //            RefRO<Collector>,
        //            RefRO<ChoosedUnit>>().WithEntityAccess())
        //        {
        //            ecb.RemoveComponent<ExtructAction>(entity);
        //        }
        //        ecb.Playback(state.EntityManager);
        //    }
        //    return;
        //}

        //Entity targetEntity = orderInformation.TargetEntity;
        //LocalTransform transform = SystemAPI.GetComponent<LocalTransform>(targetEntity);

        //foreach((
        //    PackageOfMovableUnit package,
        //    RefRW<Collector> collector,
        //    RefRO<ChoosedUnit> choosedUnit,
        //    Entity entity) in SystemAPI.Query<
        //        PackageOfMovableUnit,
        //        RefRW<Collector>,
        //        RefRO<ChoosedUnit>>().WithEntityAccess())
        //{
        //    package.MovePoint.ValueRW.PointInWorld = transform.Position;
        //    ecb.AddComponent<ExtructAction>(entity);
        //    collector.ValueRW.TargetResourceEntity = SystemAPI.GetComponent<ResourceInformation>(targetEntity);
        //}
        //ecb.Playback(state.EntityManager);
    }
}