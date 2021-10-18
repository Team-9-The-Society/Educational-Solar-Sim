using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InfoScreneTransition : MonoBehaviour
{
   public void ToInfoScene()
    {
        SceneManager.LoadScene("InfoScene");
    }
}
