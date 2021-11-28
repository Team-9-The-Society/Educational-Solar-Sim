/*      Created by Logan Edmund,11/6/21
 *      Modified on 11/27/21
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




    public void Simulation1() //current preset orbit
    { 


        GameManager.Instance.DeleteAllBodies();

        //(double mass, double xLoc, double yLoc, double zLoc, double xVel, double yVel, double zVel, double scal, bool shouldFocus, string name)
        GameManager.Instance.TrySpawnNewBody(1000000000, 0, 0, 0, 0, 0, 0, 5.906, false, "Large Stationary Body");

        GameManager.Instance.TrySpawnNewBody(100000000, 20, 15, 0, 0, 0, 5, 3.458, false, "Large Moving Body");

        GameManager.Instance.FocusOnUniverse();
    }

    public void Simulation2() //large body collision
    {
        GameManager.Instance.DeleteAllBodies();

        GameManager.Instance.TrySpawnNewBody(1000000000, -10, 0, 0, 0, 0, 0, 8, false, "LargeStationaryBody");

        GameManager.Instance.TrySpawnNewBody(1000000000, 10, 0, 0, -5, 0, 0, 8, false, "LargeMovingBody");

        GameManager.Instance.FocusOnUniverse();
    }

    public void Simulation3() //small body collision
    {
        GameManager.Instance.DeleteAllBodies();

        GameManager.Instance.TrySpawnNewBody(0.000000001, -10, 0, 0, 0, 0, 0, 1, false, "SmallStationaryBpdy");

        GameManager.Instance.TrySpawnNewBody(0.000000001, 10, 0, 0, -5, 0, 0, 1, false, "SmallMovingBody");

        GameManager.Instance.FocusOnUniverse();
    }

    public void Simulation4() //large body and small body collision
    {
        GameManager.Instance.DeleteAllBodies();

        GameManager.Instance.TrySpawnNewBody(0.000000001, -10, 0, 0, 0, 0, 0, 1, false, "SmallStationaryBody");

        GameManager.Instance.TrySpawnNewBody(1000000000, 10, 0, 0, -5, 0, 0, 8, false, "LargeMovingBody");

        GameManager.Instance.FocusOnUniverse();
    }

    public void Simulation5() //circular orbit
    {
        GameManager.Instance.DeleteAllBodies();

        GameManager.Instance.TrySpawnNewBody(1000000000, 0, 0, 0, 0, 0, 0, 5.906, false, "LargeStationaryBody");

        GameManager.Instance.TrySpawnNewBody(100000000, 20, 0, 0, 0, 10, 0, 3.458, false, "LargeMovingBody");

        GameManager.Instance.FocusOnUniverse();
    }
}