using UnityEngine;
using Unity.Entities;
public class CenterOfOrderInformation : MonoBehaviour
{
    private class ThisBaker : Baker<CenterOfOrderInformation>
    {
        public override void Bake(CenterOfOrderInformation authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);
            AddComponent<OrderInformation>(targetEntity);
        }
    }
}