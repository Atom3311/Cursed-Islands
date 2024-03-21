using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;


public class Speed : MonoBehaviour
{
    public float value;

}

public class Click : IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("1");
    }
}


public class SpeedBaker : Baker<Speed>
{
    public override void Bake(Speed authoring)
    {
        AddComponent(new SpeedData { value = authoring.value });
    }
}

public struct SpeedData : IComponentData
{
    public float value;
}