using Unity.Entities;
using Unity.Collections;
public partial struct BuilderHealthStateControll : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (builder, healthState, attentionPoint, entity) in SystemAPI.Query <
            RefRW<Builder>,
            RefRO<HealthState>,
            RefRW<AttentionPoint>>()
            .WithEntityAccess())
        {
            if (!healthState.ValueRO.IsDead)
                continue;

            attentionPoint.ValueRW.Point = null;
            
            ecb.RemoveComponent<Build>(entity);

            Entity foundationEntity = builder.ValueRO.TargetFoundation;
            if (foundationEntity == Entity.Null)
                continue;

            builder.ValueRW.TargetFoundation = Entity.Null;
            RefRW<Foundation> foundation = SystemAPI.GetComponentRW<Foundation>(foundationEntity);
            foundation.ValueRW.Builder = Entity.Null;

        }
        ecb.Playback(state.EntityManager);
    }
}