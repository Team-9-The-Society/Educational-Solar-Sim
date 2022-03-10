using UnityEngine;
using TMPro;

public class BodyIcon : MonoBehaviour
{
    private Transform cam;

    public SphereCollider planetCollider;
    public SpriteRenderer rend;
    public GameObject nameDisplay;
    public float turnOffDistance;

    private void Start()
    {
        cam = Camera.main.transform;
        GetComponentInParent<Canvas>().worldCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (cam == null)
            cam = Camera.main.transform;
        else
            transform.LookAt(transform.position + cam.forward);

        if (Vector3.Distance(cam.transform.position, this.transform.position) < turnOffDistance)
        {
            rend.enabled = false;
            nameDisplay.SetActive(false);
            planetCollider.radius = 0.55f;
        }
        else
        {
            rend.enabled = true;
            nameDisplay.SetActive(true);
            planetCollider.radius = 2.55f;
        }


    }

    public void SetName(string s)
    {
        nameDisplay.GetComponent<TMP_Text>().text = s;
    }


}
