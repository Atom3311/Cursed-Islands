using UnityRay = UnityEngine;
using Physics = Unity.Physics;
using Unity.Mathematics;
public static class UnityRayToPhysicsRayConvertator
{
    public static Physics.Ray Convert(UnityRay.Ray targetRay)
    {
        float3 position = targetRay.origin;
        float3 direction = targetRay.direction;

        Physics.Ray finalRay = new Physics.Ray();
        finalRay.Origin = position;
        finalRay.Displacement = direction;

        return finalRay;
    } 

}