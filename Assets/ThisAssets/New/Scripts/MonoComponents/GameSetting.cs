using UnityEngine;
using Unity.Entities;
public class GameSetting : MonoBehaviour
{
    public LayerMask LayerForMove;
    private class ThisBaker : Baker<GameSetting>
    {
        public override void Bake(GameSetting authoring)
        {
            Entity targetEntity = GetEntity(TransformUsageFlags.None);
            AddComponent(targetEntity, new GameSetingsComponent() { LayersForMove = authoring.LayerForMove });
        }
    }
}