using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BodiesInfoButton : MonoBehaviour
{
    private GameManager gameManagerReference;
    public int panelExpansion;
    public int panelRedaction;
    public int panelLastCount = 0;
    public int buttonSpawn;
    public int panelExpansionCount = 0;
    public int buttonCount = 0;
    public List<GameObject> Buttons;

    [Header("Input Field References")]
    public GameObject buttonPrefab;
    
    [Header("Output Field References")]
    public TMP_Text displayTxt;
    public GameObject buttonPanel;
    void Update()
    {
        displayBodies();
    }
    public void ActivateUIElement(GameManager g)
    {
        panelExpansion = 0;
        panelRedaction = 0;
        buttonSpawn = 0;
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
    {//DO NOT CHANGE THE MATHHHHHH OR YOU WILL BE SORRY
        return gameManagerReference.BodyCount - 6 - panelLastCount;

    }
    public void spawnButtons(int count)
    {



        for (int loopCount = 0; loopCount < count; loopCount++)
        {
            GameObject button = (GameObject)Instantiate(buttonPrefab);
            button.transform.SetParent(buttonPanel.transform);//Setting button parent

            //Debug.Log(panelExpansionCount + " panelExpansionCount!", this);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(432, -372 * (loopCount - panelExpansionCount) + 2195+ (float)7.8 * panelExpansionCount);//Changing text
                
          

            button.GetComponentInChildren<TMP_Text>().text = "Body " + (loopCount+1);
            Buttons.Add(button);
            buttonCount++;
            //////
        }
    }
    public string iterateBodies()
    { //DO NOT CHANGE THE MATHHHHHH OR YOU WILL BE SORRY
        int knownBodyCount = gameManagerReference.BodyCount;
       
        if (gameManagerReference.BodyCount > 6 && panelExpansion ==0)
        {
            panelExpansion = 1;
            
            panelRedaction = calculateStepHeight();
            if (panelRedaction > 0)
            {
                displayTxt.GetComponent<RectTransform>().offsetMin += new Vector2(0, (panelRedaction) * -380);
                panelExpansionCount+= panelRedaction;
                panelLastCount = gameManagerReference.BodyCount -6;
            }

           

            //rect transform text of scrollbar add -220.3
        }
        if (buttonSpawn == 0 && knownBodyCount > 0)
        {
            buttonSpawn = 1;
            spawnButtons(knownBodyCount);
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
    public void killButton(ref GameObject b)
    {
        Buttons.Remove(b);
        Destroy(b);
        Debug.Log(name +" killed", this);
        buttonCount--;
    }

    public void HidePanel()
    {
        while (buttonCount > 0)
        {
            GameObject dumb = Buttons[0];
            killButton(ref dumb);
        }
        panelExpansion = 0;
        buttonSpawn = 0;
        Debug.Log(name + " Game Object Hidden!", this);
        this.gameObject.SetActive(false);

    }



}
