using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Physics = Unity.Physics;
using Unity.Physics;
using Unity.Collections;
[UpdateAfter(typeof(InputHandler))]
partial class PeddlerPlayerMovePoint : SystemBase
{
    private InformationAboutControlMode _controlMode;
    private CollisionWorld _collisionWorld;
    private InformationAboutInputPlayer _input;
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutControlMode>();
        RequireForUpdate<InformationAboutInputPlayer>();
    }
    protected override void OnUpdate()
    {
        _controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();

        if (_controlMode.ControlMode != ControlMode.Move)
            return;

        _input = SystemAPI.GetSingleton<InformationAboutInputPlayer>();
        float2 pointOnScreen = _input.MousePosition;

        if (!(_input.ClickDown && !PointOnScreen.PointOnUIElement(pointOnScreen)))
            return;

        _collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
        #region Paddle


        UnityEngine.Ray rayFromScreen = Camera.main.ScreenPointToRay(new float3(pointOnScreen, 0));
        RaycastInput raycastInput = new RaycastInput()
        {
            Start = rayFromScreen.origin,
            End = rayFromScreen.origin + rayFromScreen.direction * Constants.MouseRange,
            Filter = CollisionFilter.Default
        };
        NativeList<Physics.RaycastHit> listOfHits = new NativeList<Physics.RaycastHit>(Allocator.Temp);

        _collisionWorld.CastRay(raycastInput, ref listOfHits);

        float3? finalPointForMove = null;

        foreach (Physics.RaycastHit hit in listOfHits)
        {
            Entity entityOfHit = hit.Entity;
            if (SystemAPI.HasComponent<SurfaceForMove>(entityOfHit))
            {
                finalPointForMove = hit.Position;
                break;
            }
        }
        if (!finalPointForMove.HasValue)
            return;

        NativeList<RefRW<MovePoint>> listOfEntities = new NativeList<RefRW<MovePoint>>(Allocator.Temp);

        foreach ((
            RefRW<MovePoint> movePoint,
            RefRO<OwnerComponent> owner,
            RefRO<ChoosedUnit> choosedUnit,
            Entity entity)
            in SystemAPI.Query<
                RefRW<MovePoint>,
                RefRO<OwnerComponent>,
                RefRO<ChoosedUnit>>().WithEntityAccess())
        {
            if (owner.ValueRO.Owner != OwnersInGame.Player)
                continue;
            listOfEntities.Add(movePoint);
        }

        int countOfEntities = listOfEntities.AsArray().Length;
        foreach (RefRW<MovePoint> movePoint in listOfEntities)
        {
            if (countOfEntities == 1)
            {
                movePoint.ValueRW.PointInWorld = finalPointForMove.Value;
            }
            else
            {
                float3 randomPoint = GetRandomPointFromRect.GetRandomPoint(finalPointForMove.Value, math.sqrt(Constants.RectForUnit * countOfEntities));
                movePoint.ValueRW.PointInWorld = randomPoint;
            }
        }

        #endregion
    }
}