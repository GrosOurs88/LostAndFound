using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTriggerScript : MonoBehaviour
{
    public GameObject trap;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            trap.GetComponent<MoveDeathObjectScript>().isFalling = true;
        }
    }
}
