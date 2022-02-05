using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MoveCameraOnTouch: MonoBehaviour
{
    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
        CinemachineImpulseManager.Instance.IgnoreTimeScale = true;

    }
    public float GetAxisCustom(string axisName)
    {
        //Touch touch = Input.GetTouch(0);

        if (axisName == "Mouse X" )
        {
            if (Input.GetMouseButton(0)&& !GameManager.Instance.uiPanelPriority)
            {
                return UnityEngine.Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            if (Input.GetMouseButton(0) && !GameManager.Instance.uiPanelPriority)
            {
                return UnityEngine.Input.GetAxis("Mouse Y");
            }
            else
            {
                return 0;
            }
        }
        return UnityEngine.Input.GetAxis(axisName);
    }
}