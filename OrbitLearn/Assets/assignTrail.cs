using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assignTrail : MonoBehaviour
{
    public ParticleSystem ps;
    public Material[] materialList;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, 5);
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = new ParticleSystem.MinMaxGradient(Color.red, Color.blue);
        var trails = ps.trails;
        trails.enabled = true;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = materialList[rand];
        var psr = GetComponent<ParticleSystemRenderer>();
        psr.trailMaterial = meshRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
