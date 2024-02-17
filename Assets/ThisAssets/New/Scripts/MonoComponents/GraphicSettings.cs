using Unity.Entities;
using UnityEngine;
public class GraphicSettings : MonoBehaviour
{
    public GameObject GraphicOfChooseMovableUnit;
    public GameObject GraphicOfChooseResource;
    private class ThisBaker : Baker<GraphicSettings>
    {
        public override void Bake(GraphicSettings authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);

            Entity entityOfMovableUnitGraphic = GetEntity(authoring.GraphicOfChooseMovableUnit, TransformUsageFlags.Dynamic);
            Entity entityOfResourceGraphic = GetEntity(authoring.GraphicOfChooseResource, TransformUsageFlags.Dynamic);

            AddComponent(targetEntity, new GraphicSettingsComponent()
            {
                GraphicOfChooseMovableUnit = entityOfMovableUnitGraphic,
                GraphicOfChooseResource = entityOfResourceGraphic
            });
        }
    }
}