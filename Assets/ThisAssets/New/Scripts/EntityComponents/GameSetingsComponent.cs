using Unity.Entities;
using UnityEngine;
struct GameSetingsComponent : IComponentData
{
    public LayerMask LayersForMove;
}