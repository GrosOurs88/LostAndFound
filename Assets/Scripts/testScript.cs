using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Untagged"))
        {
            transform.GetChild(0).GetComponent<MovementScript>().canTheAvatarMove = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Untagged"))
        {
            transform.GetChild(0).GetComponent<MovementScript>().canTheAvatarMove = true;
        }
    }
}
