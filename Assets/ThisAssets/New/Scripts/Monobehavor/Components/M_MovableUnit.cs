using UnityEngine;
[RequireComponent(typeof(M_UnitWithOwner))]
public class M_MovableUnit : MonoBehaviour
{
    [Min(0)] public float speed;
}
