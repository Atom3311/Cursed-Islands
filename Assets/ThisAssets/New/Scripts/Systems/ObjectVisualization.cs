using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
public partial struct ObjectVisualization : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach( var (
            transform,
            visualObject) in SystemAPI.Query<
                RefRO<LocalTransform>,
                VisualObject>())
        {
            float3 position = transform.ValueRO.Position;
            quaternion rotation = transform.ValueRO.Rotation;
            if (!visualObject.IsCreated)
            {
                visualObject.ThisGameObject = Object.Instantiate(visualObject.ThisGameObject, position, rotation);
                visualObject.IsCreated = true;
            }
            Transform transformOfVisualObject = visualObject.ThisGameObject.transform;
            transformOfVisualObject.position = position;
            transformOfVisualObject.rotation = rotation;
        }
    }
}