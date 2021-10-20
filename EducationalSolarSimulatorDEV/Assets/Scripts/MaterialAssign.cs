using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAssign : MonoBehaviour
{
    public Material[] materialList;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, 7);
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = materialList[rand];
    }

    
}
