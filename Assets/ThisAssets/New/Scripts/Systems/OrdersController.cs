using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Collections;
using System;
[UpdateAfter(typeof(InputHandler))]
public partial struct OrdersController : ISystem
{
    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<InformationAboutInputPlayer>();
        state.RequireForUpdate<InformationAboutControlMode>();
        state.RequireForUpdate<OrderInformation>();
    }
    private void OnUpdate(ref SystemState state)
    {
        var input = SystemAPI.GetSingleton<InformationAboutInputPlayer>();
        var collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
        var controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();

        OrderInformation newInformation = new OrderInformation()
        {
            DuringOrder = Order.None,
            TargetEntity = Entity.Null,
            TargetPoint = float3.zero
        };

        if (controlMode.ControlMode != ControlMode.Order)
        {
            SystemAPI.SetSingleton(newInformation);
            return;
        }

        if (!(input.ClickDown && !PointOnScreen.PointOnUIElement(input.MousePosition)))
        {
            SystemAPI.SetSingleton(newInformation);
            return;
        }

        float2 mousePosition = input.MousePosition;
        RaycastInput raycast = GetCameraPhysicsRaycast.Get(mousePosition);
        NativeList<RaycastHit> hits = new NativeList<RaycastHit>(Allocator.FirstUserIndex);
        collisionWorld.CastRay(raycast, ref hits);
        RaycastHit[] standartArrayWithHits = hits.AsArray().ToArray();

        Array.Sort(standartArrayWithHits, (hit1, hit2) => 
        {
            float3 startPosition = raycast.Start;
            float firstDistance = math.length(hit1.Position - startPosition);
            float secondDistance = math.length(hit2.Position - startPosition);
            return firstDistance.CompareTo(secondDistance);
        });

        foreach (RaycastHit hit in standartArrayWithHits)
        {
            Entity hitEntity = hit.Entity;
            if (SystemAPI.HasComponent<HealthState>(hitEntity))
            {
                HealthState healthState = SystemAPI.GetComponent<HealthState>(hitEntity);
                OwnersInGame targetOwner = SystemAPI.GetComponent<OwnerComponent>(hitEntity).Owner;

                if (healthState.IsDead || targetOwner == OwnersInGame.Player)
                    continue;

                newInformation.DuringOrder = Order.Attack;
                newInformation.TargetEntity = hitEntity;
                break;
            }
            else if (SystemAPI.HasComponent<ResourceInformation>(hitEntity))
            {
                newInformation.DuringOrder = Order.Extruct;
                newInformation.TargetEntity = hitEntity;
                break;
            }
            else if (SystemAPI.HasComponent<SurfaceForMove>(hitEntity))
            {
                newInformation.DuringOrder = Order.Move;
                newInformation.TargetPoint = hit.Position;
                break;
            }
            else if(SystemAPI.HasComponent<Foundation>(hitEntity))
            {
                newInformation.DuringOrder = Order.Build;
                newInformation.TargetEntity = hitEntity;
                break;
            }
            else if (SystemAPI.HasComponent<PlayerBuilding>(hitEntity))
            {
                newInformation.DuringOrder = Order.Buy;
                newInformation.TargetEntity = hitEntity;
                break;
            }
        }

        SystemAPI.SetSingleton(newInformation);
    }
}