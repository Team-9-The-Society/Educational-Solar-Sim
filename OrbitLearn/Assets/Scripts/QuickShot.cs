using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickShot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject g in objs)
        {
            Debug.Log("Scanning " + g.name + " for UIReferences");

            UIRefHandler b = g.GetComponent<UIRefHandler>();

            if (b != null)
            {
                b.PresetSimulationsRef.Simulation1();
                return;
            }
        }
    }
}
