/*
 *  Causes an object to ignore the rotation of a parent object by manually setting rotation to zero
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreParentRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
