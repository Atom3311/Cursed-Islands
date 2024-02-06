using UnityEngine;
using Unity.Entities;
public class HullOfResource : MonoBehaviour
{
    public Resource TargetResource;
    public byte Count;
    private class ThisBaker : Baker<HullOfResource>
    {
        public override void Bake(HullOfResource authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);
            AddComponent<Unit>(targetEntity);
            AddComponent(targetEntity, new ResourceInformation()
            {
                Type = authoring.TargetResource,
                Count = authoring.Count
            });
        }
    }
}