using Unity.Entities;
using UnityEngine;
public partial class S_SpreaderHandleOfPlayerClick : SystemBase
{
    protected override void OnUpdate(){}
    protected override void OnCreate()
    {
        S_ClickPlayerHandler.Handled += SpreadeInformationAboutHit;
    }
    protected override void OnDestroy()
    {
        S_ClickPlayerHandler.Handled -= SpreadeInformationAboutHit;
    }
    private void SpreadeInformationAboutHit(RaycastHit hit)
    {
        foreach (RefRW<C_MoveOnPoint> componentOfMove in SystemAPI.Query<RefRW<C_MoveOnPoint>>())
        {
            componentOfMove.ValueRW.PointForMove = hit.point;
        }
    }
}