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

        foreach (var (extructingTag, healthState, animator, collector, entity) in SystemAPI.Query<
            RefRO<Extructing>,
            RefRO<HealthState>,
            AnimatorComponent,
            RefRW<Collector>>()
            .WithEntityAccess())
        {
            if (healthState.ValueRO.IsDead)
            {
                ecb.RemoveComponent<Extructing>(entity);
                animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationExtruct, false);
                continue;
            }

            Entity targetEntity = collector.ValueRO.TargetResourceEntity;
            if (targetEntity == Entity.Null)
            {
                ecb.RemoveComponent<ExtructAction>(entity);
                ecb.RemoveComponent<Extructing>(entity);
                collector.ValueRW.DuringTime = 0;
                animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationExtruct, false);
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
                if (SystemAPI.HasBuffer<Child>(targetEntity))
                {
                    DynamicBuffer<Child> childs = SystemAPI.GetBuffer<Child>(targetEntity);
                    foreach (Child child in childs)
                    {
                        Entity childEntity = child.Value;
                        if (SystemAPI.HasComponent<GraphicOfResource>(childEntity))
                            ecb.DestroyEntity(childEntity);
                    }
                }

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
            animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationExtruct, true);
        }
        SystemAPI.SetSingleton(resources);
        ecb.Playback(state.EntityManager);
    }
}
