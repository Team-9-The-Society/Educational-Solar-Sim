using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;
using System;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{

    [Header("UI References")]
    private TextMeshProUGUI versionNum;
    public UIBodyInformationPanel BodyInfoPanel;
    public BodiesInfoButton BodiesPanel;
    public UISliderMenu SliderMenu;
    public BodyPromptScript BodyInputPanel;
    public UIPresetSimulations PresetSimulations;
    public UIHintDisplay HintDisplay;
    public GameObject PauseIcon;
    public UIFilePanel FilePanel;
    public UITimePanel TimePanel;
    private RotationDisplay RotDisplay;
    private Animator Animator;
    private AudioSource HomeBackgroundSound;

    [Header("Camera References")]
    public GameObject simulationCenter;
    public Vector3 universeCenterByPosition;
    public CinemachineFreeLook UniverseCam;
    public CinemachineFreeLook ActivePlanetCam;

    [Header("Body Prefab References")]
    public GameObject emptyBodyPrefab;


    [Header("Backend Import Variable")]
    public string importString;

    [Header("Management Variables")]
    public List<Body> SimBodies;
    public enum CamState { Universe, Body}
    public CamState CurrCamState = CamState.Universe;
    public Body focusedBody;
    public int BodyCount = 0;
    private int bodyClickCount = 0;
    public int bodyUnivCenter;
    public float currentTimeScale = 1.0f;

    public bool doubleTapReady = false;
    public bool limitDeletion = false;
    private Coroutine doubleTapCheck = null;

    public bool gamePaused = false;
    public bool uiPanelPriority = false;
    public bool bodySelectedUnivCenter = false;
    public float transitionDelayTime = 1.0f;
    public double localBoundary = 1000;
    public double globalBoundary = 100000;
    private float centroidX;
    private float centroidY;
    private float centroidZ;
    public float cinemachineValue;

    private string[] coolFacts;
    private int[] factCollisions;
    private float highLoadBalance = 0;
    private float load = 0;
    HashItUp hashObject;
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
        Animator = GameObject.Find("Transition").GetComponent<Animator>();
        HomeBackgroundSound = GameObject.Find("Sound").GetComponent<AudioSource>();

        SimBodies = new List<Body>();
        LoadFunFacts();
        hashObject = new HashItUp(factCollisions.Length, 31, 0, highLoadBalance, factCollisions);
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
        Debug.Log("Running OnSceneLoaded: " + scene.name);

        if (scene.name == "HomeScene")
        {
            versionNum = GameObject.Find("VersionNumberText").GetComponent<TextMeshProUGUI>();
            SetVersionNumber();
        }

        TryLocateUIReferences();

        if (GameObject.FindGameObjectWithTag("UniverseCam") != null)
            UniverseCam = GameObject.FindGameObjectWithTag("UniverseCam").GetComponent<CinemachineFreeLook>();

    }


    public void LoadNewScene(SceneHandler.Scene targetScene)
    {
        if (gamePaused)
            TogglePause();

        ClearUIReferences();
        SimBodies.Clear();

        StartCoroutine(DelayLoadLevel(targetScene));
    }

    private IEnumerator DelayLoadLevel(SceneHandler.Scene targetScene)
    {
        Animator.SetTrigger("TriggerOutTransition");
        if (targetScene.ToString() == "HomeScene")
            StartCoroutine(AudioFadeScript.FadeIn(HomeBackgroundSound, transitionDelayTime));
        else
            StartCoroutine(AudioFadeScript.FadeOut(HomeBackgroundSound, transitionDelayTime));
        yield return new WaitForSeconds(transitionDelayTime);
        SceneHandler.Load(targetScene);
        Animator.SetTrigger("TriggerInTransition");
    }


    #endregion

    void Update()
    {
        if (UniverseCam != null)
            RefreshUniverseCam();

        if (Input.GetMouseButtonDown(0) && !uiPanelPriority)
        {
            if (CurrCamState == CamState.Universe)
            {
                //Check to see if object is tapped
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    //If a body has been tapped
                    if (hit.collider != null || (hit.collider != null && hit.collider.isTrigger))
                    {
                        //Attempt to get reference to Body component
                        Body b = hit.collider.gameObject.GetComponent<Body>();
                        //If the body component exists, then zoom in and display relevant information.
                        if (b != null)
                        {
                            
                            ActivateBodyCam(b.planetCam);
                            ShowBodyInfo(b);
                        }
                    }
                }

                //If the player taps empty space otherwise, do nothing
                else
                {

                }

            }
            else if (CurrCamState == CamState.Body)
            {
                //Check to see if object is tapped
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    //If a body has been tapped
                    if (hit.collider != null)
                    {
                        //Attempt to get reference to Body component
                        Body b = hit.collider.gameObject.GetComponent<Body>();
                        //If the body component exists...
                        if (b != null)
                        {
                            //If the active body has been tapped, do [thing]
                            if (b.IsEqual(focusedBody))
                            {
                                Debug.Log("KeptHittingDis");
                                if (!BodyInfoPanel.gameObject.activeSelf)
                                {
                                    ShowBodyInfo(b);

                                    SetHintPanel();
                                }
                            }
                            //If a body other than the active planet has been tapped (likely erroneously), do nothing
                            else
                            {
                                //lol
                                HideHintMessage();
                                HideBodyInfo();
                                ActivateUniverseCam();
                                ActivateBodyCam(b.planetCam);
                                ShowBodyInfo(b);
                            }
                        }
                    }
                }

                //If empty space is tapped...
                else
                {
                    StartCoroutine(DelayedHidePanel());
                    //...check for a doubletap. If doubletap, zoom out and clear the screen.
                    if (doubleTapReady)
                    {
                        
                        HideBodyInfo();
                        if (UniverseCam != null)
                        {
                            focusedBody = null;
                            ActivateUniverseCam();
                        }
                        //doubleTapReady = false;
                    }
                    else
                    {
                        if (doubleTapCheck != null)
                            StopCoroutine(doubleTapCheck);
                        doubleTapCheck = StartCoroutine(DoubleTap());
                    }
                }

            }
        }       
    }

    private IEnumerator DoubleTap()
    {
        Debug.Log("New run of Doubletap()");
        doubleTapReady = true;
        yield return new WaitForSecondsRealtime(0.75f);
        doubleTapReady = false;
    }

    public IEnumerator DelayedHidePanel()
    {
        yield return new WaitForSecondsRealtime(0.13f);
        HideBodyInfo();
    }

    //uses a fixed update cycle to keep physics consistent
    void FixedUpdate()
    {
        if (gamePaused == false)
        {
            UpdateForces();
            UpdateRotation();
        }

        List<Body> die = new List<Body>();
        foreach (Body b in SimBodies)
        {
            //Test for bodies outside local and global maximums
            if (isOutOfBounds(new double[] { b.gameObject.transform.position.x, b.gameObject.transform.position.y, b.gameObject.transform.position.z}))//test if body violates bounds
            {
                die.Add(b);//delete body
            }
        }

        foreach(Body b in die)
        {
            DeleteBody(b);
            if (BodiesPanel.gameObject.activeSelf)
            {
                BodiesPanel.OutofBoundsCheck();
            }
        }
    }

    private void SetVersionNumber() => versionNum.text = "Version " + Application.version;

    //Checks if a number is within a range, and if it exceeds the range it will force number to closest number in range
    public double limitRange(double number, double max, double min)
    {
        if (number > max)
        {
            number = max;
        }
        else if (number < min)
        {
            number = min;
        }
        return number;
    }

    //Attemps to spawn a new body at 0,0,0 if the max number of planets has not been reached.
    public void TrySpawnNewBody()
    {
        if (BodyCount < 25)
        {
            GameObject b = Instantiate(emptyBodyPrefab, null, true);
            Body bodyRef = b.GetComponent<Body>();

            ActivateBodyCam(bodyRef.planetCam);

            BodyInfoPanel.gameObject.SetActive(true);
            BodyInfoPanel.SetHighlightedBody(bodyRef);
            bodyRef.bodyNumber = BodyCount;
            SimBodies.Add(bodyRef);
            BodyCount++;
            
            
            b.transform.position.Set(0f, 0f, 0f);
        }
    }

    public void SpawnNewButton(GameObject prefab, GameObject spawnPanel)
    {
        GameObject button = Instantiate(prefab, null, true);
        button.transform.SetParent(spawnPanel.transform);
        button.transform.GetChild(0).GetComponent<TMP_Text>().text = "Testing";
    }

    public void TrySpawnNewBody(double mass, double xLoc, double yLoc, double zLoc, double xVel, double yVel, double zVel, double scal, bool shouldFocus, string name, bool glowState)
    {
        mass = limitRange(mass, Math.Pow(10, 9), Math.Pow(10, -7));
        xVel = limitRange(xVel, 200, -200);
        yVel = limitRange(yVel, 200, -200);
        zVel = limitRange(zVel, 200, -200);
        if(bodySelectedUnivCenter)
        {
            xLoc = limitRange(xLoc, (double)SimBodies[bodyUnivCenter].gameObject.transform.position.x + 50, (double)SimBodies[bodyUnivCenter].gameObject.transform.position.x - 50);
            yLoc = limitRange(yLoc, (double)SimBodies[bodyUnivCenter].gameObject.transform.position.y + 50, (double)SimBodies[bodyUnivCenter].gameObject.transform.position.y - 50);
            zLoc = limitRange(zLoc, (double)SimBodies[bodyUnivCenter].gameObject.transform.position.z + 50, (double)SimBodies[bodyUnivCenter].gameObject.transform.position.z - 50);
        }
        else
        {
            xLoc = limitRange(xLoc, (double)centroidX + 50, (double)centroidX - 50);
            yLoc = limitRange(yLoc, (double)centroidY + 50, (double)centroidY - 50);
            zLoc = limitRange(zLoc, (double)centroidZ + 50, (double)centroidZ - 50);
        }
        if (BodyCount < 25)
        {
            GameObject b = Instantiate(emptyBodyPrefab, null, true);
            Body bodyRef = b.GetComponent<Body>();

            if (shouldFocus)
            {
                ActivateBodyCam(bodyRef.planetCam);
                focusedBody = bodyRef;
                BodyInfoPanel.gameObject.SetActive(true);
                BodyInfoPanel.SetHighlightedBody(bodyRef);
            }

            bodyRef.bodyNumber = BodyCount;
            SimBodies.Add(bodyRef);
            BodyCount++;
            
            Rigidbody r = b.gameObject.GetComponent<Rigidbody>();
            b.transform.position = new Vector3((float)xLoc, (float)yLoc, (float)zLoc);
            b.transform.localScale = new Vector3((float)scal, (float)scal, (float)scal);

            bodyRef.bodyName = name;
            bodyRef.icon.SetName(name);

            float camOrbit = (float)((scal * 8) + 27) / 7;
            bodyRef.planetCam.m_Orbits[0] = new CinemachineFreeLook.Orbit(camOrbit, 0.1f);
            bodyRef.planetCam.m_Orbits[1] = new CinemachineFreeLook.Orbit(0, camOrbit);
            bodyRef.planetCam.m_Orbits[2] = new CinemachineFreeLook.Orbit(-camOrbit, 0.1f);

            bodyRef.planetCam.m_XAxis.m_SpeedMode = (Cinemachine.AxisState.SpeedMode)1;
            bodyRef.planetCam.m_YAxis.m_SpeedMode = (Cinemachine.AxisState.SpeedMode)1;
            bodyRef.planetCam.m_YAxis.m_MaxSpeed = 0.1f;
            bodyRef.planetCam.m_XAxis.m_MaxSpeed = 15f;

            r.mass = (float)mass;
            r.velocity = (new Vector3((float)xVel, (float)yVel, (float)zVel));
            if (glowState)
            {
                bodyRef.flipLight();

            }

        }
    }

    //Flips the panel priority bool
    public void ChangePanelPriority()
    {
        uiPanelPriority = !uiPanelPriority;
    }

    public void ChangeRotDisplay()
    {
        if (RotDisplay.gameObject.activeSelf)
        {
            RotDisplay.gameObject.SetActive(false);
        }
        else
        {
            RotDisplay.gameObject.SetActive(true);
        }
    }

    //Deletes a body and all associated references
    public void DeleteBody(Body b)
    {
        
        int currentcount = b.bodyNumber;
        int iteratecount = currentcount + 1;
        while (iteratecount < BodyCount)
        {
            SimBodies[iteratecount].bodyNumber -= 1;
            iteratecount++;
        }
        if (currentcount < bodyUnivCenter)
        {
            bodyUnivCenter -= 1;
        }
        else if (currentcount == bodyUnivCenter)
        {
            bodySelectedUnivCenter = false;
        }
        SimBodies.Remove(b);
        BodyCount--;
        ActivateUniverseCam();

        Destroy(b.gameObject);

        
        BodyInfoPanel.ClearHighlightedBody();
        BodyInfoPanel.gameObject.SetActive(false);
    }

    public void DisplayExportHint(string msg1, string msg2)
    {
        HintDisplay.gameObject.SetActive(true);
        HintDisplay.SetMessageText(msg1, msg2);
    }

    public void DeleteAllBodies()
    {
        Body[] bodies = new Body[SimBodies.Count];

        int i = 0;
        foreach (Body b in SimBodies)
        {
            bodies[i] = b;
            i++;
        }
        for (int j = 0; j < bodies.Length; j++)
        {
            DeleteBody(bodies[j]);
        }
        SimBodies = new List<Body>();
        bodySelectedUnivCenter = false;
    }

    public void UpdateRotation()
    {
        foreach (Body b in SimBodies)
        {
            b.UpdateRotation();
        }
    }

    private int UpdateForceCallCount = 0;

    public void UpdateForces()
    {
        int numBodies = SimBodies.Count;
        if (numBodies < 2) return;

        UpdateForceCallCount++;
        string debugMsg = "UpdateForces\n";
        NBody nBody = new NBody(); //The NBody.cs file needs to be in /assets/scripts folder
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
            
            debugMsg += $"Body {b.bodyName} mass={mass[i]} @ ({position[i, 0]}, {position[i, 1]}, {position[i, 2]})\n";
            i++;
        }



        force = nBody.UpdateForce(position, mass, numBodies);
        i = 0;
        foreach (Body b in SimBodies)
        {
            b.ApplyForce(force[i, 0], force[i, 1], force[i, 2]);
            debugMsg += $"Body {b.bodyName} ApplyForce={force[i, 0]}, {force[i, 1]}, {force[i, 2]}\n";
            i++;
        }

        if (UpdateForceCallCount % 150 == 0)
            Debug.Log(debugMsg);

        return;
    }

    public void ChangeTimeScaling(float scale)
    {
        if (!gamePaused)
        {
            currentTimeScale = scale;
            Time.timeScale = scale;
        }
    }

    public void SetImportString(string simString)
    {
        GameManager.Instance.DeleteAllBodies();

        

        simString = simString.Split('}')[0];
        simString = simString.Split('{')[1];

        string[] simStringList = simString.Split(';');

        Array.Copy(simStringList, simStringList, simStringList.Length - 1);

        
        for (int i = 0; i < simStringList.Length; i++)
        {
            string[] bodyStringList = simStringList[i].Split('[');

            string name = bodyStringList[0].Substring(0, bodyStringList[0].Length - 1);

            bodyStringList = bodyStringList[1].Split(']')[0].Split(',');

            Dictionary<string, string> attr = new Dictionary<string, string>();

            for ( int j =0; j< bodyStringList.Length-1; j++)
            {
                string str = bodyStringList[j];
                string[] broken = str.Split(':');
                Debug.Log(broken[0]);
                attr.Add(broken[0],broken[1]);
            }


            if (!isOutOfBounds(new double[] { double.Parse(attr["px"]), double.Parse(attr["py"]), double.Parse(attr["pz"]) }))
            {
                TrySpawnNewBody(double.Parse(attr["m"]), double.Parse(attr["px"]), double.Parse(attr["py"]), double.Parse(attr["pz"]), double.Parse(attr["vx"]), double.Parse(attr["vy"]), double.Parse(attr["vz"]), double.Parse(attr["s"]), false, name, bool.Parse(attr["glow"]));
            }

        }


    }

    public void ExportSimulation()
    {

        String simEx = "{";


        foreach (Body b in SimBodies)
        {
            simEx += b.bodyName + ":[";

            simEx += "px:" + b.gameObject.transform.position.x + ",";
            simEx += "py:" + b.gameObject.transform.position.y + ",";
            simEx += "pz:" + b.gameObject.transform.position.z + ",";

            simEx += "vx:" + b.gameObject.GetComponent<Rigidbody>().velocity[0] + ",";
            simEx += "vy:" + b.gameObject.GetComponent<Rigidbody>().velocity[1] + ",";
            simEx += "vz:" + b.gameObject.GetComponent<Rigidbody>().velocity[2] + ",";

            simEx += "m:" + b.gameObject.GetComponent<Rigidbody>().mass + ",";

            simEx += "s:" + b.gameObject.transform.localScale[0] + ",";
            
            simEx += "glow:" + b.radiant.enabled + ",";


            simEx += "];";
        }

        simEx += "}end";

        GUIUtility.systemCopyBuffer = simEx;
    }

    public void SetDefaultTimeScale()
    {
        Time.timeScale = 1.0f;
        currentTimeScale = 1.0f;
    }

    public void TogglePause()
    {
        gamePaused = !gamePaused;
        if (gamePaused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = currentTimeScale;
        }

        if (gamePaused)
        {
            Camera.main.GetComponent<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
        }
        else
        {
            Camera.main.GetComponent<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
        }

    }

    public IEnumerator HintPanelDelay(string msg1, string msg2)
    {
        yield return new WaitForSecondsRealtime(0.17f);
        
        
        if (!uiPanelPriority&&UniverseCam.Priority == 4)
        {
            if (!SliderMenu.isOpen)
            {
                HintDisplay.gameObject.SetActive(true);
                HintDisplay.SetMessageText(msg1, msg2);
            }
        }
    }

    public void DisplayHintMessage(string msg1, string msg2)
    {
        StartCoroutine(HintPanelDelay(msg1, msg2));
    }

    public void HideHintMessage()
    {
        HintDisplay.ClearMessageText();
        HintDisplay.gameObject.SetActive(false);
    }
    private void LoadFunFacts()
    {
        coolFacts = new string[]
        {
            "Our neighbor galaxy is Andromeda.",
            "In 2008 NASA confirmed water on Mars.",
            "Mercury orbits our Sun in appr. 88 days.",
            "Venus' surface can reach 450 Degrees C.",
            "Halley's comet won't pass us again 'til 2061.",
            "Our solar system is 4.57 billion years old.",
            "Footprints left on the Moon won't disappear.",
            "There are 79 known moons orbiting Jupiter.",
            "Earth is the only planet not named after a god.",
            "Pluto is one third water.",
             "One season on Uranus is 21 years on Earth.",
             "The moon Triton orbits Neptune backwards.",
             "Pluto rotates once every 6.4 Earth days.",
             "Saturn orbits the Sun once every 29.4 Earth years.",
             "Only 5 percent of the universe is visible from Earth.",
             "Outer Space is only 62 miles away.",
             "On Venus, it snows metal & rains sulfuric acid.",
             "Saturn has 150 moons and smaller moonlets.",
             "Saturn consists mostly of hydrogen.",
             "Venus is the hottest planet in our solar system.",
             "Astronauts can't burp in space.",
             "Uranus was originally called 'George's Star'.",
             "A sunset on Mars is blue.",
             "The Earth weighs approximately 81 times more than the Moon.",
             "Gennady Padalka has spent 879 days in space.",
             "There is no wind or weather on Mercury.",
             "Jupiter's Red Spot is shrinking.",
             "A day on Mercury is approximately 58 Earth days.",
             "Since space has no gravity, pens will not work.",
             "The center of a comet is called a 'nucleus'.",
             "There are 5 Dwarf Planets in our Solar System.",
             "Jupiter's moon Ganymede is the largest moon in the solar system.",
             "A full NASA space suit costs $12,000,000.",
             "Neutron stars can spin 600 times per second.",
             "A day on Venus lasts 243 Earth days.",
             "There are mountains on Pluto.",
             "The largest known asteroid is 965km wide.",
             "The Moon was once a piece of the Earth.",
             "The universe is around 13.8 billion years old.",
             "Mars and Earth have roughly the same landmass.",
             "Only 18 missions to Mars have been successful.",
             "Pieces of Mars have fallen to Earth.",
             "One day Mars will have a ring.",
             "A year on Neptune lasts 165 Earth years.",
             "There is a volcano on Mars three times the size of Everest.",
             "Neptune is the most distant planet from the Sun.",
             "99 percent of our solar system's mass is the Sun.",
             "Only one spacecraft has flown by Uranus.",
             "Uranus hits the coldest temperatures of any planet.",
             "Jupiter has the shortest day of all the planets.",
             "Eight spacecrafts have visited Jupiter.",
             "A year on Venus lasts 225 Earth days.",
             "The position of the North Star will change over time."
        };

        factCollisions = new int[coolFacts.Length];
    }
   
    public void MakeBodyCenterOfUniv(int bodyNumber)
    {
        if (bodyNumber < 0 || bodyNumber > BodyCount-1)
        {
            bodySelectedUnivCenter = false;
        }
        else
        {
            bodyUnivCenter = bodyNumber;
            bodySelectedUnivCenter = true;
        }
    }

    public string GenerateFunSpaceFact(int address)
    {
        return coolFacts[address];
    }
    #region Camera Functions
    //Changes the priority to favor a particular planet cam over the universe cam
    
    public void SetHintPanel()
    {
        int ran = UnityEngine.Random.Range(0, factCollisions.Length - 1);

        hashObject.ChangeKey(ran);

        if (bodyClickCount > 0)
        {
            if (highLoadBalance > .70)
            {
                hashObject.PublicLoadReset();
            }
            DisplayHintMessage(GenerateFunSpaceFact(hashObject.HashItOut(0)), GenerateFunSpaceFact(hashObject.HashItOut(1)));
            hashObject.ManualIncrementAndLoadBalance(2);
            load = hashObject.getLoad();
            highLoadBalance = hashObject.getLoadBalance();
        }
        else
        {
            DisplayHintMessage("Tap twice outside of the body to unfocus.", "");
        }
        bodyClickCount++;
    }

    public void ActivateBodyCam(CinemachineFreeLook cam)
    {
        cam.Priority = 5;
        ActivePlanetCam = cam;
        UniverseCam.Priority = 4;
        CurrCamState = CamState.Body;
        SetHintPanel();
    }

    //Changes the priority to favor the universe over a particular planet
    public void ActivateUniverseCam()
    {
        BodyInfoPanel.ClearHighlightedBody();
        BodyInfoPanel.gameObject.SetActive(false);

        CurrCamState = CamState.Universe;

        if (ActivePlanetCam != null)
        {
            ActivePlanetCam.Priority = 4;
            ActivePlanetCam = null;
        }
        if (HintDisplay != null)
        {
            if (limitDeletion)
            {
                limitDeletion = false;
            }
            else
            {
                HideHintMessage();
            }
        }
        UniverseCam.Priority = 5;
    }

    //Increase the size of the Universe Cam orbit based on planetary positions
    public void RefreshUniverseCam()
    {
        if (UniverseCam != null && simulationCenter != null)
        {
            //Set position of the UniverseCenter
            if (SimBodies.Count == 0)
            {
                simulationCenter.transform.position = new Vector3(0, 0, 0);
                UniverseCam.m_Orbits[0] = new CinemachineFreeLook.Orbit(30, 0.1f);
                UniverseCam.m_Orbits[1] = new CinemachineFreeLook.Orbit(0, 30);
                UniverseCam.m_Orbits[2] = new CinemachineFreeLook.Orbit(-30, 0.1f);
                centroidX=0;
                centroidY=0;
                centroidZ=0;
                cinemachineValue = 30;
}
            else if (SimBodies.Count == 1)
            {
                simulationCenter.transform.position = SimBodies[0].gameObject.transform.position;
                UniverseCam.m_Orbits[0] = new CinemachineFreeLook.Orbit(30, 0.1f);
                UniverseCam.m_Orbits[1] = new CinemachineFreeLook.Orbit(0, 30);
                UniverseCam.m_Orbits[2] = new CinemachineFreeLook.Orbit(-30, 0.1f);
                centroidX = SimBodies[0].gameObject.transform.position.x;
                centroidY = SimBodies[0].gameObject.transform.position.y;
                centroidZ = SimBodies[0].gameObject.transform.position.z;
                cinemachineValue = 30;
            }
            else if (bodySelectedUnivCenter && BodyCount > bodyUnivCenter)
            {
                float xCenter = 0;
                float yCenter = 0;
                float zCenter = 0;

                foreach (Body b in SimBodies)
                {
                    xCenter += (b.gameObject.transform.position.x);
                    yCenter += (b.gameObject.transform.position.y);
                    zCenter += (b.gameObject.transform.position.z);
                }

                xCenter = xCenter / SimBodies.Count;
                yCenter = yCenter / SimBodies.Count;
                zCenter = zCenter / SimBodies.Count;
                centroidX = xCenter;
                centroidY = yCenter;
                centroidZ = zCenter;
                universeCenterByPosition = new Vector3(xCenter, yCenter, zCenter);

                float maxDist = 25;
                foreach (Body b in SimBodies)
                {
                    float distance = Vector3.Distance(b.gameObject.transform.position, universeCenterByPosition);
                    if (maxDist < distance)
                    {
                        maxDist = distance;
                    }
                }
                maxDist *= 2;


                simulationCenter.transform.position = SimBodies[bodyUnivCenter].gameObject.transform.position;
                UniverseCam.m_Orbits[0] = new CinemachineFreeLook.Orbit(maxDist, 0.1f);
                UniverseCam.m_Orbits[1] = new CinemachineFreeLook.Orbit(0, maxDist);
                UniverseCam.m_Orbits[2] = new CinemachineFreeLook.Orbit(-maxDist, 0.1f);
                cinemachineValue = maxDist;
            }
            else
            {
                bodySelectedUnivCenter = false;
                float xCenter = 0;
                float yCenter = 0;
                float zCenter = 0;

                foreach (Body b in SimBodies)
                {
                    xCenter += (b.gameObject.transform.position.x);
                    yCenter += (b.gameObject.transform.position.y);
                    zCenter += (b.gameObject.transform.position.z);
                }

                xCenter = xCenter / SimBodies.Count;
                yCenter = yCenter / SimBodies.Count;
                zCenter = zCenter / SimBodies.Count;
                centroidX = xCenter;
                centroidY = yCenter;
                centroidZ = zCenter;
                universeCenterByPosition = new Vector3(xCenter, yCenter, zCenter);

                float maxDist = 25;
                foreach (Body b in SimBodies)
                {
                    float distance = Vector3.Distance(b.gameObject.transform.position, universeCenterByPosition);
                    if (maxDist < distance)
                    {
                        maxDist = distance;
                    }
                }
                maxDist *= 2;

                simulationCenter.transform.position = universeCenterByPosition;

                UniverseCam.m_Orbits[0] = new CinemachineFreeLook.Orbit(maxDist, 0.1f);
                UniverseCam.m_Orbits[1] = new CinemachineFreeLook.Orbit(0, maxDist);
                UniverseCam.m_Orbits[2] = new CinemachineFreeLook.Orbit(-maxDist, 0.1f);
                cinemachineValue = maxDist;
            }
        }


    }


    #endregion

    #region UI Detection and Manipulation

    private void TryLocateUIReferences()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject g in objs)
        {
            UIRefHandler b = g.GetComponent<UIRefHandler>();

            if (b != null)
            {
                if (b.SliderRef != null)
                {
                    SliderMenu = b.SliderRef;
                    SliderMenu.ActivateUIElement(this);
                    SliderMenu.gameObject.SetActive(true);
                }

                if (b.BodiesInfoPanelRef != null)
                {
                    BodyInfoPanel = b.BodiesInfoPanelRef;
                    BodyInfoPanel.ActivateUIElement(this);
                    BodyInfoPanel.gameObject.SetActive(false);
                }

                if (b.BodyInputPanelRef != null)
                {
                    BodyInputPanel = b.BodyInputPanelRef;
                    BodyInputPanel.ActivateUIElement(this);
                    BodyInputPanel.gameObject.SetActive(false);
                }

                if (b.BodiesInfoButtonRef != null)
                {
                    BodiesPanel = b.BodiesInfoButtonRef;
                    BodiesPanel.ActivateUIElement(this);
                    BodiesPanel.gameObject.SetActive(false);
                }

                if (b.PresetSimulationsRef != null)
                {
                    PresetSimulations = b.PresetSimulationsRef;
                    PresetSimulations.ActivateUIElement(this);
                    PresetSimulations.gameObject.SetActive(false);
                }

                if (b.HintDisplayRef != null)
                {
                    HintDisplay = b.HintDisplayRef;
                    HintDisplay.gameObject.SetActive(false);
                }

                if (b.PauseDisplayRef != null)
                {
                    PauseIcon = b.PauseDisplayRef;
                    PauseIcon.SetActive(false);
                }

                simulationCenter = b.UniverseCenter;

                if(b.FileRef != null)
                {
                    FilePanel = b.FileRef;
                    FilePanel.ActivateUIElement(this);
                    FilePanel.gameObject.SetActive(false);
                }
                if(b.RotDisplayRef != null)
                {
                    RotDisplay = b.RotDisplayRef;
                    RotDisplay.ActivateUIElement(this);
                    RotDisplay.gameObject.SetActive(false);
                }
                if (b.TimeRef != null)
                {
                    TimePanel = b.TimeRef;
                    TimePanel.ActivateUIElement(this);
                    TimePanel.gameObject.SetActive(false);
                }

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
        PresetSimulations = null;
        simulationCenter = null;
    }

    private IEnumerator MaybeShowIdleMenu()
    {
        yield return new WaitForSecondsRealtime(0.15f);
        
        
        if (SliderMenu.isOpen)
        {
            SliderMenu.ShowIdleMenu();
        }
        
        
    }
    private IEnumerator BodyInfoPanelDisplay()
    {
        yield return new WaitForSecondsRealtime(0.17f);
        
        
        if (!uiPanelPriority&&UniverseCam.Priority == 4)
        {
            if (!SliderMenu.isOpen)
            {
                BodyInfoPanel.gameObject.SetActive(true);
            }
        }
        /*else if (gamePaused && !BodiesPanel.noBodyVerified && uiPanelPriority)
        {
            BodyInfoPanel.gameObject.SetActive(true);
        }*/
    }

    //Displays the Body Info Panel for the input Body
    public void ShowBodyInfo(Body b)
    {
        focusedBody = b;
        //If the slider menu is open, and someone clicks on a body, now the slider menu is moved back to starting position.
        if (SliderMenu.isOpen)
        {
            StartCoroutine(MaybeShowIdleMenu());
        }
        BodyInfoPanel.SetHighlightedBody(b);
        // yield return new WaitForSeconds(0.05f);
        StartCoroutine(BodyInfoPanelDisplay());
        
        //DisplayHintMessage("Quickly tap on the body again to focus.", "Testing");
    }

    //Hides Body Info Panel
    public void HideBodyInfo()
    {
        if (BodyInfoPanel != null)
        {
            BodyInfoPanel.ClearHighlightedBody();
           // focusedBody = null;
            BodyInfoPanel.gameObject.SetActive(false);
        }
        if (HintDisplay != null)
            HideHintMessage();
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

    public bool isOutOfBounds(double[] pos)
    {   
        if(simulationCenter != null)
        {
            double distanceSquared = 0;
            for (int k = 0; k < 3; k++)
            {
                distanceSquared += Math.Pow(pos[k] - simulationCenter.transform.position[k], 2);
            }
            double distance = Math.Sqrt(distanceSquared);
            if(distance > localBoundary)
            {
                if(!uiPanelPriority & !SliderMenu.isOpen) {
                    limitDeletion = true;
                    DisplayExportHint("Body removed from simulation", "Position too far from current simulation center.");
                }
                return true;
            }
        }

        for (int k = 0; k < 3; k++)
        {
            if(Math.Abs(pos[k]) > globalBoundary)
            {
                if (!uiPanelPriority & !SliderMenu.isOpen) {
                    limitDeletion = true;
                    DisplayExportHint("Body removed from simulation", "Position exceeds maximum allowed value.");
                    }
                return true;
            }
        }

        return false; 
    }
}
