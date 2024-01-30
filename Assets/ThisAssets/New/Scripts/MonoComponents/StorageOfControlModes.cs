using UnityEngine;
using Unity.Entities;
public class StorageOfControlModes : MonoBehaviour
{
    public ControlMode Mode;
    private class ThisBaker : Baker<StorageOfControlModes>
    {
        public override void Bake(StorageOfControlModes authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);
            AddComponent(targetEntity, new InformationAboutControlMode() { ControlMode = authoring.Mode });
        }
    }
}