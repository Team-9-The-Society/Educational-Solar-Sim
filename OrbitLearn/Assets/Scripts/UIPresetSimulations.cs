/*      Created vy Logan Edmund,11/6/21
 * 
 *      Script used to hold and manage the information passed tothe GameManager to create/reset preset simulation scenarios
 * 
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPresetSimulations : MonoBehaviour
{

    private GameManager gameManagerReference;

    public void ActivateUIElement(GameManager g)
    {
        SetGameManRef(g.GetComponent<GameManager>());
    }

    public void SetGameManRef(GameManager gm)
    {
        gameManagerReference = gm;
    }

    //Initiates the preset simulation #1 - two bodies of e^9
    public void Simulation1()
    {
        gameManagerReference.DeleteAllBodies();

        gameManagerReference.TrySpawnNewBody(1000000000, 0, 0, 0, 0, 0, 0, 5.906, false);

        gameManagerReference.TrySpawnNewBody(100000000, 20, 0, 0, 0, 10, 0, 3.458, false);

        gameManagerReference.FocusOnUniverse();
    }
}
