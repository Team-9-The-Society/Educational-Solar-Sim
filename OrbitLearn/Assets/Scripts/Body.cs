using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Body : MonoBehaviour
{
    [Header("Planet name and velocities")]
    public string bodyName;
    public GameObject body;
    private Rigidbody rb;

    [Header("Reference to Planet's Orbiting Camera")]
    public CinemachineFreeLook planetCam;

    [Header("Planet Spotlight")]
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
        for(int i =0; i<4; i++)
        {
            float position = 0;
            switch (i)
            {
                case 0:
                    position = (float)1.5 + (float)body.transform.localScale.x - (float)1;
                    lightArray[i].transform.position = new Vector3((float)0, (float)position, (float)0);
                    break;
                case 1:
                    position = (float)1.5 + (float)body.transform.localScale.x - (float)1;
                    lightArray[i].transform.position = new Vector3((float)position, (float)0, (float)0);
                    break;
                case 2:
                    position = (float)-1.5 - (float)body.transform.localScale.x + (float)1;
                    lightArray[i].transform.position = new Vector3((float)0, (float)position, (float)0);
                    break;
                case 3:
                    position = (float)-1.5 - (float)body.transform.localScale.x + (float)1;
                    lightArray[i].transform.position = new Vector3((float)position, (float)0, (float)0);
                    break;
            }
            lightArray[i].enabled = !lightArray[i].enabled;
        }
    }
}
