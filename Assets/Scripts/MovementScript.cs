using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour 
{
    [Header("Inputs")]
    public string inputHorizontalAxis;
    public string inputVerticalAxis;
    public string inputRun;

    [Header("Movement")]
    public float speed;
    public float speedRun;
    public bool isAvatarRunning = false;

    [Header("Movements Constraints")]
    public bool canTheAvatarMove = true;
    public bool canTheAvatarRun = true;

    [Header("Stamina")]
    public float staminaMaxValue;
    public float staminaIncreaseValueAmount;
    public float runStaminaDecreaseValueAmount;
    public Image staminaBarImage;

    [Header("OnFire Mode")]
    public bool isAvatarOnFire = false;
    public float staminaLossMultiplierWhenOnFire = 5f;
    public float speedOnFire = 15f;

    // Check input to run
    private bool thePlayerCanUseTheTrigger = true;

    private void Start()
    {
        staminaBarImage.fillAmount = 1f;
    }

    private void FixedUpdate()
    {
        if (canTheAvatarMove)
        {
            if (isAvatarOnFire)
            {
                isAvatarRunning = true;

                staminaBarImage.fillAmount -= runStaminaDecreaseValueAmount * staminaLossMultiplierWhenOnFire * Time.fixedDeltaTime;

                float movX = Input.GetAxis(inputHorizontalAxis) * speedOnFire;
                float movZ = speedOnFire;

                transform.parent.transform.Translate(movX * Time.fixedDeltaTime, 0, movZ * Time.fixedDeltaTime);
            }
            else if (isAvatarRunning)
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
            if (Input.GetAxisRaw(inputRun) == 1)
            {
                if(thePlayerCanUseTheTrigger == true)
                {
                    isAvatarRunning = true;
                }
            }
            else if (Input.GetAxisRaw(inputRun) == 0)
            {
                isAvatarRunning = false;
                thePlayerCanUseTheTrigger = true;
            }

            if (staminaBarImage.fillAmount == 0)
            {
                isAvatarRunning = false;
                thePlayerCanUseTheTrigger = false;
            }
        }
        RaiseStaminaBar();
    }

    public void RaiseStaminaBar()
    {
        staminaBarImage.fillAmount += staminaIncreaseValueAmount * Time.deltaTime;
    }
}
