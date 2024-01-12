using Unity.Entities;
using UnityEngine;
public struct C_SpeedOfMovement : IComponentData
{
    public float Speed 
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = Mathf.Max(0 , value);
        }
    }

    [Min(0)]
    [SerializeField]
    private float _speed;

    public C_SpeedOfMovement(float speed)
    {
        _speed = 0;
        Speed = speed;
    }
}