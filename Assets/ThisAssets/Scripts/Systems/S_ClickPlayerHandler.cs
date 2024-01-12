using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using System;
public partial class S_ClickPlayerHandler : SystemBase
{
    public static Action<RaycastHit> Handled;
    private LayerMask _layersForHandle;
    protected override void OnCreate()
    {
        _layersForHandle = Parameters.GetParameters().LayersForContactWithPlayerClik;
        S_PlayerActions.Cliked += HandleClick;
    }
    protected override void OnDestroy()
    {
        S_PlayerActions.Cliked -= HandleClick;
    }
    private void HandleClick(float2 point)
    {
        Ray ray = Camera.main.ScreenPointToRay((Vector2)point);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, _layersForHandle);
        if (hit.transform)
        {
            Handled?.Invoke(hit);
        }
    }

    protected override void OnUpdate(){}
}