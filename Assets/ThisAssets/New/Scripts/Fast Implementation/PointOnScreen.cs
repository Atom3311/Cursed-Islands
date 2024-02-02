using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public static class PointOnScreen
{
    static public bool PointOnUIElement(Vector2 point)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = point;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        return results.Count > 0;
    }
}