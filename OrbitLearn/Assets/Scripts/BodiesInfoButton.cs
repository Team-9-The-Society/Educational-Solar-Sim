using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public int presetFilm = 0;
    
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
        displayTxt.text = iterateBodies();
    }
    public int calculateStepHeight()
    {//DO NOT CHANGE THE MATHHHHHH OR YOU WILL BE SORRY
        return gameManagerReference.BodyCount - 1 - panelLastCount;//- 6 -

    }
    public float[] getScreenplier()
    {
        float screenplier = ((((float)Screen.height) / ((float)Screen.width)) / ((float)1920 / 1080));
        screenplier = screenplier - 2 * (screenplier - 1) / 5;
        int xPosition = 442;

        if (presetFilm == 1)
        {
            screenplier = 1900 * screenplier;

        }
        else
        {
            screenplier = 2195 * screenplier;
            xPosition = 465;
        }
        float[] screen = new float[2];
        screen[0] = screenplier;
        screen[1] = (float)xPosition;
        return screen;
    }

    public float offsetHeightCalc(float baseC)  
    {
        return baseC*(float)Screen.height / 1920;
    }

    public float offsetWidthCalc(float baseC) 
    {
        return baseC*(float)Screen.width / 1080;
    }

    public float buttonYPlacement(int loopCount, float screenplier)
    {
        if (presetFilm == 1)
        {
            return (float)-455.5 * (loopCount - panelExpansionCount) - 1325 + screenplier + (float)2.8 * panelExpansionCount;
        }
        return (float)-455.5 * (loopCount - panelExpansionCount) - 1500 + screenplier + (float)2.8 * panelExpansionCount;
    }

    public void spawnButtons(int count)
    {
        List<Body> tmp = new List<Body>();
        foreach (Body b in gameManagerReference.SimBodies)
        {
            tmp.Add(b);
        }
        float [] screenplierArr = getScreenplier();
        float screenplier = screenplierArr[0];
        int xPosition = (int)screenplierArr[1];

        float baseHeightDisplay = 150, baseWidthDisplay = 320;
        float textSizeIndex = (float)320/68;

        float offsetHeight = offsetHeightCalc(baseHeightDisplay);
        float offsetWidth = offsetWidthCalc(baseWidthDisplay);
        //1.778 is the ratio of height to width that has a favored starting offset.
        // this needs to and accounts for different phone sizes for button initial offset.


        for (int loopCount = 0;loopCount<count;loopCount++)
        {
            int num = loopCount;
            GameObject button = Instantiate(buttonPrefab);

            button.GetComponent<Button>().onClick.AddListener(() => FocusOnPlanet(num));
            button.transform.SetParent(buttonPanel.transform);//Setting button parent

            //Debug.Log(panelExpansionCount + " panelExpansionCount!", this);//372
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPosition, buttonYPlacement(num, screenplier));//Changing text
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(offsetWidth, offsetHeight);

          
            button.GetComponentInChildren<TMP_Text>().text = tmp[num].bodyName;
            button.GetComponentInChildren<TMP_Text>().fontSize = offsetWidth/textSizeIndex;//((float)Screen.width * (float)Screen.height) / (baseHeightDisplay * baseWidthDisplay);
            Buttons.Add(button);
            buttonCount++;
            
            //////
        }
    }
    public void panelGrowth()
    {
        panelExpansion = 1;

        panelRedaction = calculateStepHeight();
       
            
        displayTxt.GetComponent<RectTransform>().offsetMin += new Vector2(0, (panelRedaction) * -458);
        panelExpansionCount += panelRedaction;
            
        panelLastCount = gameManagerReference.BodyCount - 1;
        



        //rect transform text of scrollbar add -458.
    }
    public string iterateBodies()
    { //DO NOT CHANGE THE MATHHHHHH OR YOU WILL BE SORRY
        int knownBodyCount = gameManagerReference.BodyCount;
        //growing panel
        if (gameManagerReference.BodyCount > 0 && panelExpansion ==0)
        {
            panelGrowth();
        }
        //spawning buttons
        if (buttonSpawn == 0 && knownBodyCount > 0)
        {
            buttonSpawn = 1;
            spawnButtons(knownBodyCount);
        }
        //no buttons/bodies display
        string totalDisplay = "";
        if (knownBodyCount == 0)
        {
            totalDisplay += "No Bodies are currently displayed. Please add a body for their information to be displayed promply here.";
        }

        //PrintOut
        totalDisplay += iterateBodyInformation(totalDisplay);
        
        return totalDisplay;

    }


    public string iterateBodyInformation(string totalDisplay)
    {

        int count = 1;
        foreach (Body b in gameManagerReference.SimBodies)
        {
            totalDisplay += "<u><b>Body Name:</u></b> " + b.bodyName + "\n";

            totalDisplay += returnText(b, 3);
            totalDisplay += returnText(b, 0);
            totalDisplay += returnText(b, 1);
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
                shipped += "X Velocity: " + b.returnRigBody().velocity.x.ToString("#.00") + "m/s\n"+ "Y Velocity: " + b.returnRigBody().velocity.y.ToString("#.00") + "m/s\n" + "Z Velocity: " + b.returnRigBody().velocity.z.ToString("#.00") + "m/s\n\n";
                break;
            case 1:
                shipped += "X Position: " + b.gameObject.transform.position.x.ToString("#.00") + "m\n" + "Y Position: " + b.gameObject.transform.position.y.ToString("#.00") + "m\n" + "Z Position: " + b.gameObject.transform.position.z.ToString("#.00") + "m\n";

                break;
            default:
                shipped += "Mass " + b.returnRigBody().mass.ToString("E2") + "kg\n\n";
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

    public void FocusOnPlanet(int i)
    {
        try
        {
            Debug.LogWarning("Running FocusOnPlanet for i=" + i);
            gameManagerReference.ShowBodyInfo(gameManagerReference.SimBodies[i]);
            gameManagerReference.ActivatePlanetCam(gameManagerReference.SimBodies[i].planetCam);

            HidePanel();
        }
        catch
        {
            Debug.LogError("Error -- Invalid Index " + i);
        }
    }

}
