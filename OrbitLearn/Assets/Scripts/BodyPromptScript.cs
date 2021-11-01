using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BodyPromptScript : MonoBehaviour
{

    public GameObject SetAttributePanel;

    public void Awake()
    {
        hidePrompt();
    }

    public void showPrompt()
    {
        if(SetAttributePanel!=null)
        {
            SetAttributePanel.SetActive(true);
        }
    }

    public void hidePrompt()
{
        if(SetAttributePanel!=null)
        {
            SetAttributePanel.SetActive(false);
        }
    }
}
