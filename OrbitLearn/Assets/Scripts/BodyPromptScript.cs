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

    public Slider sizeInput;

    [Header("Input Variables")]
    public double mass = 0;

    public double xPos = 0;
    public double yPos = 0;
    public double zPos = 0;

    public double xVel = 0;
    public double yVel = 0;
    public double zVel = 0;

    public double size = 1;
    public bool goodInput = true;

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
        if(goodInput)
        {
            GameManagerReference.TrySpawnNewBody(mass, xPos, yPos, zPos, xVel, yVel, zVel, size, true);
            ClearInputsAndValues();
            HidePanel();
        }
        goodInput = true;
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

        sizeInput.value = 1;

        mass = 0;
        xPos = 0;
        yPos = 0;
        zPos = 0;

        xVel = 0;
        yVel = 0;
        zVel = 0;

        size = 1;
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
                xPosInput.textComponent.color = Color.black;
                parseToDouble(ref xPosInput, ref xPos);
                break;
            case "yPos":
                yPosInput.textComponent.color = Color.black;
                parseToDouble(ref yPosInput, ref yPos);
                break;
            case "zPos":
                zPosInput.textComponent.color = Color.black;
                parseToDouble(ref zPosInput, ref zPos);
                break;
            case "mass":
                //this takes scientific notation and converts it to a double. Input: 1E+-X (1E-4)(1E+8) where if no sign is entered it's assumed positive
                massInput.textComponent.color = Color.black;
                parseToDouble(ref massInput, ref mass);
                checkInput(ref massInput, ref mass);
                break;
            case "xVel":
                xVelInput.textComponent.color = Color.black;
                parseToDouble(ref xVelInput, ref xVel);
                break;
            case "yVel":
                yVelInput.textComponent.color = Color.black;
                parseToDouble(ref yVelInput, ref yVel);
                break;
            case "zVel":
                zVelInput.textComponent.color = Color.black;
                parseToDouble(ref zVelInput, ref zVel);
                break;
            case "size":
                size = Convert.ToDouble(sizeInput.value);
                break;
        }
    }

    public void checkInput(ref TMP_InputField inputRef, ref double mass)
    {
        if((mass <= 0 || mass > (Math.Pow(10,9))) && !inputRef.text.Equals(""))
        {
            inputRef.text = "Invalid Input";
            inputRef.textComponent.color = Color.red;
            goodInput = false;
        }
    }

    public void parseToDouble(ref TMP_InputField inputRef, ref double output)
    {

        bool test = double.TryParse(inputRef.text, out output);
     
        if (test || inputRef.text.Equals(""))
        {
            double.TryParse(inputRef.text, out output);
        }
        else
        {
            inputRef.text = "Invalid Input";
            inputRef.textComponent.color = Color.red;
            goodInput = false;
        }
    }

}
