using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFilePanel : MonoBehaviour
{
    [Header("Game Manager Reference")]
    private GameManager gameManagerReference;






    public void ActivateUIElement(GameManager g)
    {
        SetGameManRef(g.GetComponent<GameManager>());
    }

    public void SetGameManRef(GameManager gm)
    {
        gameManagerReference = gm;
    }

    public void SetInput(string filename)
    {

    }

}
