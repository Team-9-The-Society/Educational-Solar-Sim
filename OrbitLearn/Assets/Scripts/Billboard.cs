/*
 *  Lightweight script to ensure objects always face the camera.
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;

    private void LateUpdate()
    {
        if (cam == null)
            cam = Camera.main.transform;
        else
            transform.LookAt(transform.position + cam.forward);
    }
}