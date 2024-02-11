using UnityEngine;
using Unity.Entities;
[RequireComponent(typeof(HullOfMovableUnit))]
public class UnitCollector : MonoBehaviour
{
    public Resource TargetResource;
    public float Speed;
    public float Range;

    private class ThisBaker : Baker<UnitCollector>
    {
        public override void Bake(UnitCollector authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(targetEntity, new Collector()
            {
                TargetResource = authoring.TargetResource,
                Speed = authoring.Speed,
                Range = authoring.Range
            });
            AddComponent<TargetResourceInformation>(targetEntity);
        }
    }
}