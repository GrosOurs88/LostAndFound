using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTriggerScript : MonoBehaviour
{
    public bool canEscape;
    private int numberOfPlayerInTheBoatEscapeZone = 0;
    public Canvas escapeCanvas;
    public Canvas scoreCanvas;
    public Canvas playerCanvas;
    private bool canBoatMove;
    public GameObject boat;
    public float boatSpeed;
    public GameObject avatar;
    public Camera escapeCamera;

    private void Start()
    {
        escapeCamera.enabled = false;
        escapeCanvas.enabled = false;
        scoreCanvas.enabled = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter) && canEscape == true)
        {
            escapeCamera.enabled = true;
            avatar.transform.GetChild(0).GetComponent<MovementScript>().canTheAvatarMove = false;
            playerCanvas.enabled = false;
            escapeCanvas.enabled = false;
            scoreCanvas.enabled = true;
            canBoatMove = true;
        }        
    }

    private void FixedUpdate()
    {
        if (canBoatMove)
        {
            boat.transform.Translate(Vector3.left * boatSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            numberOfPlayerInTheBoatEscapeZone++;
            escapeCanvas.enabled = true;
            canEscape = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            numberOfPlayerInTheBoatEscapeZone--;

            if(numberOfPlayerInTheBoatEscapeZone == 0)
            {
                escapeCanvas.enabled = false;
                canEscape = false;
            }
            else
            {
                return;
            }            
        }
    }
}
