using UnityEngine;
using Unity.Entities;
public class CenterOfOrdersInformation : MonoBehaviour
{
    private class ThisBaker : Baker<CenterOfOrdersInformation>
    {
        public override void Bake(CenterOfOrdersInformation authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);
            AddComponent<OrderInformation>(targetEntity);
        }
    }
}