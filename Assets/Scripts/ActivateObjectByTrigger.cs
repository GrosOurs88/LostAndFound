using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObjectByTrigger : MonoBehaviour
{
    public GameObject objectToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectToActivate.gameObject.SetActive(true);

            gameObject.SetActive(false);
        }
    }
}
