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

    public float xVelocity;
    public float yVelocity;
    public float zVelocity;

    private Rigidbody rb;

    [Header("Reference to Planet's Orbiting Camera")]
    public CinemachineFreeLook planetCam;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        ApplyForce();
    }

    public void ApplyForce()
    {
        rb.AddForce(new Vector3(xVelocity, yVelocity, zVelocity), ForceMode.Acceleration);
    }

    


}
