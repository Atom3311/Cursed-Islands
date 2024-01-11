using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
public partial struct Example_S_Move : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<LocalTransform> transform, RefRO<C_SpeedOfMovement> speed, RefRO<C_OrientationInGame> orientation) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<C_SpeedOfMovement>, RefRO<C_OrientationInGame>> ())
        {
            float finalMove = speed.ValueRO.Speed * Parameters.FactorOfFrames;
            transform.ValueRW = transform.ValueRO.Translate( orientation.ValueRO.Orientation * finalMove);
        }
    }
}