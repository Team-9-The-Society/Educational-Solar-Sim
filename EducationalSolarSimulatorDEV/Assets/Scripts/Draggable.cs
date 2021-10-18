/*  Created by Logan Edmund, 10/18/21
 *  
 *  Draggable.cs allows objects with this component attached to be dragged and moved by the user.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 mouseOffset;
    private float mouseZ;


    private void OnMouseDown()
    {
        mouseZ = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mouseOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mouseZ;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }


    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mouseOffset;
    }
}
