using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour 
{
	public float speed;
    public Transform avatarStartPositionAndRotation;

    public bool canTheAvatarMove;

    public string inputHorizontalAxis;
    public string inputVerticalAxis;

    public void Awake()
    {
        transform.parent.transform.position = avatarStartPositionAndRotation.transform.position; //Replace avatar at the specific transform in the editor
        transform.parent.transform.rotation = avatarStartPositionAndRotation.transform.rotation; //Reoriente avatar at the specific transform in the editor
    }

    void Update()
    {
        if (canTheAvatarMove)
        {
            float movX = Input.GetAxis(inputHorizontalAxis) * speed;
            float movZ = Input.GetAxis(inputVerticalAxis) * speed;

            transform.parent.transform.Translate(movX * Time.deltaTime, 0, movZ * Time.deltaTime);
        }
    }
}
