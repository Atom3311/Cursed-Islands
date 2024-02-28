using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
[UpdateAfter(typeof(BuilderHealthStateControll))]
public partial struct BuildControll : ISystem
{
    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BuildingParametersComponent>();
    }
    private void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var buildingParametersComopnent = SystemAPI.GetSingleton<BuildingParametersComponent>();

        foreach (var (foundation, transform, entity) in SystemAPI.Query<
            RefRW<Foundation>,
            RefRW<LocalTransform>>()
            .WithEntityAccess())
        {
            Entity builderEntity = foundation.ValueRO.Builder;

            if (builderEntity == Entity.Null || !SystemAPI.HasComponent<Build>(builderEntity))
            {
                foundation.ValueRW.DuringTime = 0;
                continue;
            }

            if(foundation.ValueRO.DuringTime >= Constants.NeededTimeForBuild)
            {
                RefRW<Builder> builder = SystemAPI.GetComponentRW<Builder>(builderEntity);
                builder.ValueRW.TargetFoundation = Entity.Null;

                ecb.RemoveComponent<Build>(builderEntity);
                
                ecb.DestroyEntity(entity);

                Entity createdEntity = ecb.Instantiate(buildingParametersComopnent.Building);

                float3 position = transform.ValueRO.Position;
                LocalTransform transformForCreatedEntity = LocalTransform.Identity.WithPosition(position);

                ecb.SetComponent(createdEntity, transformForCreatedEntity);

                ecb.AddComponent<Building>(createdEntity);
                ecb.AddComponent<PlayerBuilding>(createdEntity);
            }
            else
            {
                foundation.ValueRW.DuringTime += SystemAPI.Time.DeltaTime;
            }
        }
        ecb.Playback(state.EntityManager);
    }
}