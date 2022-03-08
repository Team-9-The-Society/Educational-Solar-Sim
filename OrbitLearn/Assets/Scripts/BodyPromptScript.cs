using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class BodyPromptScript : MonoBehaviour
{
    private GameManager GameManagerReference;
    public Body passedBody;

    [Header("Input Field References")]
    public TMP_InputField massInput;
    public TMP_InputField nameInput;
    public TMP_InputField xPosInput;
    public TMP_InputField yPosInput;
    public TMP_InputField zPosInput;
    public TMP_InputField xVelInput;
    public TMP_InputField yVelInput;
    public TMP_InputField zVelInput;
    public Toggle glowToggle;
    public Slider sizeInput;

    [Header("Input Variables")]
    public double mass;
    public string bodyName;
    public double xPos;
    public double yPos;
    public double zPos;

    public double xVel;
    public double yVel;
    public double zVel;

    public double size = 1;
    public bool goodInput = true;

    public bool editMode = false;

    public void ActivateUIElement(GameManager g)
    {
        SetGameManRef(g.GetComponent<GameManager>());
        ClearInputsAndValues();
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
        finalCheck();
        if (!editMode)
        {
            if (goodInput)
            {
                if(bodyName == "")
                {
                    bodyName = "Body " + (GameManagerReference.BodyCount + 1);
                }
                GameManagerReference.TrySpawnNewBody(mass, xPos, yPos, zPos, xVel, yVel, zVel, size, true, bodyName, glowToggle.isOn);
                ClearInputsAndValues();
                HidePanel();
            }
            goodInput = true;
        }
        else
        {
            if (goodInput)
            {
                //Check here
                if ((passedBody.returnLayer() == 6 && glowToggle.isOn) || (passedBody.returnLayer() == 3 && !glowToggle.isOn))
                {
                    passedBody.flipLight();
                }
                Rigidbody r = passedBody.gameObject.GetComponent<Rigidbody>();
                passedBody.transform.position = new Vector3((float)xPos, (float)yPos, (float)zPos);
                passedBody.transform.localScale = new Vector3((float)size, (float)size, (float)size);
                if (bodyName == "")
                {
                    bodyName = "Body" + (GameManagerReference.BodyCount);
                }
                passedBody.bodyName = bodyName;
                float camOrbit = (float)((size * 8) + 27) / 7;
                passedBody.planetCam.m_Orbits[0] = new CinemachineFreeLook.Orbit(camOrbit, 0.1f);
                passedBody.planetCam.m_Orbits[1] = new CinemachineFreeLook.Orbit(0, camOrbit);
                passedBody.planetCam.m_Orbits[2] = new CinemachineFreeLook.Orbit(-camOrbit, 0.1f);

                r.mass = (float)mass;
                r.velocity = (new Vector3((float)xVel, (float)yVel, (float)zVel));

                ClearInputsAndValues();
                HidePanel();
                editMode = false;
                passedBody = null;
            }
            goodInput = true;
        }
    }


    public void ClearInputsAndValues()
    {
        massInput.text = "";
        nameInput.text =  "";
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
        glowToggle.isOn = false;
    }

    public void SetInput(string variable)
    {
        switch (variable)
        {
            case "xPos":
                xPosInput.textComponent.color = Color.black;
                xPos = parseToDouble(ref xPosInput);
                break;
            case "yPos":
                yPosInput.textComponent.color = Color.black;
                yPos = parseToDouble(ref yPosInput);
                break;
            case "zPos":
                zPosInput.textComponent.color = Color.black;
                zPos =  parseToDouble(ref zPosInput);
                break;
            case "mass":
                //this takes scientific notation and converts it to a double. Input: 1E+-X (1E-4)(1E+8) where if no sign is entered it's assumed positive
                massInput.textComponent.color = Color.black;
                mass = parseToDouble(ref massInput);
                checkInputMass(ref massInput, ref mass);
                break;
            case "xVel":
                xVelInput.textComponent.color = Color.black;
                xVel = parseToDouble(ref xVelInput);
                checkSpeed(ref xVel, ref xVelInput);
                break;
            case "yVel":
                yVelInput.textComponent.color = Color.black;
                yVel = parseToDouble(ref yVelInput);
                checkSpeed(ref yVel, ref yVelInput);
                break;
            case "zVel":
                zVelInput.textComponent.color = Color.black;
                zVel = parseToDouble(ref zVelInput);
                checkSpeed(ref zVel, ref zVelInput);
                break;
            case "size":
                size = Convert.ToDouble(sizeInput.value);
                break;
            case "name":
                bodyName = nameInput.text;
                break;
        }
    }

    public void checkInputMass(ref TMP_InputField inputRef, ref double mass)
    {
        if((mass <= 0 || mass > (Math.Pow(10,9))) && !inputRef.text.Equals(""))
        {
            inputRef.text = "Invalid Input";
            inputRef.textComponent.color = Color.red;
            goodInput = false;
        }
    }

    public void checkSpeed(ref double speed, ref TMP_InputField inputRef)
    {
        if(speed > 200 || speed < -200)
        {
            inputRef.text = "Invalid Input";
            inputRef.textComponent.color = Color.red;
            goodInput = false;
        }
    }

    public double parseToDouble(ref TMP_InputField inputRef)
    {
        double output;
        bool test = double.TryParse(inputRef.text, out output);
     
        if (test || inputRef.text.Equals(""))
        {
            double.TryParse(inputRef.text, out output);
            return output;
        }
        else
        {
            inputRef.text = "Invalid Input";
            inputRef.textComponent.color = Color.red;
            goodInput = false;
            return 0;
        }
    }

    public void beginEdit(ref Body b)
    {
        editMode = true;
        passedBody = b;
        nameInput.text = b.bodyName;
        xVelInput.text = passedBody.returnRigBody().velocity.x.ToString("#.00");
        yVelInput.text = passedBody.returnRigBody().velocity.y.ToString("#.00");
        zVelInput.text = passedBody.returnRigBody().velocity.z.ToString("#.00");
        xPosInput.text = passedBody.gameObject.transform.position.x.ToString("#.00");
        yPosInput.text = passedBody.gameObject.transform.position.y.ToString("#.00");
        zPosInput.text = passedBody.gameObject.transform.position.z.ToString("#.00");
        massInput.text = passedBody.returnRigBody().mass.ToString("E2");
        sizeInput.value = passedBody.transform.localScale.x;
       if(passedBody.returnLayer() == 3)
        {
            glowToggle.isOn = true;
        }
       else
        {
            glowToggle.isOn = false;
        }
    }

    public void finalCheck()
    {
        SetInput("xPos");
        SetInput("name");
        SetInput("yPos");
        SetInput("zPos");
        SetInput("yVel");
        SetInput("xVel");
        SetInput("zVel");
        SetInput("mass");
    }

    void Update()
    {
        nameInput.characterLimit = 7;
    }
}
