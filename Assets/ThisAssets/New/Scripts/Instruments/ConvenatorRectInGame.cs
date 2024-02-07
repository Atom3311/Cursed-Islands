using UnityEngine;
using Unity.Mathematics;
public static class ConvenatorRectInGame
{
    public static Rect Convert(RectInGame targetRect)
    {
        float2 finalPosition = targetRect.Position;
        float2 finalScale = targetRect.Scale;

        finalPosition.y = Screen.height - finalPosition.y;
        finalScale.y *= -1;

        Rect finalRect = new Rect(finalPosition, finalScale);

        return finalRect;
    }
}
