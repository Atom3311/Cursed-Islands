using UnityEngine;
using Unity.Entities;
public class SourceOfInformationAboutInputPlayer : MonoBehaviour
{
    private class ThisBaker : Baker<SourceOfInformationAboutInputPlayer>
    {
        public override void Bake(SourceOfInformationAboutInputPlayer authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);
            AddComponent<InformationAboutInputPlayer>(targetEntity);
        }
    }
}