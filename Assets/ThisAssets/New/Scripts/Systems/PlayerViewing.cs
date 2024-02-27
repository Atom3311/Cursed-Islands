using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
[UpdateAfter(typeof(InputHandler))]
public partial class PlayerViewing : SystemBase
{
    private InformationAboutControlMode _controlMode;
    private InformationAboutInputPlayer _input;
    private float2? _previousPositionClik;
    protected override void OnUpdate()
    {
        _controlMode = SystemAPI.GetSingleton<InformationAboutControlMode>();
        _input = SystemAPI.GetSingleton<InformationAboutInputPlayer>();

        if (_controlMode.ControlMode != ControlMode.Viewing)
            return;

        if (_input.ClickUp)
            _previousPositionClik = null;

        float2 duringMousePosition = _input.MousePosition;

        if (_input.ClickDown && !PointOnScreen.PointOnUIElement(duringMousePosition))
            _previousPositionClik = _input.MousePosition;

        if (!_previousPositionClik.HasValue)
            return;

        if (!_input.Hold)
            return;

        float2 translate = (_previousPositionClik.Value - duringMousePosition) * Constants.SpeedOffsetCameraOnPixel;
        Transform cameraTransform = Camera.main.transform;
        cameraTransform.Translate(math.right() * translate.x);
        float3 forward = cameraTransform.forward;
        float3 finalForward = math.normalize(new float3(forward.x, 0, forward.z));
        cameraTransform.Translate(finalForward * translate.y, Space.World);

        _previousPositionClik = duringMousePosition;

    }
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutControlMode>();
        RequireForUpdate<InformationAboutInputPlayer>();
    }
}