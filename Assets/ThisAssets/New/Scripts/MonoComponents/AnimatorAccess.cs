using UnityEngine;
using Unity.Entities;
public class AnimatorAccess : MonoBehaviour
{
    private class ThisBaker : Baker<AnimatorAccess>
    {
        public override void Bake(AnimatorAccess authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(targetEntity, new AnimatorComponent());
        }
    }
}