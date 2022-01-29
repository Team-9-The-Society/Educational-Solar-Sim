
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


    public Light[] lightArray;

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

    public void flipLight()
    {
        foreach(Light l in lightArray)
        {
            l.enabled = !l.enabled;
        }
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
