using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMenuAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PanelMenu;
    public void ShowIdleMenu()
    {
        if(PanelMenu!= null)
        {
            Animator animator = PanelMenu.GetComponent<Animator>();
            if (animator!=null)
            {
                bool isOpen = animator.GetBool("showAnimation");
                animator.SetBool("showAnimation", !isOpen);
            }
        }
    }
}
