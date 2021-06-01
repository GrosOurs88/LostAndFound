using UnityEngine;

public class PlayerCameraControlScript : MonoBehaviour 
{
    [SerializeField]
    private float mouseSensitivityX = 5f;
    [SerializeField]
    private float mouseSensitivityY = 5f;

    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;

    [SerializeField]
	private float cameraRotationLimit = 85f;

    [SerializeField]
    private bool isCursorVisible;

    public GameObject cam;

    private PlayerMovementScript playerMovementScript;

    private void Start()
    {
        playerMovementScript = GetComponent<PlayerMovementScript>();
    }

    void Update()
	{
        if (playerMovementScript.canTheAvatarMove)
        {
            //Record mouse movements on X axis
            float rotY = Input.GetAxisRaw("Mouse X");
            Vector3 rotation = new Vector3(0, rotY, 0) * mouseSensitivityX;
            playerMovementScript.Rotate(rotation);

            //Record mouse movements on Y axis
            float rotX = Input.GetAxisRaw("Mouse Y");
            float cameraRotationX = rotX * mouseSensitivityY;
            RotateCamera(cameraRotationX);
        }
    }

    private void FixedUpdate()
    {
        PerformCameraRotation();
    }

    private void RotateCamera(float _camRotationX) //Stocke la valeur de rotation de l'avatar sur laxe Y
    {
        cameraRotationX = _camRotationX;
    }

    private void PerformCameraRotation() //Calcule et applique la rotation de la camera
    {
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    public void switchCursorVisible() //Rend le curseur visible
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void switchCursorNotVisible() //Rend le curseur invisible
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
