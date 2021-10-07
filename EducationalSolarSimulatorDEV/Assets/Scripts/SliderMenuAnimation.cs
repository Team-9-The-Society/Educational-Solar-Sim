using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMenuAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PanelMenu;
    public void ShowIdleMenu()
    {
            Animator animator = PanelMenu.GetComponent<Animator>();
           
            bool isOpen = animator.GetBool("show");
            animator.SetBool("show", !isOpen);
    }
}
