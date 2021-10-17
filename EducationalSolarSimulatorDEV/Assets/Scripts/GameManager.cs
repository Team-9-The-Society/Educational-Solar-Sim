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
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public UIBodyInformationPanel BodyInfoPanel;
    public UISliderMenu SliderMenu;

    [Header("Body Prefab References")]
    public GameObject emptyBodyPrefab;


    [Header("Management Variables")]
    public int BodyCount = 0;



    private int tapCount = 0;


    public static GameManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else if (Instance == null && Instance != this)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);


        if (BodyInfoPanel != null)
        {
            BodyInfoPanel.SetGameManRef(this);
            BodyInfoPanel.gameObject.SetActive(false);
        }

        if (SliderMenu != null)
        {
            SliderMenu.SetGameManRef(this);
            SliderMenu.gameObject.SetActive(true);
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

    public void TrySpawnNewBody()
    {
        if (BodyCount < 12)
        {
            GameObject b = Instantiate(emptyBodyPrefab, null, true);
            BodyCount++;

            b.transform.position.Set(0f, 0f, 0f);
        }
    }

    public void DeleteBody(Body b)
    {
        Destroy(b.gameObject);
        BodyCount--;
        BodyInfoPanel.ClearHighlightedBody();
        BodyInfoPanel.gameObject.SetActive(false);
    }

    public void LoadNewScene(SceneHandler.Scene targetScene)
    {
        SceneHandler.Load(targetScene); 
    }

}
