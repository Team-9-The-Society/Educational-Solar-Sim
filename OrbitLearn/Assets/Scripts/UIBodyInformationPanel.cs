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
    private Rigidbody highlightedBodyRB;

    public TMP_Text BodyNameDisplay;
    public TMP_Text xVelocityDisplay;
    public TMP_Text yVelocityDisplay;
    public TMP_Text zVelocityDisplay;


    private GameManager gameManagerReference;


    private void Awake()
    {
        if (gameManagerReference == null)
        {
            GameObject g = GameObject.FindGameObjectWithTag("GameController");
            if (g != null)
            {
                SetGameManRef(g.GetComponent<GameManager>());
                gameObject.SetActive(false);
            }
        }
    }



    public void SetGameManRef(GameManager gm)
    {
        gameManagerReference = gm;
    }



    public void Update()
    {
        if (highlightedBody != null)
            UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        BodyNameDisplay.text = highlightedBody.bodyName;

        xVelocityDisplay.text = "X Velocity: " + highlightedBodyRB.velocity.x.ToString("#.00");
        yVelocityDisplay.text = "Y Velocity: " + highlightedBodyRB.velocity.y.ToString("#.00");
        zVelocityDisplay.text = "Z Velocity: " + highlightedBodyRB.velocity.z.ToString("#.00");

    }

    public void ClearDisplay()
    {
        BodyNameDisplay.text = "Nothing Selected";
        xVelocityDisplay.text = "X Velocity: 00.00";
        yVelocityDisplay.text = "Y Velocity: 00.00";
        zVelocityDisplay.text = "Z Velocity: 00.00";

    }

    public void SetHighlightedBody(Body b)
    {
        highlightedBody = b;
        highlightedBodyRB = b.GetComponent<Rigidbody>();
    }

    public void ClearHighlightedBody()
    {
        highlightedBody = null;
        highlightedBodyRB = null;
    }

    public void DeleteBody()
    {
        gameManagerReference.DeleteBody(highlightedBody);
    }

}