using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
public partial struct S_MoveOnPoint : ISystem
{

    void OnUpdate(ref SystemState state)
    {
        foreach (
            (RefRW<LocalTransform> transformComponent,
            RefRW<C_MoveOnPoint> pointComponent ,
            RefRO<C_SpeedOfMovement> speedComponent,
            Entity e)
                in SystemAPI.Query
                    <RefRW<LocalTransform>,
                    RefRW<C_MoveOnPoint>,
                    RefRO<C_SpeedOfMovement>>().WithEntityAccess())
        {
            if (!pointComponent.ValueRO.PointForMove.HasValue)
            {
                continue;
            }

            float moveSpeed = speedComponent.ValueRO.Speed * SystemAPI.Time.DeltaTime;
            float3 point = pointComponent.ValueRO.PointForMove.Value;
            float3 position = transformComponent.ValueRO.Position;

            float3 difference = point - position;

            Vector3 differenceOnXZ = new Vector3
                (
                    difference.x,
                    0,
                    difference.z
                );


            if (differenceOnXZ.magnitude <= moveSpeed)
            {
                transformComponent.ValueRW.Position = new float3(point.x, position.y, point.z);
                pointComponent.ValueRW.PointForMove = null;
            }
            else
            {
                float3 vectorOfMove = differenceOnXZ.normalized * moveSpeed;
                transformComponent.ValueRW.Position += vectorOfMove;
            }
        }
    }
}