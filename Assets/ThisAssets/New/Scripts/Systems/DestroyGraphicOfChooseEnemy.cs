using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
partial struct DestroyGraphicOfChooseEnemy : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (healthState, childs) in SystemAPI.Query<
            RefRO<HealthState>,
            DynamicBuffer<Child>>())
        {
            if (!healthState.ValueRO.IsDead)
                continue;

            foreach (Child child in childs)
            {
                Entity childEntity = child.Value;
                if (SystemAPI.HasComponent<GraphicOfChooseEnemy>(childEntity))
                    ecb.DestroyEntity(childEntity);
            }

        }
        ecb.Playback(state.EntityManager);
    }
}