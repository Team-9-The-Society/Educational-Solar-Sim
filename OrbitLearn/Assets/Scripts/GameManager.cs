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
    public BodyPromptScript BodyInputPanel;

    [Header("Camera References")]
    public CinemachineFreeLook UniverseCam;
    public CinemachineFreeLook ActivePlanetCam;

    [Header("Body Prefab References")]
    public GameObject emptyBodyPrefab;


    [Header("Management Variables")]
    public List<Body> SimBodies;
    public int BodyCount = 0;

    public static RuntimePlatform platform
    {
        get
        {
#if UNITY_ANDROID
                 return RuntimePlatform.Android;
#elif UNITY_STANDALONE_WIN
                 return RuntimePlatform.WindowsPlayer;
#endif
        }
    }

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

        if (BodyInputPanel != null)
        {
            BodyInputPanel.SetGameManRef(this);
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
        TryLocateBodyInputPanel();
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

        RefreshUniverseCam();
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

    public void TrySpawnNewBody(double mass, double xLoc, double yLoc, double zLoc, double xVel, double yVel, double zVel)
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

            Rigidbody r = b.gameObject.GetComponent<Rigidbody>();

            b.transform.position.Set((float)xLoc, (float)yLoc, (float)zLoc);
            r.mass = (float)mass;
            r.velocity = (new Vector3((float)xVel, (float)yVel, (float)zVel));
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

        /* Can be uncommented once libraries are functional
        try{
            switch(ApplicationUtil.platform)
            {
                case RuntimePlatform.Android:
                    //Android library
                    break;
                case RuntimePlatform.WindowsPlayer:
                    //DLL library
                    break;
                default:
                    force = nBody.UpdateForce(position, mass, numBodies);
                    break;
            }
        }
        //Catch case if the libraries fail
        catch
        {
            force = nBody.UpdateForce(position, mass, numBodies);
        }
        */
        
        //once libraries are added, remove this line
        force = nBody.UpdateForce(position, mass, numBodies);
        i = 0;
        foreach (Body b in SimBodies)
        {
            b.ApplyForce(force[i, 0], force[i, 1], force[i, 2]);
            i++;
        }

        return;
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

    //Increase the size of the Universe Cam orbit based on planetary positions
    public void RefreshUniverseCam()
    {
        float maxDist = 25;
        foreach (Body b in SimBodies)
        {
            float distance = Vector3.Distance(b.gameObject.transform.position, Vector3.zero);
            if (maxDist < distance)
            {
                maxDist = distance;
            }
        }
        maxDist += 5;

        Debug.Log("MaxDist = " + maxDist);

        /*
        CinemachineFreeLook.Orbit t = new CinemachineFreeLook.Orbit(maxDist, 0.1f);
        CinemachineFreeLook.Orbit m = new CinemachineFreeLook.Orbit(0, maxDist);
        CinemachineFreeLook.Orbit l = new CinemachineFreeLook.Orbit(-maxDist, 0.1f);
        */

        UniverseCam.m_Orbits[0] = new CinemachineFreeLook.Orbit(maxDist, 0.1f);
        UniverseCam.m_Orbits[1] = new CinemachineFreeLook.Orbit(0, maxDist);
        UniverseCam.m_Orbits[2] = new CinemachineFreeLook.Orbit(-maxDist, 0.1f);

    }


    #region UI Detection


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

    private void TryLocateBodyInputPanel()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject g in objs)
        {
            Debug.Log("Scanning " + g.name + " for BodyInputPanel");

            if (BodyInfoPanel == null)
            {
                BodyPromptScript b = g.GetComponent<BodyPromptScript>();

                if (b != null)
                {
                    BodyInputPanel = b;
                    BodyInputPanel.SetGameManRef(this);
                    Debug.Log("Found Body Input Panel");
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
        BodyInputPanel = null;
    }

    #endregion

}
