using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
public partial class InputHandler : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutInputPlayer>();
    }
    protected override void OnUpdate()
    {
        var inputInformation = SystemAPI.GetSingleton<InformationAboutInputPlayer>();

        inputInformation.ClickDown = Input.GetMouseButtonDown(0);
        inputInformation.ClickUp = Input.GetMouseButtonUp(0);
        inputInformation.Hold = Input.GetMouseButton(0);

        float3 mousePosition = Input.mousePosition;
        inputInformation.MousePosition = new float2(mousePosition.x, mousePosition.y);

        SystemAPI.SetSingleton(inputInformation);
    }
}