using Unity.Entities;
using Unity.Mathematics;
struct C_MoveOnPoint : IComponentData
{
    public float3? PointForMove
    {
        set
        {
            pp = value;
            if (value != null)
                P = value.Value;
            else
                P = float3.zero;
        }
        get
        {
            return pp;
        }
    }
    public float3 P;
    private float3? pp;
}