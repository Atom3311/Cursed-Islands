using Unity.Mathematics;
public static class GetRandomPointFromRect
{
    public static float3 GetRandomPoint(float3 position, float size)
    {
        size /= 2;
        float positionX = position.x;
        float positionZ = position.z;

        float3 finalPosition = new float3()
        {
            y = position.y,
            x = UnityEngine.Random.Range(positionX - size, positionX + size),
            z = UnityEngine.Random.Range(positionZ - size, positionZ + size)
        };
        return finalPosition;
    }
}