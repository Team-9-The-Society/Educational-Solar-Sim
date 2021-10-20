using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAssign : MonoBehaviour
{
    public Material newMaterial;
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        int rand = Random.Range(0, 6);
        switch (rand)
        {
           /* case 0: meshRenderer.material = Black_Moon;
                break;
            case 1: meshRenderer.material = Blue_Moon;
                break;
            case 2:meshRenderer.material = Blue_Moon; 
                break;
            case 3:
                meshRenderer.material = Blue_Moon ;
                break;
            case 4:
                meshRenderer.material = Blue_Moon;
                break;
            case 5:
                meshRenderer.material = Blue_Moon ;
                break;
            case 6:
                meshRenderer.material = Blue_Moon ;
                break;
            default:
                meshRenderer.material = Blue_Moon;
                break;*/
        }
    }

    
}
