/*  Created by Logan Edmund, 10/14/21
 *  
 *  Handles overarching functions of the program
 * 
 * 
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //Reference to body display panel script

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.GetComponent<Body>() != null)
                    {

                        Debug.Log("clicked body");
                    }
                }
            }
            else
            {
                Debug.Log("Clicked Nothing");
            }
        }
    }




}
