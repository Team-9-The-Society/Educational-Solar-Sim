using UnityEngine;
using TMPro;

public class BodyIcon : MonoBehaviour
{
    private Transform cam;
    private float cameraDistance;
    private RectTransform canvasTransform;
    private Transform planetIconTransform;
    private Vector3 tmpLocalScale = new Vector3();

    public SphereCollider planetCollider;
    public SpriteRenderer rend;
    public GameObject nameDisplay;
    public float turnOffDistance;

    const float sizeModifier = 75;

    private void Start()
    {
        cam = Camera.main.transform;
        GetComponentInParent<Canvas>().worldCamera = Camera.main;
        canvasTransform = GetComponentInParent<RectTransform>();
        planetIconTransform = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        if (cam == null)
            cam = Camera.main.transform;
        else
            transform.LookAt(transform.position + cam.forward);

        cameraDistance = Vector3.Distance(cam.transform.position, planetCollider.transform.position);

        if (cameraDistance < turnOffDistance)
        {
            rend.enabled = false;
            nameDisplay.SetActive(false);
            planetCollider.radius = 0.55f;
        }
        else
        {
            tmpLocalScale.x = cameraDistance / sizeModifier;
            tmpLocalScale.y = tmpLocalScale.x;
            tmpLocalScale.z = 1;

            // Apply new scale
            planetIconTransform.localScale = tmpLocalScale;
            Debug.Log($"tmpLocalScale {tmpLocalScale.x}\nplanetIconTransform.localScale {canvasTransform.localScale}");

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
