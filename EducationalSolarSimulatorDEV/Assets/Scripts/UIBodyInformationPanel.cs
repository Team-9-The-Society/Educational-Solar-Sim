/*Created by Logan Edmund, 10/14/21
 * 
 * 
 * Displays information about the currently-selected body to the relevant UI Panel
 * 
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBodyInformationPanel : MonoBehaviour
{
    public Body highlightedBody;

    public TMP_Text BodyNameDisplay;
    public TMP_Text xVelocityDisplay;
    public TMP_Text yVelocityDisplay;
    public TMP_Text zVelocityDisplay;

    public void Update()
    {
        
    }

    public void UpdateDisplay()
    {
        .ToString("#.00");
    }

    public void ClearDisplay()
    {
        BodyNameDisplay.text = "Nothing Selected";
        xVelocityDisplay.text = "X Velocity: 00.00";
        yVelocityDisplay.text = "Y Velocity: 00.00";
        zVelocityDisplay.text = "Z Velocity: 00.00";

    }



}
