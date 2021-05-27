using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Untagged"))
        {
            transform.GetComponent<PlayerMovementScript>().canTheAvatarMove = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Untagged"))
        {
            transform.GetComponent<PlayerMovementScript>().canTheAvatarMove = true;
        }
    }
}
