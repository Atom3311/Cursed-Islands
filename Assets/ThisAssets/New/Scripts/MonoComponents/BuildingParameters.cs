using UnityEngine;
using Unity.Entities;
public class BuildingParameters : MonoBehaviour
{
    public GameObject Building;
    private class ThisBaker : Baker<BuildingParameters>
    {
        public override void Bake(BuildingParameters authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);
            Entity buildingEntity = GetEntity(authoring.Building, TransformUsageFlags.None);
            AddComponent(targetEntity, new BuildingParametersComponent() { Building = buildingEntity });
        }
    }
}