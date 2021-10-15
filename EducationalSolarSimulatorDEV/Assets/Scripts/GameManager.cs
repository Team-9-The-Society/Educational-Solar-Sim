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
    public UIBodyInformationPanel BodyInfoPanel;

    private int tapCount = 0;

    private void Awake()
    {
        if (BodyInfoPanel != null)
        {
            BodyInfoPanel.SetGameManRef(this);
            BodyInfoPanel.gameObject.SetActive(false);
        }
    }



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            tapCount++;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.GetComponent<Body>() != null)
                    {
                        BodyInfoPanel.gameObject.SetActive(true);
                        BodyInfoPanel.SetHighlightedBody((hit.collider.gameObject.GetComponent<Body>()));
                        Debug.Log("clicked body");
                        tapCount = 0;
                    }
                }
            }
            else
            {
                Debug.Log("Clicked Nothing");
                if (tapCount > 1)
                {
                    BodyInfoPanel.ClearHighlightedBody();
                    BodyInfoPanel.gameObject.SetActive(false);
                }

            }
        }
    }

    public void DeleteBody(Body b)
    {
        Destroy(b.gameObject);
        BodyInfoPanel.ClearHighlightedBody();
        BodyInfoPanel.gameObject.SetActive(false);
    }


}
