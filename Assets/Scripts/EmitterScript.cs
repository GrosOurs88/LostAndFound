using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmitterScript : MonoBehaviour
{
    public GameObject receiverToActivate;
    public bool canAChestBePlaced = true;
    public float ObjectPlacedYoffset;
   
    private GameObject objectOnEmitter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Chest") && canAChestBePlaced == true)
        {
            objectOnEmitter = other.gameObject;

            PlaceObjectOnEmitter();              
        }

        else if (other.CompareTag("Player") && canAChestBePlaced == true)
        {
            objectOnEmitter = other.gameObject;

            PlaceAvatarOnEmitter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && canAChestBePlaced == false)
        {
            canAChestBePlaced = true;
            receiverToActivate.GetComponent<ReceiverScript>().numberOfActivatorsOn--;

            if (receiverToActivate.GetComponent<ReceiverScript>().numberOfActivatorsOn == receiverToActivate.GetComponent<ReceiverScript>().activatorsNeeded.Count - 1)
            {
                receiverToActivate.GetComponent<ReceiverScript>().SwitchToClose();
            }
        }
    }

    public void PlaceObjectOnEmitter()
    {
        Vector3 objectPlacedPosition = new Vector3(transform.position.x, transform.position.y + ObjectPlacedYoffset, transform.position.z);
        objectOnEmitter.transform.SetPositionAndRotation(objectPlacedPosition, transform.rotation);

        receiverToActivate.GetComponent<ReceiverScript>().numberOfActivatorsOn++;
        objectOnEmitter.transform.GetComponent<Rigidbody>().isKinematic = true;

        if(objectOnEmitter.GetComponent<ChestScript>())
        {
            objectOnEmitter.transform.GetComponent<ChestScript>().isTaken = false;
            objectOnEmitter.transform.GetComponent<ChestScript>().emitterLinked = gameObject;
        }       

        if (receiverToActivate.GetComponent<ReceiverScript>().numberOfActivatorsOn == receiverToActivate.GetComponent<ReceiverScript>().activatorsNeeded.Count)
        {
            switch (receiverToActivate.GetComponent<ReceiverScript>().type)
            {
                case ReceiverScript.Type.Door:
                    receiverToActivate.GetComponent<ReceiverScript>().SwitchToOpen();
                    break;
                case ReceiverScript.Type.Stairs:
                    Debug.Log("Stairs");
                    break;
                default:
                    Debug.Log("NOTHING");
                    break;
            }
        }

        canAChestBePlaced = false;
    }

    public void PlaceAvatarOnEmitter()
    {
        receiverToActivate.GetComponent<ReceiverScript>().numberOfActivatorsOn++;

        if (receiverToActivate.GetComponent<ReceiverScript>().numberOfActivatorsOn == receiverToActivate.GetComponent<ReceiverScript>().activatorsNeeded.Count)
        {
            switch (receiverToActivate.GetComponent<ReceiverScript>().type)
            {
                case ReceiverScript.Type.Door:
                    receiverToActivate.GetComponent<ReceiverScript>().SwitchToOpen();
                    break;
                case ReceiverScript.Type.Stairs:
                    Debug.Log("Stairs");
                    break;
                default:
                    Debug.Log("NOTHING");
                    break;
            }
        }

        canAChestBePlaced = false;
    }
}


