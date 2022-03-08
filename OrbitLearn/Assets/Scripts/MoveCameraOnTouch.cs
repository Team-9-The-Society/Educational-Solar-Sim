using UnityEngine;
using Cinemachine;

public class MoveCameraOnTouch : MonoBehaviour
{
    public float TouchSensitivityX = 100f;
    public float TouchSensitivityY = 100f;
    public GameObject VitFreeLook;

    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
        CinemachineImpulseManager.Instance.IgnoreTimeScale = true;
    }

    public float GetAxisCustom(string axisName)
    {
        if (Input.GetMouseButton(0) && !GameManager.Instance.uiPanelPriority)
        {
            var FreeLookComponent = VitFreeLook.GetComponent<CinemachineFreeLook>();
            if (GameManager.Instance.gamePaused)
            {
                FreeLookComponent.m_YAxis.m_MaxSpeed = 0.05f;
                FreeLookComponent.m_XAxis.m_MaxSpeed = 7.5f;
            }
            else
            {
                FreeLookComponent.m_YAxis.m_MaxSpeed = 0.1f;
                FreeLookComponent.m_XAxis.m_MaxSpeed = 15f;
            }
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
                return Input.GetAxis(axisName);
            }
        }
        return 0f;
    }
}