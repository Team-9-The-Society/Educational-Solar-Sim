/*  Created by Logan Edmund, 10/14/21
 *  
 *  Object classifier for planetary bodies.
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Body : MonoBehaviour
{
    [Header("Planet name and velocities")]
    public string bodyName;

    private Rigidbody rb;

    [Header("Reference to Planet's Orbiting Camera")]
    public CinemachineFreeLook planetCam;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        //ApplyForce();
    }

    public void ApplyForce(double xForce, double yForce, double zForce)
    {
        rb.AddForce(new Vector3((float)xForce, (float)yForce, (float)zForce), ForceMode.Force);
    }
}
