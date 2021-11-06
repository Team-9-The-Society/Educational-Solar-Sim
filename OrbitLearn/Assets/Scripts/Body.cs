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


    [Header("Debug - Force Change Current Velocity")]
    public double dxVel;
    public double dyVel;
    public double dzVel;
    public bool DebugSetNewVelocity;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        if (DebugSetNewVelocity)
        {
            DEBUGForceVelocityUpdate();
        }
    }

    public void ApplyForce(double xForce, double yForce, double zForce)
    {
        Debug.Log($"Forced applied to {gameObject.name}: {xForce}, {yForce}, {zForce}");
        rb.AddForce(new Vector3((float)xForce, (float)yForce, (float)zForce), ForceMode.Force);
    }

    public void DEBUGForceVelocityUpdate()
    {
        rb.velocity = new Vector3((float)dxVel, (float)dyVel, (float)dzVel);
        DebugSetNewVelocity = false;
    }
    public Rigidbody returnRigBody()
    {
        return rb;
    }
}
