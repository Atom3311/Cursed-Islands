using Unity.Entities;
using UnityEngine;
public struct C_OrientationInGame : IComponentData
{
    public Vector3 Orientation
    {
        get
        {
            return _orientation.normalized;
        }
        set
        {
            _orientation = value;
        }
    }
    [SerializeField] private Vector3 _orientation;
}