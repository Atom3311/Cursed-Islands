using UnityEngine;
using Unity.Entities;
public class BuildingParameters : MonoBehaviour
{
    public GameObject Building;
    public int Gold;
    public int Wood;
    public int Food;

    private class ThisBaker : Baker<BuildingParameters>
    {
        public override void Bake(BuildingParameters authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);
            Entity buildingEntity = GetEntity(authoring.Building, TransformUsageFlags.None);
            AddComponent(targetEntity, new BuildingParametersComponent()
            {
                Building = buildingEntity,
                Gold = authoring.Gold,
                Wood = authoring.Wood,
                Food = authoring.Food
            });
        }
    }
}