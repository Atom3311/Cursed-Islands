using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class CanChangeControlMode : MonoBehaviour
{
    public Button ThisButton;
    public ControlMode TargetMode;
    private void Awake()
    {
        ThisButton = GetComponent<Button>();
    }
}
