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
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    /*
    [Header("UI Prefab References")]
    public GameObject BodyInfoPanelPrefab;
    public GameObject BodiesPanelPrefab;
    public GameObject UISliderMenuPrefab;
    public GameObject BodyInputPanelPrefab;
    */

    [Header("UI References")]
    public UIBodyInformationPanel BodyInfoPanel;
    public BodiesInfoButton BodiesPanel;
    public UISliderMenu SliderMenu;
    public BodyPromptScript BodyInputPanel;

    [Header("Camera References")]
    public CinemachineFreeLook UniverseCam;
    public CinemachineFreeLook ActivePlanetCam;

    [Header("Body Prefab References")]
    public GameObject emptyBodyPrefab;


    [Header("Management Variables")]
    public List<Body> SimBodies;
    public Body focusedBody;
    public int BodyCount = 0;
    private int tapCount = 0;

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



    public static GameManager Instance { get; set; }


    #region Scene Management + Misc.

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

        TryLocateUIReferences();
    }
    public List<Body> getList()
    {
        return SimBodies;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Running OnSceneLoaded");

        TryLocateUIReferences();

        if (GameObject.FindGameObjectWithTag("UniverseCam") != null)
            UniverseCam = GameObject.FindGameObjectWithTag("UniverseCam").GetComponent<CinemachineFreeLook>();

    }


    public void LoadNewScene(SceneHandler.Scene targetScene)
    {
        ClearUIReferences();
        SimBodies.Clear();
        SceneHandler.Load(targetScene);
    }


    #endregion

    void Update()
    {
        if (UniverseCam != null)
            RefreshUniverseCam();
        //Debug.Log("Pointer over UI: " + IsPointerOverUIElement());

        if (Input.GetMouseButtonDown(0))
        {
            //Increment taps
            tapCount++;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //If a body has been tapped
                if (hit.collider != null)
                {
                    //Attempts to get reference to Body component
                    Body b = hit.collider.gameObject.GetComponent<Body>();
                    if (tapCount >= 2 && focusedBody == b)
                    {
                        ActivatePlanetCam(b.planetCam);
                    }
                    else if (tapCount == 1 && b != null)
                    {
                        focusedBody = b;
                        ShowBodyInfo(b);
                    }
                }
            }
            //Otherwise, if the focused body is not equal to null
            else if (focusedBody != null)
            {
                focusedBody = null;
            }
            else
            {
                tapCount = 0;
                HideBodyInfo();
                if (UniverseCam != null)
                    FocusOnUniverse();
            }
        }

        UpdateForces();

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

            b.transform.position = new Vector3((float)xLoc, (float)yLoc, (float)zLoc);
            r.mass = (float)mass;
            r.velocity = (new Vector3((float)xVel, (float)yVel, (float)zVel));

            focusedBody = bodyRef;
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

            Debug.Log($"Body {i} mass={mass[i]} @ ({position[i,0]},{position[i, 1]},{position[i, 2]})");

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


    #region Camera Functions
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

       // Debug.Log("MaxDist = " + maxDist);

        /*
        CinemachineFreeLook.Orbit t = new CinemachineFreeLook.Orbit(maxDist, 0.1f);
        CinemachineFreeLook.Orbit m = new CinemachineFreeLook.Orbit(0, maxDist);
        CinemachineFreeLook.Orbit l = new CinemachineFreeLook.Orbit(-maxDist, 0.1f);
        */

        UniverseCam.m_Orbits[0] = new CinemachineFreeLook.Orbit(maxDist, 0.1f);
        UniverseCam.m_Orbits[1] = new CinemachineFreeLook.Orbit(0, maxDist);
        UniverseCam.m_Orbits[2] = new CinemachineFreeLook.Orbit(-maxDist, 0.1f);

    }


    #endregion

    #region UI Detection and Manipulation

    private void TryLocateUIReferences()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject g in objs)
        {
            Debug.Log("Scanning " + g.name + " for UIReferences");

            UIRefHandler b = g.GetComponent<UIRefHandler>();

            if (b != null)
            {
                Debug.Log("Found ReferenceHandler");
                SliderMenu = b.SliderRef;
                SliderMenu.ActivateUIElement(this);
                SliderMenu.gameObject.SetActive(true);

                BodyInfoPanel = b.BodiesInfoPanelRef;
                BodyInfoPanel.ActivateUIElement(this);
                BodyInfoPanel.gameObject.SetActive(false);

                BodyInputPanel = b.BodyInputPanelRef;
                BodyInputPanel.ActivateUIElement(this);
                BodyInputPanel.gameObject.SetActive(false);

                BodiesPanel = b.BodiesInfoButtonRef;
                BodiesPanel.ActivateUIElement(this);
                BodiesPanel.gameObject.SetActive(false);

            }
        }
    }

    private void ClearUIReferences()
    {
        BodyInfoPanel = null;
        SliderMenu = null;
        BodyInputPanel = null;
        BodiesPanel = null;
        focusedBody = null;
    }


    //Displays the Body Info Panel for the input Body
    public void ShowBodyInfo(Body b)
    {
        BodyInfoPanel.gameObject.SetActive(true);
        BodyInfoPanel.SetHighlightedBody(b);
    }

    //Hides Body Info Panel
    public void HideBodyInfo()
    {
        if (BodyInfoPanel != null)
        {
            BodyInfoPanel.ClearHighlightedBody();
            focusedBody = null;
            BodyInfoPanel.gameObject.SetActive(false);
        }
    }



    #endregion

    #region Return if Pointer is over a UI Element
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.GetComponent<Canvas>() == null && curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
    #endregion

}
