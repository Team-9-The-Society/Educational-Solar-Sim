using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class UISliderMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Game Manager Reference")]
    private GameManager gameManagerReference;

    public Animator animator;

    [Header("Panel References")]
    public GameObject PanelMenu;
    public GameObject PanelHideHint;
    public GameObject BodyInfoInputPanel;
    public GameObject BodiesDescriptionPanel;
    public GameObject BodyInfoPanel;
    public GameObject PresetSimulationsPanel;
    public GameObject PauseButton;
    public GameObject FilePanel;
    public GameObject TimePanel;


    public Button LoadButton;
    public Button HomeButton;
    public Button AddButton;
    public Button BodiesButton;
    public Button TemplateButton;
    public Button TimeButton;

    [Header("Management Variables")]
    public bool paused = false;
    public bool isOpen = false;

    private void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
      
        BodyInfoInputPanel.SetActive(false);
        BodiesDescriptionPanel.SetActive(false);
    }

    public void ActivateUIElement(GameManager g)
    {
        SetGameManRef(g.GetComponent<GameManager>());
    }

    //Sets the reference to the game manager
    public void SetGameManRef(GameManager gm)
    {
        gameManagerReference = gm;
    }
   
    public void ChangePanelPrior()
    {
        gameManagerReference.ChangePanelPriority();
    }

    public void MaybeChangePanelPrior()
    {
        //This method only changes panel priority if uipanelpriority is true. Only use for hamburger button.
        if (gameManagerReference.uiPanelPriority)
        {
            gameManagerReference.ChangePanelPriority();
        }
    }
    /// <summary>
    /// //sgwregwegwegwegw
    /// </summary>
    public void ResetScene()
    {
        gameManagerReference.DeleteAllBodies();
    }

    public void ChangeButtonInteractability()
    {
        if (LoadButton != null)
        {
            LoadButton.interactable = isOpen;
        }
        if (HomeButton != null)
        {
            HomeButton.interactable = isOpen;
        }
        if (AddButton != null)
        {
            AddButton.interactable = isOpen;
        }
        if (BodiesButton != null)
        {
            BodiesButton.interactable = isOpen;
        }
        if (TemplateButton != null)
        {
            TemplateButton.interactable = isOpen;
        }
        if (TimeButton != null)
        {
            TimeButton.interactable = isOpen;
        }
    }

    //Triggers the panel sliding in/out of the frame
    public void ShowIdleMenu()
    {
        isOpen = !isOpen;
        ChangeButtonInteractability();
        //Debug.Log(this.Controls[0]);
        //isOpen = animator.GetBool("show");
        animator.SetBool("show", isOpen);
    }



    //Calls the game manager to spawn in a new body
    public void SpawnNewBody()
    {
        gameManagerReference.TrySpawnNewBody();
    }
    //Enables the bodies description panel to appear
    public void ShowBodiesPanel()
    {
        
        BodiesDescriptionPanel.SetActive(true);
    }
    public void ShowContextPanel()
    {
        BodyInfoInputPanel.SetActive(true);
    }
    public void ChangeRotDisplay()
    {
        gameManagerReference.ChangeRotDisplay();
    }

    //Shows the list of preset simulations
    public void ShowPresetSimsPanel()
    {
        if (PresetSimulationsPanel != null)
            PresetSimulationsPanel.SetActive(true);
        else
            Debug.LogError("PresetSimulationsPanel reference on " + name + " is null!");
    }
    public void ShowFilePanel()
    {
        if (FilePanel != null)
        {
            FilePanel.SetActive(true);
        }
    }

    public void ShowTimePanel()
    {
        if (TimePanel != null)
        {
            TimePanel.SetActive(true);
        }
    }

    //Hides the list of preset simulations
    public void HidePresetSimsPanel()
    {
        if (PresetSimulationsPanel != null)
        {
            PresetSimulationsPanel.SetActive(false);
            gameManagerReference.HideHintMessage();
        }
        else
            Debug.LogError("PresetSimulationsPanel reference on " + name + " is null!");
    }



    public void HideContextPanel()
    {
        BodyInfoInputPanel.SetActive(false);
    }

    public void HideAllPanels()
    {
        BodyInfoInputPanel.SetActive(false);
        BodiesDescriptionPanel.SetActive(false);
        BodyInfoPanel.SetActive(false);
        PanelHideHint.SetActive(false);
        FilePanel.SetActive(false);
        TimePanel.SetActive(false);
    }

    public void LoadHomeScene()
    {
        gameManagerReference.SetDefaultTimeScale();
        gameManagerReference.LoadNewScene(SceneHandler.Scene.HomeScene);
    }



    public void NotImplemented()
    {
        Debug.LogWarning("This function has not been implemented yet!");
    }

    public void TogglePause()
    {
        DisplayPause();
        gameManagerReference.TogglePause();
    }

    public void DisplayPause()//async
    {
        if (!paused)
        {
            paused = true;
            //await Task.Delay(TimeSpan.FromSeconds(0.50f));
            UpdatePauseButton("PLAY", Color.red);
        }
        else
        {
            paused = false;
            //await Task.Delay(TimeSpan.FromSeconds(0.25f));
            UpdatePauseButton("PAUSE", Color.white);
        }
    }

    public void UpdatePauseButton(string buttonText, Color color)
    {
        /*ColorBlock cb = PauseButton.GetComponent<Button>().colors;
        Button buttonColor = PauseButton.GetComponent<Button>();

        cb.normalColor = color;
        cb.highlightedColor = color;
        cb.pressedColor = color;
        buttonColor.colors = cb;*/
        PauseButton.GetComponentInChildren<Image>().color = color;

        PauseButton.GetComponentInChildren<Text>().text = buttonText;
        PauseButton.GetComponentInChildren<Text>().color = color;
    }

}
