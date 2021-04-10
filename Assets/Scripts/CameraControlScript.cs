using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour 
{
    public string inputMouseXAxis;
    public string inputMouseYAxis;

    public float sensitivityX = 5f;
	public float sensitivityY = 5f;
	public float minimumX = -360f;
	public float maximumX = 360f;
	public float minimumY = -60f;
	public float maximumY = 60f;
    private float rotationY = 0f;  

    private void Start()
    {
        switchCursorNotVisible();
    }

    void Update ()
	{
        if (GetComponent<MovementScript>().canTheAvatarMove)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis(inputMouseXAxis) * sensitivityX;

            rotationY += Input.GetAxis(inputMouseYAxis) * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.parent.transform.Rotate(0, Input.GetAxis(inputMouseXAxis) * sensitivityX, 0);
            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }    
    }

    public void switchCursorNotVisible()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
