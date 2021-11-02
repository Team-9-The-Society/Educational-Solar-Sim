using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BodyPromptScript : MonoBehaviour
{
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

    public void showPrompt()
    {
        this.gameObject.SetActive(true);
    }

    public void hidePrompt()
    {
        this.gameObject.SetActive(false);
    }

    public void ClearInputsAndValues()
    {

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


}
