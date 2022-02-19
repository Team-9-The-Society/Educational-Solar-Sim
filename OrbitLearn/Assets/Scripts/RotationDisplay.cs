/*
 *  Causes an object to ignore the rotation of a parent object by manually setting rotation to zero
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationDisplay : MonoBehaviour
{
    public Quaternion rotOriginal = Quaternion.Euler(0, 0, 0);
    private GameManager gameManagerReference;
    private void Awake()
    {
        //Transform cam = Camera.main.transform;
        //rotOriginal = Quaternion.LookRotation(cam.transform.position - transform.position);
        //rotOriginal = this.transform.rotation;
    }

    public void ActivateUIElement(GameManager g)
    {
        SetGameManRef(g.GetComponent<GameManager>());
    }

    //Sets the reference to the game manager
    public void SetGameManRef(GameManager gm)
    {
        gameManagerReference = gm;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rot = Camera.main.transform.rotation;
        this.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
    }
}
