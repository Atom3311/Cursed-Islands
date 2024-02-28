using UnityEngine;
using UnityEngine.UI;
using System;
public class BuyUnitsParameters : MonoBehaviour
{
    public GameObject TargetUI;
    [HideInInspector] public Button[] ButtonsForSelection;

    [SerializeField] private ButtonForChange[] _buttons;

    public void Init()
    {
        foreach(ButtonForChange button in _buttons)
        {
            ButtonsForSelection[(int)button.Type] = button.Button;
        }
    }

    [Serializable]
    private struct ButtonForChange
    {
        public BuySelection Type;
        public Button Button;
    }
}