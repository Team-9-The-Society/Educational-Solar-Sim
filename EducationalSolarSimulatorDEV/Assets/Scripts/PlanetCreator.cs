//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCreator : MonoBehaviour
{
   // private Random random = new Random();
    // Start is called before the first frame update
    public void createBody()
    {
        
        float randx = Random.Range(0, 200);
        float randy = Random.Range(0f, 200f);
        float randz = Random.Range(0f, 200f);
        float randSize = Random.Range(50f, 500f);
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(randx, randy, randz);
        sphere.transform.localScale += new Vector3(randSize, randSize, randSize);
    }
}
