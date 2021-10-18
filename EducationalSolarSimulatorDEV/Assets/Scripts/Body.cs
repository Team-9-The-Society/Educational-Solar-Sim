/*  Created by Logan Edmund, 10/14/21
 *  
 *  Object classifier for planetary bodies.
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public string bodyName;

    public float xVelocity;
    public float yVelocity;
    public float zVelocity;

    private Rigidbody rb;

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