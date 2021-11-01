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
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public UIBodyInformationPanel BodyInfoPanel;
    public UISliderMenu SliderMenu;

    [Header("Camera References")]
    public CinemachineFreeLook UniverseCam;
    public CinemachineFreeLook ActivePlanetCam;

    [Header("Body Prefab References")]
    public GameObject emptyBodyPrefab;


    [Header("Management Variables")]
    public List<Body> SimBodies;
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

        SimBodies = new List<Body>();

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
        UniverseCam = GameObject.FindGameObjectWithTag("UniverseCam").GetComponent<CinemachineFreeLook>();
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
                    Body b = hit.collider.gameObject.GetComponent<Body>();
                    if (b != null)
                    {
                        Debug.Log("clicked body");
                        if (BodyInfoPanel != null)
                        {
                            FocusOnBody(b);
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
                    FocusOnUniverse();
                }

            }
        }
    }

    //Sets a focused body and switches to the camera orbiting it
    public void FocusOnBody(Body b)
    {
        BodyInfoPanel.gameObject.SetActive(true);
        BodyInfoPanel.SetHighlightedBody(b);
        ActivatePlanetCam(b.planetCam);
    }

    //Unfocuses on a selected body, if any, and zooms out to a universe view
    public void FocusOnUniverse()
    {
        BodyInfoPanel.ClearHighlightedBody();
        BodyInfoPanel.gameObject.SetActive(false);
        ActivateUniverseCam();
    }

    //Attemps to spawn a new body at 0,0,0 if the max number of planets has not been reached.
    public void TrySpawnNewBody()
    {
        if (BodyCount < 12)
        {
            GameObject b = Instantiate(emptyBodyPrefab, null, true);
            Body bodyRef = b.GetComponent<Body>();

            ActivatePlanetCam(bodyRef.planetCam);

            BodyInfoPanel.gameObject.SetActive(true);
            BodyInfoPanel.SetHighlightedBody(bodyRef);

            SimBodies.Add(bodyRef);
            BodyCount++;

            b.transform.position.Set(0f, 0f, 0f);
        }
    }

    //Deletes a body and all associated references
    public void DeleteBody(Body b)
    {
        SimBodies.Remove(b);
        BodyCount--;
        ActivateUniverseCam();

        Destroy(b.gameObject);


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

    public void UpdateForces()
    {
        NBody nBody = new NBody(); //The NBody.cs file needs to be in /assets/scripts folder
        int numBodies = SimBodies.Count;
        double[] mass = new double[numBodies];
        double[,] position = new double[numBodies, 3];
        double[,] force;

        int i = 0;
        foreach (Body b in SimBodies)
        {
            mass[i] = b.gameObject.GetComponent<Rigidbody>().mass;
            position[i, 0] = b.gameObject.transform.position.x;
            position[i, 1] = b.gameObject.transform.position.y;
            position[i, 2] = b.gameObject.transform.position.z;
            i++;
        }

        force = nBody.UpdateForce(position, mass, numBodies);

        i = 0;
        foreach (Body b in SimBodies)
        {
            //Apply the output of force[] to each body using ForceMode.Force;
        }

    }

    //Changes the priority to favor a particular planet cam over the universe cam
    public void ActivatePlanetCam(CinemachineFreeLook cam)
    {
        cam.Priority = 5;
        ActivePlanetCam = cam;
        UniverseCam.Priority = 4;
    }

    //Changes the priority to favor the universe over a particular planet
    public void ActivateUniverseCam()
    {
        if (ActivePlanetCam != null)
        {
            ActivePlanetCam.Priority = 4;
            ActivePlanetCam = null;
        }
        UniverseCam.Priority = 5;
    }

}
