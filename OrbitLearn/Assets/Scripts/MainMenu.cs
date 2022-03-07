/*  Created by Logan Edmund 10/18/21
 *  
 *  
 *  Handles Main Menu stuff
 * 
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private GameManager gameManagerReference;

    public Button StartSim;
    public Button Info;
    public Button Equations;

    public bool isOpen = true;

    private void Awake()
    {
        if (gameManagerReference == null)
        {
            GameObject g = GameObject.FindGameObjectWithTag("GameController");
            if (g != null)
                SetGameManRef(g.GetComponent<GameManager>());
        }
    }

    public void SetGameManRef(GameManager gm)
    {
        gameManagerReference = gm;
    }

    public void LoadHomeScene()
    {
        gameManagerReference.LoadNewScene(SceneHandler.Scene.HomeScene);
    }


    public void LoadSimulationScene()
    {
        gameManagerReference.LoadNewScene(SceneHandler.Scene.SimulationScene);
    }

    public void LoadInfoScene()
    {
        gameManagerReference.LoadNewScene(SceneHandler.Scene.InfoScene);
    }

    public void LoadEquationsScene()
    {
        gameManagerReference.LoadNewScene(SceneHandler.Scene.EquationScene);
    }


    public void ChangeButtonInteractability()
    {
        if (StartSim != null)
        {
            StartSim.interactable = isOpen;
        }
        if (Info != null)
        {
            Info.interactable = isOpen;
        }
        if (Equations != null)
        {
            Equations.interactable = isOpen;
        }
    }

    //Triggers the panel sliding in/out of the frame
    public void ShowIdleMenu()
    {
        isOpen = !isOpen;
        ChangeButtonInteractability();
    }

    //Here lies former preset sim load.
    //Here lies former preset sim load.

    public void NotImplemented()
    {
        Debug.LogWarning("This method has not been implemented yet!");
    }





}
