/*  Created by Logan Edmund, 10/16/21
 * 
 *  User interacts with this panel to trigger the spawning of a new body
 * 
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAddBodyPanel : MonoBehaviour
{

    private GameManager gameManagerReference;


    public void SetGameManRef(GameManager gm)
    {
        gameManagerReference = gm;
    }


    public void SpawnNewBody()
    {
        gameManagerReference.TrySpawnNewBody();
    }


}
