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

    [Header("Debug - Force Change Current Velocity")]
    public double dxVel;
    public double dyVel;
    public double dzVel;
    public bool DebugSetNewVelocity;

    [Header("Rotation")]
    public float rotSpeed;
    public float rotAxis;
    public Quaternion curRot;
    public Vector3 curEuler;
    public float x, y, z;


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rotSpeed = Random.Range(-100.0f, 100.0f);
        rotAxis = Random.Range(0.0f, 100.0f);

        x = 0;
        y = 0;
        z = 0;
        if (rotAxis > 80)
        {
            x = rotAxis / 10;
        }
        else if (rotAxis > 60)
        {
            z = rotAxis / 10;
        }
        else
        {
            y = rotAxis / 10;
        }
        curEuler += new Vector3(20, 0, 0) * Time.deltaTime * rotSpeed;
        curRot.eulerAngles = curEuler;
        rb.transform.rotation = curRot;
        curEuler = new Vector3(0, 0, 0) * Time.deltaTime * rotSpeed;
        curRot.eulerAngles = curEuler;
        rb.transform.rotation = curRot;
    }


    private void FixedUpdate()
    {
        if (DebugSetNewVelocity)
        {
            DEBUGForceVelocityUpdate();
        }
    }

    public void UpdateRotation()
    {
        curEuler += new Vector3(x, y, z) * Time.deltaTime * rotSpeed;
        curRot.eulerAngles = curEuler;
        rb.transform.rotation = curRot;
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
