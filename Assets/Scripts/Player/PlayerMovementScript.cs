using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementScript : MonoBehaviour 
{
    private Vector3 velocity;
    private Vector3 rotation;
    private Rigidbody rb;

    public bool canTheAvatarMove = true;

    public float staminaMaxValue;
    public Image staminaBarImage;
    private float staminaValue;

    public float staminaIncreaseValueAmount;
    public float runStaminaDecreaseValueAmount;

    private PlayerControllerScript playerControllerScript;

    private void Start()
    {
        playerControllerScript = GetComponent<PlayerControllerScript>();

        rb = GetComponent<Rigidbody>();

        staminaBarImage.fillAmount = 1f;
    }

    private void FixedUpdate()
    {
        if (canTheAvatarMove)
        {
            PerformMovement();
            PerformRotation();
        }
    }

    void Update()
    {
        if(playerControllerScript.CheckSpeedMode(PlayerControllerScript.PlayerSpeed.Run))
        {
            DecreaseStaminaBar();
        }

        if (canTheAvatarMove)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                playerControllerScript.UpdateCurrentSpeed(PlayerControllerScript.PlayerSpeed.Run);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) || staminaBarImage.fillAmount == 0)
            {
                playerControllerScript.UpdateCurrentSpeed(PlayerControllerScript.PlayerSpeed.Normal);
            }
        }        

        IncreaseStaminaBar();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    private void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
    }

    public void IncreaseStaminaBar()
    {
        staminaBarImage.fillAmount += staminaIncreaseValueAmount * Time.deltaTime;
    }

    public void DecreaseStaminaBar()
    {
        staminaBarImage.fillAmount -= runStaminaDecreaseValueAmount * Time.deltaTime;
    }
}
