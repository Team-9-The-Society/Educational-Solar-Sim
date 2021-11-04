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

    private void Awake()
    {
        if (gameManagerReference == null)
        {
            GameObject g = GameObject.FindGameObjectWithTag("GameController");
            if (g != null)
                SetGameManRef(g.GetComponent<GameManager>());
        }

        animator = this.gameObject.GetComponent<Animator>();
      
        BodyInfoInputPanel.SetActive(false);
        BodiesDescriptionPanel.SetActive(false);

    }


    //Sets the reference to the game manager
    public void SetGameManRef(GameManager gm)
    {
        gameManagerReference = gm;
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

    public void LoadHomeScene()
    {
        gameManagerReference.LoadNewScene(SceneHandler.Scene.HomeScene);
    }



    public void NotImplemented()
    {
        Debug.LogWarning("This function has not been implemented yet!");
    }


}
