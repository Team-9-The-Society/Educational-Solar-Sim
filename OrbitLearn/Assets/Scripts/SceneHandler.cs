/*
 * Created by Logan Edmund, 10/16/21
 * 
 * Handles the loading of new scenes and the transitioning between them
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneHandler
{
    public enum Scene
    {
        None,
        LoadingScene,
        Debug_Playground,
        EquationScene,
        HomeScene,
        InfoScene,
        SimulationScene
    }

    private static Action onLoaderCallback;

    public static void Load(Scene scene)
    {

        /*
            SceneManager.LoadScene(Scene.LoadingScene.ToString());

        onLoaderCallback = () =>
        {
            SceneManager.LoadScene(scene.ToString());
        };

        */
        SceneManager.LoadScene(scene.ToString());

    }

    public static void LoadScreenCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
