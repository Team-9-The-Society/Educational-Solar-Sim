using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodiesInfoButton : MonoBehaviour
{
    private GameManager gameManagerReference;

    // Start is called before the first frame update
   /* private void Awake()
    {
        if (gameManagerReference == null)
        {
            GameObject g = GameObject.FindGameObjectWithTag("GameController");
            if (g != null)
            {
                SetGameManRef(g.GetComponent<GameManager>());
                gameObject.SetActive(false);
            }
        }
    }*/

    /*void Update()
    {
        
    }*/

    public void SetGameManRef(GameManager gm)
    {
        gameManagerReference = gm;
    }


    public void displayBodies()
    {
        List<Body> display = gameManagerReference.SimBodies;

        foreach(Body b in display)
        {

        }
    }

    public void HidePanel()
    {
        this.gameObject.SetActive(false);
    }



}
