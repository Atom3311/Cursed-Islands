using Unity.Entities;
using UnityEngine;
public class VisualObject : IComponentData
{
    public GameObject ThisGameObject;
    public bool IsCreated;
}