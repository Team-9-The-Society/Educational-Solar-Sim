using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIFilePanel : MonoBehaviour
{
    [Header("Game Manager Reference")]
    private GameManager gameManagerReference;

    [Header("Input Field References")]
    public TMP_InputField file;


    [Header("Input Variables")]
    public string fileImportName;


    public void ActivateUIElement(GameManager g)
    {
        SetGameManRef(g.GetComponent<GameManager>());
    }

    public void SetGameManRef(GameManager gm)
    {
        gameManagerReference = gm;
    }



    public void HideFilePanel()
    {
        this.gameObject.SetActive(false);
    }


    public void ExportSim()
    {
        gameManagerReference.ExportSimulation();
    }


    public void SendInput()
    {
        if (name != null)
        {
            gameManagerReference.SetImportString(fileImportName);
        }
    }

    public void SetInput(string filename)
    {
        switch (filename)
        {
            case "name":
                fileImportName = file.text;
                break;
        }

    }







}
