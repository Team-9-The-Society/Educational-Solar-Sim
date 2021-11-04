using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BodiesInfoButton : MonoBehaviour
{
    public GameManager gameManagerReference;
    
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
            foreach (Body b in gameManagerReference.SimBodies)
            {

            displayTxt.text += returnText(b, 3);
            displayTxt.text += returnText(b, 0);
            displayTxt.text += returnText(b, 1);

            /*
                Rigidbody br = b.GetComponent<Rigidbody>();
                 displayTxt.text += "X Velocity: " + b.returnRigBody().velocity.x.ToString("#.00") + "Y Velocity: " + b.returnRigBody().velocity.y.ToString("#.00") + "Z Velocity: " + b.returnRigBody().velocity.z.ToString("#.00") + "\n";
                displayTxt.text += "X Velocity: " + br.velocity.x.ToString("#.00") + "Y Velocity: " + br.velocity.y.ToString("#.00") + "Z Velocity: " + br.velocity.z.ToString("#.00") + "\n";

            mass[i] = b.gameObject.GetComponent<Rigidbody>().mass;
            position[i, 0] = b.gameObject.transform.position.x;
            position[i, 1] = b.gameObject.transform.position.y;
            position[i, 2] = b.gameObject.transform.position.z;
            i++;
            */
        }
        //}
    }

    public string returnText(Body b, int mode)
    {
        string shipped = "";
        switch (mode)
        {
            case 0:
                shipped += "X Velocity: " + b.returnRigBody().velocity.x.ToString("#.00") +"\n"+ "Y Velocity: " + b.returnRigBody().velocity.y.ToString("#.00") + "\n" + "Z Velocity: " + b.returnRigBody().velocity.z.ToString("#.00") + "\n";
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
