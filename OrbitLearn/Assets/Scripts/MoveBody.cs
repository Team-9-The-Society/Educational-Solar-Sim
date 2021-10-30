using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBody : MonoBehaviour
{
    private Vector3 moveOffset;
    private float mouseCoordinateZ;
    // Start is called before the first frame update
    void OnMouseDown()
    {
        mouseCoordinateZ = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        moveOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    // Update is called once per frame
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePointer = Input.mousePosition;

        mousePointer.z = mouseCoordinateZ;
        return Camera.main.ScreenToWorldPoint(mousePointer);
    }
    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + moveOffset;
    }
}
