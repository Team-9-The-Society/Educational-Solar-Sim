using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HomeScreneTransition : MonoBehaviour
{
    public void ToHomeScene()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
