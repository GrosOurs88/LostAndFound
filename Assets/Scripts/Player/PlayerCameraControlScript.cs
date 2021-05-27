using UnityEngine;

public class PlayerCameraControlScript : MonoBehaviour 
{
	public float mouseSensitivityX = 5f;
	public float mouseSensitivityY = 5f;

	public float minimumY = -60f;
	public float maximumY = 60f;

    public bool isCursorVisible;

    public GameObject cam;

    private Vector3 cameraRotation;

    private PlayerMovementScript playerMovementScript;

    private void Start()
    {
        playerMovementScript = GetComponent<PlayerMovementScript>();
    }

    void Update()
	{
        if (playerMovementScript.canTheAvatarMove)
        {
            float rotY = Input.GetAxisRaw("Mouse X") * mouseSensitivityX;
            Vector3 rotation = new Vector3(0, rotY, 0);

            playerMovementScript.Rotate(rotation);

            float rotX = Input.GetAxisRaw("Mouse Y") * mouseSensitivityY;
            Vector3 cameraRotation = new Vector3(rotX, 0, 0);

            RotateCamera(cameraRotation);
        }
    }

    private void FixedUpdate()
    {
        PerformCameraRotation();
    }

    private void RotateCamera(Vector3 _camRotation)
    {
        cameraRotation = _camRotation;
    }

    private void PerformCameraRotation()
    {
        cam.transform.Rotate(-cameraRotation);
    }

    public void switchCursorVisible()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void switchCursorNotVisible()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
