using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using Unity.Transforms;
public partial struct UnitsDeadController : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (healthState, entity) in SystemAPI.Query<
            RefRW<HealthState>>()
            .WithEntityAccess())
        {
            if (!healthState.ValueRO.IsDead)
                continue;


            if(healthState.ValueRO.DuringDeadTime >= Constants.TimeForDead)
            {
                ecb.DestroyEntity(entity);
                VisualObject visualObject = state.EntityManager.GetComponentObject<VisualObject>(entity);
                Object.Destroy(visualObject.ThisGameObject);

            }
            else
            {
                healthState.ValueRW.DuringDeadTime += SystemAPI.Time.DeltaTime;
            }
        }
        ecb.Playback(state.EntityManager);
    }
}