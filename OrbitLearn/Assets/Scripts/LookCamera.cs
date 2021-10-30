using UnityEngine;
using System.Collections;

public class LookCamera : MonoBehaviour 
{
    public float rotSpeedNormal = 10.0f;
    public float rotSpeedFast   = 50.0f;

	public float moveSpeedNormal = 10.0f;
	public float moveSpeedFast = 40.0f;

    public float mouseSensitivityX = 5.0f;
	public float mouseSensitivityY = 5.0f;
    
	float rotY = 0.0f;
    
	void Start()
	{
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}

	void Update()
	{	
        // rotation        
        if (Input.GetMouseButton(1)) 
        {
            float rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivityX;
            rotY += Input.GetAxis("Mouse Y") * mouseSensitivityY;
            rotY = Mathf.Clamp(rotY, -89.5f, 89.5f);
            transform.localEulerAngles = new Vector3(-rotY, rotX, 0.0f);
        }

        float camSpeed = moveSpeedNormal;
        if (Input.GetKey(KeyCode.LeftShift))
            camSpeed = moveSpeedFast;


		if (Input.GetKey(KeyCode.U))
		{
			gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
		}

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = transform.position + (-transform.right * camSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = transform.position + (transform.right * camSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = transform.position + (transform.forward * camSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = transform.position + (-transform.forward * camSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.position = transform.position + (transform.up * camSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.position = transform.position + (-transform.up * camSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.PageUp))
        {
            transform.position = transform.position + (Vector3.up * camSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.PageDown))
        {
            transform.position = transform.position + (-Vector3.up * camSpeed * Time.deltaTime);
        }

    }
}
