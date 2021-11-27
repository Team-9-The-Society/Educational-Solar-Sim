using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyIcon : MonoBehaviour
{
    private Transform cam;

    public SphereCollider planetCollider;
    public SpriteRenderer rend;
    public float turnOffDistance;

    private void Start()
    {
        cam = Camera.main.transform;
        GetComponentInParent<Canvas>().worldCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (cam == null)
            cam = Camera.main.transform;
        else
            transform.LookAt(transform.position + cam.forward);

        if (Vector3.Distance(cam.position, this.transform.position) < turnOffDistance)
        {
            rend.enabled = false;
            //planetCollider.radius = 0.5f;
        }
        else
        {
            rend.enabled = true;
            //planetCollider.radius = 3;
        }


    }


}
