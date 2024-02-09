using UnityEngine;
using Unity.Entities;
public class VisualObjectCreator : MonoBehaviour
{
    public GameObject TargetObject;
    private class ThisBaker : Baker<VisualObjectCreator>
    {
        public override void Bake(VisualObjectCreator authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(targetEntity, new VisualObject { ThisGameObject = authoring.TargetObject });
        }
    }
}