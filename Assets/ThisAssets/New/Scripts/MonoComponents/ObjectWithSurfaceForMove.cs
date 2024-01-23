using UnityEngine;
using Unity.Entities;
public class ObjectWithSurfaceForMove : MonoBehaviour
{
    private class ThisBaker : Baker<ObjectWithSurfaceForMove>
    {
        public override void Bake(ObjectWithSurfaceForMove authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);
            AddComponent(targetEntity, new SurfaceForMove());
        }
    }
}