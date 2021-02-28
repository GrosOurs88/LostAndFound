using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour 
{
	public float speed;
    public float speedRun;
    private bool isAvatarRunning;

    private GameObject avatar;

    public bool canTheAvatarMove;

    public string inputHorizontalAxis;
    public string inputVerticalAxis;

    private float staminaValue;
    public float staminaMaxValue;
    public Image staminaBarImage;

    public float staminaIncreaseValueAmount;
    public float runStaminaDecreaseValueAmount;

    public void Awake()
    {
        avatar = transform.parent.gameObject;

        //transform.parent.transform.position = avatarStartPositionAndRotation.transform.position; //Replace avatar at the specific transform in the editor
        //transform.parent.transform.rotation = avatarStartPositionAndRotation.transform.rotation; //Reoriente avatar at the specific transform in the editor
    }

    private void Start()
    {
        staminaBarImage.fillAmount = 1f;
    }

    private void FixedUpdate()
    {
        if (canTheAvatarMove)
        {
            if (isAvatarRunning)
            {
                staminaBarImage.fillAmount -= runStaminaDecreaseValueAmount * Time.fixedDeltaTime;

                float movX = Input.GetAxis(inputHorizontalAxis) * speedRun;
                float movZ = Input.GetAxis(inputVerticalAxis) * speedRun;

                transform.parent.transform.Translate(movX * Time.fixedDeltaTime, 0, movZ * Time.fixedDeltaTime);
            }
            else
            {
                float movX = Input.GetAxis(inputHorizontalAxis) * speed;
                float movZ = Input.GetAxis(inputVerticalAxis) * speed;

                transform.parent.transform.Translate(movX * Time.fixedDeltaTime, 0, movZ * Time.fixedDeltaTime);
            }
        }
    }

    void Update()
    {
        if (canTheAvatarMove)
        {      
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isAvatarRunning = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isAvatarRunning = false;
            }

            if (staminaBarImage.fillAmount == 0)
            {
                isAvatarRunning = false;
            }
        }

        RaiseStaminaBar();
    }

    public void RaiseStaminaBar()
    {
        staminaBarImage.fillAmount += staminaIncreaseValueAmount * Time.deltaTime;
    }
}
