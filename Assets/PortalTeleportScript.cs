using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleportScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.transform.CompareTag("Player") || collider.transform.CompareTag("Chest"))
        {
            collider.transform.position = transform.parent.GetComponent<PortalScript>().exitPortal.GetComponent<PortalScript>().exitTeleportTransform.position;
           // collider.transform.rotation = transform.parent.GetComponent<PortalScript>().exitPortal.GetComponent<PortalScript>().exitTeleportTransform.rotation;
        }
    }
}
