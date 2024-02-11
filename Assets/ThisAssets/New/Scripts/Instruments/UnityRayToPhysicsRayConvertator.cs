using Unity.Physics;
using Unity.Mathematics;
public static class GetCameraPhysicsRaycast
{
    public static RaycastInput Get(float2 pointOnScreen)
    {
        UnityEngine.Ray unityRay = UnityEngine.Camera.main.ScreenPointToRay(new float3(pointOnScreen, 0));
        RaycastInput raycastInput = new RaycastInput()
        {
            Start = unityRay.origin,
            End = unityRay.origin + unityRay.direction * Constants.MouseRange,
            Filter = CollisionFilter.Default
        };
        return raycastInput;
    }

}