using Unity.Mathematics;
public struct RectInGame
{
    public float2 Position;
    public float2 Scale;
    private float minX
    {
        get
        {
            return math.min(Position.x, Position.x + Scale.x);
        }
    }
    private float maxX
    {
        get
        {
            return math.max(Position.x, Position.x + Scale.x);
        }
    }
    private float minY
    {
        get
        {
            return math.min(Position.y, Position.y + Scale.y);
        }
    }
    private float maxY
    {
        get
        {
            return math.max(Position.y, Position.y + Scale.y);
        }
    }
    public bool Contains(float2 point)
    {
        if (point.x < minX || point.x > maxX || point.y < minY || point.y > maxY)
            return false;
        return true;
    }
}
