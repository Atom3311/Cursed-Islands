using UnityEngine;
using Unity.Entities;
public class HullOfPlayerBuilding : MonoBehaviour
{
    private class ThisBaker : Baker<HullOfPlayerBuilding>
    {
        public override void Bake(HullOfPlayerBuilding authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Unit>(targetEntity);
            AddComponent<Building>(targetEntity);
            AddComponent<PlayerBuilding>(targetEntity);
        }
    }
}