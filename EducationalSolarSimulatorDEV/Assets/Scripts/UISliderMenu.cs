using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISliderMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Game Manager Reference")]
    private GameManager gameManagerReference;


    //Sets the reference to the game manager
    public void SetGameManRef(GameManager gm)
    {
        gameManagerReference = gm;
    }


    //Triggers the panel sliding in/out of the frame
    public GameObject PanelMenu;
    public void ShowIdleMenu()
    {
            Animator animator = PanelMenu.GetComponent<Animator>();
           
            bool isOpen = animator.GetBool("show");
            animator.SetBool("show", !isOpen);
    }



    //Calls the game manager to spawn in a new body
    public void SpawnNewBody()
    {
        gameManagerReference.TrySpawnNewBody();
    }


    public void LoadHomeScene()
    {
        gameManagerReference.LoadNewScene(SceneHandler.Scene.HomeScene);
    }

}
