using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BodyPromptScript : MonoBehaviour
{
    private GameManager GameManagerReference;

    [Header("Input Field References")]
    public TMP_InputField massInput;

    public TMP_InputField xPosInput;
    public TMP_InputField yPosInput;
    public TMP_InputField zPosInput;

    public TMP_InputField xVelInput;
    public TMP_InputField yVelInput;
    public TMP_InputField zVelInput;


    [Header("Input Variables")]
    public double mass;

    public double xPos;
    public double yPos;
    public double zPos;

    public double xVel;
    public double yVel;
    public double zVel;

    public double size;

    public void ActivateUIElement(GameManager g)
    {
        SetGameManRef(g.GetComponent<GameManager>());
    }

    public void SetGameManRef(GameManager g)
    {
        GameManagerReference = g;
    }

    public void HidePanel()
    {
        this.gameObject.SetActive(false);
    }

    public void CancelNewBody()
    {
        ClearInputsAndValues();
        HidePanel();
    }

    public void SubmitNewBody()
    {
        GameManagerReference.TrySpawnNewBody(mass, xPos, yPos, zPos, xVel, yVel, zVel, size, true);
        ClearInputsAndValues();
        HidePanel();
    }


    public void ClearInputsAndValues()
    {
        massInput.text = "";

        xPosInput.text = "";
        yPosInput.text = "";
        zPosInput.text = "";

        xVelInput.text = "";
        yVelInput.text = "";
        zVelInput.text = "";

        mass = 0;
        xPos = 0;
        yPos = 0;
        zPos = 0;

        xVel = 0;
        yVel = 0;
        zVel = 0;
    }

    public double StringToDouble(string s)
    {
        return Convert.ToDouble(s);
    }


    public void SetInput(string variable)
    {
        switch (variable)
        {
            case "xPos":
                xPos = Convert.ToDouble(xPosInput.text);
                break;
            case "yPos":
                yPos = Convert.ToDouble(yPosInput.text);
                break;
            case "zPos":
                zPos = Convert.ToDouble(zPosInput.text);
                break;
            case "mass":
                mass = Convert.ToDouble(massInput.text);
                if (mass == 0)
                {
                    mass = 1;
                    throwPopUpError();
                }
                break;
            case "xVel":
                xVel = Convert.ToDouble(xVelInput.text);
                break;
            case "yVel":
                yVel = Convert.ToDouble(yVelInput.text);
                break;
            case "zVel":
                zVel = Convert.ToDouble(zVelInput.text);
                break;
        }
    }
    public void throwPopUpError()
    {

    }


}
