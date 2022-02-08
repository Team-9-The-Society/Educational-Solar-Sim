/*      Created by Logan Edmund,11/6/21
 *      Modified on 11/27/21
 * 
 *      Script used to hold and manage the information passed tothe GameManager to create/reset preset simulation scenarios
 * 
 * 
 */


using System.Collections;
using System.Collections.Generic;
using System;
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
        GameManager.Instance.TrySpawnNewBody(Math.Pow(10,9), 0, 0, 0, 0, 0, 0, 5.906, false, "LrgStat", false);

        GameManager.Instance.TrySpawnNewBody(Math.Pow(10,8), 20, 15, 0, 0, 0, 5, 3.458, false, "LrgMove", false);

        GameManager.Instance.FocusOnUniverse();
    }

    public void Simulation2() //large body collision
    {
        GameManager.Instance.DeleteAllBodies();

        GameManager.Instance.TrySpawnNewBody(Math.Pow(10, 9), -10, 0, 0, 0, 0, 0, 8, false, "LrgStat", false);

        GameManager.Instance.TrySpawnNewBody(Math.Pow(10, 9), 10, 0, 0, -5, 0, 0, 8, false, "LrgMove", false);

        GameManager.Instance.FocusOnUniverse();
    }

    public void Simulation3() //small body collision
    {
        GameManager.Instance.DeleteAllBodies();

        GameManager.Instance.TrySpawnNewBody(Math.Pow(10, 9), 0, 0, 0, 0, 0, 0, 4, false, "Steven", false);

        GameManager.Instance.TrySpawnNewBody(Math.Pow(10, 9), 10, 0, 0, 0, 0, 0, 4, false, "Robert", false);

        GameManager.Instance.FocusOnUniverse();
    }

    public void Simulation4() //large body and small body collision
    {
        GameManager.Instance.DeleteAllBodies();

        GameManager.Instance.TrySpawnNewBody(Math.Pow(10, 9), 0, 0, 0, 0, 0, 0, 4, false, "StaticB", false);

        GameManager.Instance.TrySpawnNewBody(Math.Pow(10, 9), 10, 0, 0, -10, 0, 0, 4, false, "MovingB", false);

        GameManager.Instance.FocusOnUniverse();
    }

    public void Simulation5() //circular orbit
    {
        GameManager.Instance.DeleteAllBodies();

        GameManager.Instance.TrySpawnNewBody(Math.Pow(10, 9), 0, 0, 0, 0, 0, 0, 8, false, "StaticB", false);

        GameManager.Instance.TrySpawnNewBody(Math.Pow(10, 5), 10, 0, 10, 0, 7.5, 0, 8, false, "MovingB", false);

        GameManager.Instance.FocusOnUniverse();
    }

    public void HidePanel() => this.gameObject.SetActive(false);
}