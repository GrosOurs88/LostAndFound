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

    public void Move(Vector3 _velocity) //Stocke la valeur de velocite de l'avatar
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation) //Stocke la valeur de rotation de l'avatar
    {
        rotation = _rotation;
    }

    private void PerformMovement() //Fais rotater l'avatar s'il a une velocité
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    private void PerformRotation() //Bouge le rigidbody
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
    }

    public void IncreaseStaminaBar() //Augmente la barre de stamina
    {
        staminaBarImage.fillAmount += staminaIncreaseValueAmount * Time.deltaTime;
    }

    public void DecreaseStaminaBar() //Réduit la barre de stamina
    {
        staminaBarImage.fillAmount -= runStaminaDecreaseValueAmount * Time.deltaTime;
    }
}
