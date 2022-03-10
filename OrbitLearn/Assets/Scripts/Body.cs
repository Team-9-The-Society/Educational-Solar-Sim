using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Body : MonoBehaviour
{
    [Header("Planet name and velocities")]
    public string bodyName;
    public int bodyNumber;
    public GameObject body;
    private Rigidbody rb;
    [Header("Reference to Planet's Radiant Light Object")]
    public Light radiant; 
    [Header("Reference to Planet's Orbiting Camera")]
    public CinemachineFreeLook planetCam;

    [Header("Reference to Planet's Icon")]
    public BodyIcon icon;

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

    public void flipLight()
    {
        if (body.layer == 3)
        {
            body.layer = 6;
            radiant.enabled = false;
        }
        else
        {
            body.layer = 3;
            radiant.enabled = true;
        }
        Debug.Log("Test Radient: " + radiant.enabled);
    }

    public int returnLayer()
    {
        return body.layer;
    }

    public bool IsEqual(Body comparer)
    {
        if (comparer.bodyNumber == this.bodyNumber)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
