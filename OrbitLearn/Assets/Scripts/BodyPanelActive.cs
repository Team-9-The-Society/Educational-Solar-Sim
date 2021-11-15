/**
 * Created by Ryan Derr 11/1/21 
 * This script is used to open and close the panel to provide info for spawning a body
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPanelActive : MonoBehaviour
{
    public GameObject BodyInfoPrompt;
    public void OpenPanel()
    {
        if(BodyInfoPrompt!=null)
        {
            BodyInfoPrompt.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        if (BodyInfoPrompt != null)
        {
            BodyInfoPrompt.SetActive(false);
        }
    }
}
