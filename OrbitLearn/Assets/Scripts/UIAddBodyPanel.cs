
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAddBodyPanel : MonoBehaviour
{

    private GameManager gameManagerReference;


    public void SetGameManRef(GameManager gm)
    {
        gameManagerReference = gm;
    }


    public void SpawnNewBody()
    {
        gameManagerReference.TrySpawnNewBody();
    }


}
