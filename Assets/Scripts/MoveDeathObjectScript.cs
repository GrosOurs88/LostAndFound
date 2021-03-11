using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDeathObjectScript : MonoBehaviour
{
    public float speed;
    public Vector3 movement;
    public bool isFalling = false;


    private void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.CompareTag("Floor"))
        {
            isFalling = false;
        }    
    }

    void Update()
    {
        if(isFalling)
        {
            transform.position += movement * speed * Time.deltaTime;
        }
    }
}
