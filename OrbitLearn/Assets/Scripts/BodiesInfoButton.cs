using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BodiesInfoButton : MonoBehaviour
{
    private GameManager gameManagerReference;
    
    [Header("Output Field References")]
    public TMP_Text displayTxt;

    void Update()
    {
        displayBodies();
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
    public string iterateBodies()
    {
        string totalDisplay = "";
        int count = 1;
        foreach (Body b in gameManagerReference.SimBodies)
        {
            totalDisplay += "Body Number " +count + "\n";
            totalDisplay += returnText(b, 3);
            totalDisplay += returnText(b, 0);
            
            totalDisplay+= returnText(b, 1);
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
                shipped += "X Velocity: " + b.returnRigBody().velocity.x.ToString("#.00") + "\n"+ "Y Velocity: " + b.returnRigBody().velocity.y.ToString("#.00") + "\n" + "Z Velocity: " + b.returnRigBody().velocity.z.ToString("#.00") + "\n";
                break;
            case 1:
                shipped += "X Position: " + b.gameObject.transform.position.x.ToString("#.00") + "\n" + "Y Position: " + b.gameObject.transform.position.y.ToString("#.00") + "\n" + "Z Position: " + b.gameObject.transform.position.z.ToString("#.00") + "\n";

                break;
            default:
                shipped += "Mass " + b.returnRigBody().mass + "\n";
                break;
        }
        return shipped;
    }

    public void HidePanel()
    {
        
        this.gameObject.SetActive(false);

    }



}
