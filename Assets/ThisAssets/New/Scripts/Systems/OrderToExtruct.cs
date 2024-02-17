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
        var orderInformation = SystemAPI.GetSingleton<OrderInformation>();
        var input = SystemAPI.GetSingleton<InformationAboutInputPlayer>();
        var graphicSettings = SystemAPI.GetSingleton<GraphicSettingsComponent>();

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        if (input.ClickDown)
        {
            foreach(var (graphicTag, entity) in SystemAPI.Query<
                RefRO<GraphicOfResource>>()
                .WithEntityAccess())
            {
                ecb.DestroyEntity(entity);
            }
        }

        if (orderInformation.DuringOrder == Order.None)
        {
            ecb.Playback(state.EntityManager);
            return;
        }

        if (orderInformation.DuringOrder != Order.Extruct)
        {
            foreach ((
            RefRW<Collector> collector,
            RefRO<ChoosedUnit> choosedUnit,
            AnimatorComponent animator,
            Entity entity) in SystemAPI.Query<
                RefRW<Collector>,
                RefRO<ChoosedUnit>,
                AnimatorComponent>()
                .WithEntityAccess())
            {
                ecb.RemoveComponent<ExtructAction>(entity);
                ecb.RemoveComponent<Extructing>(entity);
                collector.ValueRW.DuringTime = 0;
                //animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationExtruct, false);

            }
            ecb.Playback(state.EntityManager);
            return;
        }

        Entity targetEntity = orderInformation.TargetEntity;
        LocalTransform transform = SystemAPI.GetComponent<LocalTransform>(targetEntity);

        Entity graphic = ecb.Instantiate(graphicSettings.GraphicOfChooseResource);
        ecb.AddComponent<GraphicOfResource>(graphic);
        ecb.AddComponent(graphic, new Parent() { Value = targetEntity });

        foreach ((
            PackageOfMovableUnit package,
            RefRW<Collector> collector,
            RefRO<ChoosedUnit> choosedUnit,
            Entity entity) in SystemAPI.Query<
                PackageOfMovableUnit,
                RefRW<Collector>,
                RefRO<ChoosedUnit>>().WithEntityAccess())
        {
            ResourceInformation resource = SystemAPI.GetComponent<ResourceInformation>(targetEntity);

            if (resource.Type != collector.ValueRO.TargetResourceType)
                continue;

            ecb.AddComponent<ExtructAction>(entity);
            collector.ValueRW.TargetResourceEntity = targetEntity;
            package.MovePoint.ValueRW.PointInWorld = transform.Position;
        }
        ecb.Playback(state.EntityManager);
    }
}