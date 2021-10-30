using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoHomeButtonTransition : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PanelMenu;
    public void ShowIdleMenu()
    {
        Animator animator = PanelMenu.GetComponent<Animator>();

        bool isOpen = animator.GetBool("infoShow");
        animator.SetBool("infoShow", !isOpen);
    }
}