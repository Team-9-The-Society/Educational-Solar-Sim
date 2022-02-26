using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MoveCameraOnTouch : MonoBehaviour
{
    public float TouchSensitivityX = 100f;
    public float TouchSensitivityY = 100f;


    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
        CinemachineImpulseManager.Instance.IgnoreTimeScale = true;

    }
    public float GetAxisCustom(string axisName)
    {
        if (Input.GetMouseButton(0) && !GameManager.Instance.uiPanelPriority)
        {
            if (Input.touchCount > 0)
            {
                switch (axisName)
                {
                    case "Mouse X":
                        return Input.touches[0].deltaPosition.x / TouchSensitivityX;
                    case "Mouse Y":
                        return Input.touches[0].deltaPosition.y / TouchSensitivityY;
                    default:
                        Debug.LogError("Input <" + axisName + "> not recognyzed.", this);
                        break;
                }
            }
            else
            {
                return Input.GetAxisRaw(axisName);
            }
        }
        return 0f;
    }
}