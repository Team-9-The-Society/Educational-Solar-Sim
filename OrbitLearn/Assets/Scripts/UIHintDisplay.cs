/*  Created by Logan Edmund, 11/29/21
 *  
 *   
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHintDisplay : MonoBehaviour
{
    public TMP_Text hint1;
    public TMP_Text hint2;

    public void Awake()
    {
        ClearMessageText();
    }

    public void SetMessageText(string msg1, string msg2)
    {
        hint1.text = msg1;
        hint2.text = msg2;
    }

    public void ClearMessageText()
    {
        hint1.text = "";
        hint2.text = "";
    }




}
