using Unity.Entities;
using UnityEngine;
public class GraphicSettings : MonoBehaviour
{
    public GameObject GraphicOfChooseUnit;
    private class ThisBaker : Baker<GraphicSettings>
    {
        public override void Bake(GraphicSettings authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);
            Entity entityOfGraphic = GetEntity(authoring.GraphicOfChooseUnit, TransformUsageFlags.Dynamic);
            AddComponent(targetEntity, new GraphicSettingsComponent() { GraphicOfChooseUnit = entityOfGraphic});
        }
    }
}