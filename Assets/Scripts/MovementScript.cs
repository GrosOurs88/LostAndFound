using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour 
{
	public float speed;
    public float speedRun;
    [HideInInspector]
    public  bool isAvatarRunning;

    private GameObject avatar;

    public bool canTheAvatarMove = true;
    public bool canTheAvatarRun = true;

    public string inputHorizontalAxis;
    public string inputVerticalAxis;
    public string inputRun;

    private float staminaValue;
    public float staminaMaxValue;
    public Image staminaBarImage;

    public float staminaIncreaseValueAmount;
    public float runStaminaDecreaseValueAmount;

    //***TEST***
    [HideInInspector]
    public bool isAvatarOnFire = false;
    private float staminaLossMultiplierWhenOnFire = 5f;
    private float speedOnFire = 15f;
    //***TEST***

    public void Awake()
    {
        avatar = transform.parent.gameObject;
    }

    private void Start()
    {
        staminaBarImage.fillAmount = 1f;
    }

    private void FixedUpdate()
    {
        if (canTheAvatarMove)
        {
            //***TEST
            if (isAvatarOnFire)
            {
                isAvatarRunning = true;

                staminaBarImage.fillAmount -= runStaminaDecreaseValueAmount * staminaLossMultiplierWhenOnFire * Time.fixedDeltaTime;

                float movX = Input.GetAxis(inputHorizontalAxis) * speedOnFire;
                float movZ = speedOnFire;

                transform.parent.transform.Translate(movX * Time.fixedDeltaTime, 0, movZ * Time.fixedDeltaTime);
            }

            //***TEST

            else if (isAvatarRunning) //Remove the else
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
        if (canTheAvatarMove && canTheAvatarRun)
        {
            // if (Input.GetKeyDown(KeyCode.LeftShift))
            if (Input.GetAxis(inputRun) > 0)               
            {
                isAvatarRunning = true;
            }
            // if (Input.GetKeyUp(KeyCode.LeftShift))
            if (Input.GetAxis(inputRun) == 0)
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
