using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISliderMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Game Manager Reference")]
    private GameManager gameManagerReference;

    public Animator animator;

    [Header("Panel References")]
    public GameObject PanelMenu;
    public GameObject BodyInfoInputPanel;
    public GameObject BodiesDescriptionPanel;
    public GameObject BodyInfoPane;

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
   


    public void ResetScene()
    {
        gameManagerReference.LoadNewScene(SceneHandler.Scene.SimulationScene);
    }


    //Triggers the panel sliding in/out of the frame
    public void ShowIdleMenu()
    {
            bool isOpen = animator.GetBool("show");
            animator.SetBool("show", !isOpen);
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



    public void HideContextPanel()
    {
        BodyInfoInputPanel.SetActive(false);
    }

    public void HideAllPanels()
    {
        BodyInfoInputPanel.SetActive(false);
        BodiesDescriptionPanel.SetActive(false);
        BodyInfoPane.SetActive(false);
    }

    public void LoadHomeScene()
    {
        gameManagerReference.LoadNewScene(SceneHandler.Scene.HomeScene);
    }



    public void NotImplemented()
    {
        Debug.LogWarning("This function has not been implemented yet!");
    }


}
