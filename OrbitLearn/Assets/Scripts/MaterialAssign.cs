using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAssign : MonoBehaviour
{
    public Material[] materialList;
    public TrailRenderer tail;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, 22);
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = materialList[rand];
        Debug.Log(meshRenderer.material.color);
        tail.startColor = meshRenderer.material.color;
        tail.endColor = meshRenderer.material.color;
    }

    
}
