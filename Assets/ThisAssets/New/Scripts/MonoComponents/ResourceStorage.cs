using UnityEngine;
using Unity.Entities;
public class ResourceStorage : MonoBehaviour
{
    [Min(0)] public int Gold;
    [Min(0)] public int Wood;
    [Min(0)] public int Food;
    private class ThisBaker : Baker<ResourceStorage>
    {
        public override void Bake(ResourceStorage authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);
            InformationAboutResources resources = new InformationAboutResources();

            resources.SetValue(Resource.Gold, authoring.Gold);
            resources.SetValue(Resource.Wood, authoring.Wood);
            resources.SetValue(Resource.Food, authoring.Food);

            AddComponent(targetEntity, resources);

            AddComponentObject(targetEntity, new EventChangedResources());

            
        }
    }
}