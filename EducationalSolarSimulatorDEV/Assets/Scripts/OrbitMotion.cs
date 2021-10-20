using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMotion : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform orbitingPlanet;
    public float newX, newY;
    public bool check;
    public Ellipse orbitPath;

    [Range(0f, 1f)]
    public float orbitProgress = 0f;
    public float orbitPeriod = 3f;
    public bool orbitActive = true;


    void Start()
    {
        if (orbitingPlanet == null)
        {
            orbitActive = false;
        }
        else
        {
            SetOrbitingObjectPosition();
            StartCoroutine(AnimateOrbit());

        }
    }
    void SetOrbitingObjectPosition()
    {
        if (check)
        {
            orbitPath = new Ellipse(newX, newY);
            check = false;
        }
        Vector2 orbPos = orbitPath.Evaluate(orbitProgress);
        orbitingPlanet.localPosition = new Vector3(orbPos.x, 0, orbPos.y);

    }
    IEnumerator AnimateOrbit()
    {
        if (orbitPeriod < 0.1f)
            orbitPeriod = 0.1f;
        float orbitSpeed = 1f / orbitPeriod;
         while (orbitActive)
         {

             orbitProgress += Time.deltaTime * orbitSpeed;
             orbitProgress %= 1f;
             SetOrbitingObjectPosition();
             yield return null;
         }
        yield return null;
    }

    void FixedUpdate()
    {
        if (!orbitActive)
        {
            SetOrbitingObjectPosition();
            StartCoroutine(AnimateOrbit());
            orbitActive = true;
        }

    }
    /*void OnMouseDown()
    {
        Time.timeScale = 0;
    }*/
    /*IEnumerator AnimateOrbit()
    {
        if (orbitPeriod < 0.1f)
            orbitPeriod = 0.1f;
        float orbitSpeed = 1f / orbitPeriod;
        orbitProgress += Time.deltaTime * orbitSpeed;
        orbitProgress %= 1f;
        SetOrbitingObjectPosition();
        yield return null;
    }*/

}
