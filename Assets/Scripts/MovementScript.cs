using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour 
{
	public float speed;
    public float speedRun;
    private bool isAvatarRunning;

    public Transform avatarStartPositionAndRotation;

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
        transform.parent.transform.position = avatarStartPositionAndRotation.transform.position; //Replace avatar at the specific transform in the editor
        transform.parent.transform.rotation = avatarStartPositionAndRotation.transform.rotation; //Reoriente avatar at the specific transform in the editor
    }

    private void Start()
    {
        staminaBarImage.fillAmount = 1f;
    }

    void Update()
    {
        RaiseStaminaBar();

        if (canTheAvatarMove)
        {
            if(isAvatarRunning)
            {
                staminaBarImage.fillAmount -= runStaminaDecreaseValueAmount * Time.deltaTime;

                float movX = Input.GetAxis(inputHorizontalAxis) * speedRun;
                float movZ = Input.GetAxis(inputVerticalAxis) * speedRun;

                transform.parent.transform.Translate(movX * Time.deltaTime, 0, movZ * Time.deltaTime);
            }
            else
            {
                float movX = Input.GetAxis(inputHorizontalAxis) * speed;
                float movZ = Input.GetAxis(inputVerticalAxis) * speed;

                transform.parent.transform.Translate(movX * Time.deltaTime, 0, movZ * Time.deltaTime);
            }

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
    }

    public void RaiseStaminaBar()
    {
        staminaBarImage.fillAmount += staminaIncreaseValueAmount * Time.deltaTime;
    }
}
