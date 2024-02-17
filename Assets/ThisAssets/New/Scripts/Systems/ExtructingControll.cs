using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
public partial struct ExtructingControll : ISystem
{
    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<InformationAboutResources>();
    }
    private void OnUpdate(ref SystemState state)
    {
        InformationAboutResources resources = SystemAPI.GetSingleton<InformationAboutResources>();
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (extructingTag, animator, collector, entity) in SystemAPI.Query<
            RefRO<Extructing>,
            AnimatorComponent,
            RefRW<Collector>>()
            .WithEntityAccess())
        {
            Entity targetEntity = collector.ValueRO.TargetResourceEntity;
            if (targetEntity == Entity.Null)
            {
                ecb.RemoveComponent<ExtructAction>(entity);
                ecb.RemoveComponent<Extructing>(entity);
                collector.ValueRW.DuringTime = 0;
                //animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationExtruct, false);
                continue;
            }

            ResourceInformation resourceInformation = SystemAPI.GetComponent<ResourceInformation>(targetEntity);

            if(resourceInformation.Count <= 0)
            {
                foreach (RefRW<Collector> checkingCollector in SystemAPI.Query<RefRW<Collector>>())
                {
                    if (checkingCollector.ValueRO.TargetResourceEntity == targetEntity)
                    {
                        checkingCollector.ValueRW.TargetResourceEntity = Entity.Null;
                    }
                }
                ecb.DestroyEntity(targetEntity);
                Entity duringGraphicResource = SystemAPI.GetSingletonEntity<GraphicOfResource>();
                Parent parent = SystemAPI.GetComponent<Parent>(duringGraphicResource);

                if (parent.Value == targetEntity)
                    ecb.DestroyEntity(duringGraphicResource);

                continue;
            }
            if(collector.ValueRO.DuringTime >= collector.ValueRO.NeededTime)
            {
                collector.ValueRW.DuringTime = 0;
                resources.AddValue(resourceInformation.Type, 1);
                resourceInformation.Count -= 1;
                SystemAPI.SetComponent(targetEntity, resourceInformation);
            }
            else
            {
                collector.ValueRW.DuringTime += SystemAPI.Time.DeltaTime;
            }
            //animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationExtruct, true);
        }
        SystemAPI.SetSingleton(resources);
        ecb.Playback(state.EntityManager);
    }
}
