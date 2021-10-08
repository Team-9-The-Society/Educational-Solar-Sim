using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationScreenTransition : MonoBehaviour
{
    public void ToSimulationScreen()
    {
        SceneManager.LoadScene("SimulationScene");
    }
}
