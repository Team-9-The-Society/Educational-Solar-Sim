using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BodiesInfoButton : MonoBehaviour
{
    private GameManager gameManagerReference;
    public int panelExpansion;
    public int panelRedaction;
    
    [Header("Output Field References")]
    public TMP_Text displayTxt;

    void Update()
    {
        displayBodies();
    }
    public void ActivateUIElement(GameManager g)
    {
        panelExpansion = 0;
        panelRedaction = 0;
        SetGameManRef(g.GetComponent<GameManager>());
    }

    public void SetGameManRef(GameManager gm)
    {
        gameManagerReference = gm;
    }


    public void displayBodies()
    {
        // List<Body> display = gameManagerReference.SimBodies.GetClone(); //is the error with how c sharp does shallow copies?***

        //if (gameManagerReference != null)
        //{
        displayTxt.text = iterateBodies();
            
        //}
    }
    public int calculateStepHeight()
    {
        return gameManagerReference.BodyCount - 6 - panelRedaction;

    }
    public string iterateBodies()
    {
        if (gameManagerReference.BodyCount > 6 && panelExpansion ==0)
        {
            panelExpansion = 1;
            panelRedaction = calculateStepHeight();
            if (panelRedaction > 0)
            {
                displayTxt.GetComponent<RectTransform>().offsetMin += new Vector2(0, (panelRedaction) * -390);
                
            }

            //rect transform text of scrollbar add -220.3
        }
        string totalDisplay = "";
        int count = 1;
        foreach (Body b in gameManagerReference.SimBodies)
        {
            totalDisplay += "<u><b>Body Number " +count + "</u></b>\n";
            totalDisplay += returnText(b, 3);
            totalDisplay += returnText(b, 0);
            totalDisplay+= returnText(b, 1);
            totalDisplay += "\n";
            count++;
        }
        return totalDisplay;

    }
    public string returnText(Body b, int mode)
    {
        string shipped = "";
        switch (mode)
        {
            case 0:
                shipped += "X Velocity: " + b.returnRigBody().velocity.x.ToString("#.00") + "m/s\n"+ "Y Velocity: " + b.returnRigBody().velocity.y.ToString("#.00") + "m/s\n" + "Z Velocity: " + b.returnRigBody().velocity.z.ToString("#.00") + "m/s\n";
                break;
            case 1:
                shipped += "X Position: " + b.gameObject.transform.position.x.ToString("#.00") + "m\n" + "Y Position: " + b.gameObject.transform.position.y.ToString("#.00") + "m\n" + "Z Position: " + b.gameObject.transform.position.z.ToString("#.00") + "m\n";

                break;
            default:
                shipped += "Mass " + b.returnRigBody().mass.ToString("E2") + "kg\n";
                break;
        }
        return shipped;
    }

    public void HidePanel()
    {
        panelExpansion = 0;
        this.gameObject.SetActive(false);

    }



}
