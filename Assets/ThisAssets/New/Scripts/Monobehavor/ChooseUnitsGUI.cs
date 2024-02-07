using UnityEngine;
public class ChooseUnitsGUI : MonoBehaviour
{
    public Rect? TargetRect;
    public Texture2D TextureForRender;
    public Color ColorForRender;
    private void OnGUI()
    {
        if (TargetRect.HasValue)
        {
            GUI.color = ColorForRender;
            GUI.DrawTexture(TargetRect.Value, TextureForRender);
        }
    }
}