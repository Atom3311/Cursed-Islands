using Unity.Entities;
using UnityEngine;
using System;
using Unity.Mathematics;
public partial class S_PlayerActions : SystemBase
{
    public static Action<float2> Cliked;
    protected override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
            Cliked?.Invoke((Vector2)Input.mousePosition);
    }
}