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
using UnityEngine.SceneManagement;

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
        else
        {
            TryLocateBodyInfoPanel();
        }

        if (SliderMenu != null)
        {
            SliderMenu.SetGameManRef(this);
            SliderMenu.gameObject.SetActive(true);
        }
        else
        {
            TryLocateSliderPanel();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Running OnSceneLoaded");
        TryLocateBodyInfoPanel();
        TryLocateSliderPanel();
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
                        Debug.Log("clicked body");
                        if (BodyInfoPanel != null)
                        {
                            BodyInfoPanel.gameObject.SetActive(true);
                            BodyInfoPanel.SetHighlightedBody((hit.collider.gameObject.GetComponent<Body>()));
                        }

                        tapCount = 0;
                    }
                }
            }
            else
            {
                Debug.Log("Clicked Nothing");
                if (tapCount > 1 && BodyInfoPanel != null)
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
        ClearUIReferences();
        SceneHandler.Load(targetScene); 
    }

    private void TryLocateBodyInfoPanel()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject g in objs)
        {
            Debug.Log("Scanning " + g.name + " for BodyInfoPanel");

            if (BodyInfoPanel == null)
            {
                UIBodyInformationPanel b = g.GetComponent<UIBodyInformationPanel>();

                if (b != null)
                {
                    BodyInfoPanel = b;
                    BodyInfoPanel.SetGameManRef(this);
                    Debug.Log("Found Body Info Panel");
                }
            }
        }
    }

    private void TryLocateSliderPanel()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject g in objs)
        {
            Debug.Log("Scanning " + g.name + " for SliderPanel");

            if (SliderMenu == null)
            {
                UISliderMenu b = g.GetComponent<UISliderMenu>();
                if (b != null)
                {
                    SliderMenu = b;
                    SliderMenu.SetGameManRef(this);
                    Debug.Log("Found Slider Menu");
                }
            }

            
        }
    }

    private void ClearUIReferences()
    {
        BodyInfoPanel = null;
        SliderMenu = null;
    }

}
