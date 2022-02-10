using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MoveCameraOnTouch: MonoBehaviour
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
        //Touch touch = Input.GetTouch(0);

        if (Input.GetMouseButton(0) && !GameManager.Instance.uiPanelPriority)
        {
            switch (axisName)
            {
                case "Mouse X":
                    if (Input.touchCount > 0)
                    {
                        return Input.touches[0].deltaPosition.x / TouchSensitivityX;
                    }
                    else
                    {
                        return Input.GetAxis(axisName);
                    }
                case "Mouse Y":
                    if (Input.touchCount > 0)
                    {
                        return Input.touches[0].deltaPosition.y / TouchSensitivityY;
                    }
                    else
                    {
                        return Input.GetAxis(axisName);
                    }
                default:
                    Debug.LogError("Input <" + axisName + "> not recognyzed.", this);
                    break;
            }
        }
            
        return 0f;
    }
}