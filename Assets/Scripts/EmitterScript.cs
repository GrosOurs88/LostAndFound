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

    public GameObject bars;
    
    Vector3 closedPosition;
    Vector3 openPosition;
    public Vector3 closePositionVectorLocal;
    public float closePositionDistance;
    public float closeTime;

    public bool isItADoorEmitter;

    private void Start()
    {
        openPosition = bars.transform.position;
        closedPosition = bars.transform.position + (closePositionVectorLocal * closePositionDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Chest") && canAChestBePlaced == true)
        {
            if(other.GetComponent<ChestScript>().isTaken == false)
            {
                objectOnEmitter = other.gameObject;

                PlaceObjectOnEmitter();

                if (isItADoorEmitter)
                {
                    StartCoroutine(Close(closeTime));
                }
            }            
        }

        //if (other.CompareTag("Player") && canAChestBePlaced == true)
        //{
        //    objectOnEmitter = other.gameObject;

        //    PlaceAvatarOnEmitter();
        //}
    }

    public IEnumerator Close(float time)
    {
        Vector3 startingPos = bars.transform.position;
        Vector3 finalPos = closedPosition;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            bars.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player") && canAChestBePlaced == false)
    //    {
    //        canAChestBePlaced = true;
    //        receiverToActivate.GetComponent<ReceiverScript>().numberOfEmittersOn--;
    //        receiverToActivate.GetComponent<ReceiverScript>().UpdateDoorLevel();

    //        if (receiverToActivate.GetComponent<ReceiverScript>().numberOfEmittersOn == receiverToActivate.GetComponent<ReceiverScript>().numberOfEmittersNeeded - 1
    //            && receiverToActivate.GetComponent<ReceiverScript>().isItLockedWhenActivated == false)
    //        {
    //            switch (receiverToActivate.GetComponent<ReceiverScript>().type)
    //            {
    //                case ReceiverScript.Type.Door:
    //                   // receiverToActivate.GetComponent<ReceiverScript>().SwitchToClose();
    //                    break;
    //                case ReceiverScript.Type.Stairs:
    //                    break;
    //                default:
    //                    Debug.Log("NOTHING");
    //                    break;
    //            }
    //        }
    //    }
    //}

    public void PlaceObjectOnEmitter()
    {
        Vector3 objectPlacedPosition = new Vector3(transform.position.x, transform.position.y + ObjectPlacedYoffset, transform.position.z);
        objectOnEmitter.transform.SetPositionAndRotation(objectPlacedPosition, transform.rotation);

        receiverToActivate.GetComponent<ReceiverScript>().numberOfEmittersOn++;
        receiverToActivate.GetComponent<ReceiverScript>().UpdateDoorLevel();

        objectOnEmitter.transform.GetComponent<Rigidbody>().isKinematic = true;

        objectOnEmitter.GetComponent<ChestScript>().canBeTaken = false;

        if(objectOnEmitter.GetComponent<ChestScript>())
        {
            objectOnEmitter.transform.GetComponent<ChestScript>().isTaken = false;
            objectOnEmitter.transform.GetComponent<ChestScript>().emitterLinked = gameObject;
        }       

        if (receiverToActivate.GetComponent<ReceiverScript>().numberOfEmittersOn == receiverToActivate.GetComponent<ReceiverScript>().numberOfEmittersNeeded)
        {
            switch (receiverToActivate.GetComponent<ReceiverScript>().type)
            {
                case ReceiverScript.Type.Door:
                    receiverToActivate.GetComponent<ReceiverScript>().SwitchToOpen();
                    break;
                case ReceiverScript.Type.Stairs:                    
                    break;
                default:
                    Debug.Log("NOTHING");
                    break;
            }
        }

        canAChestBePlaced = false;
    }

    //public void PlaceAvatarOnEmitter()
    //{
    //    receiverToActivate.GetComponent<ReceiverScript>().numberOfEmittersOn++;
    //    receiverToActivate.GetComponent<ReceiverScript>().UpdateDoorLevel();

    //    if (receiverToActivate.GetComponent<ReceiverScript>().numberOfEmittersOn == receiverToActivate.GetComponent<ReceiverScript>().numberOfEmittersNeeded)
    //    {
    //        switch (receiverToActivate.GetComponent<ReceiverScript>().type)
    //        {
    //            case ReceiverScript.Type.Door:
    //                receiverToActivate.GetComponent<ReceiverScript>().SwitchToOpen();
    //                break;
    //            case ReceiverScript.Type.Stairs:
    //                Debug.Log("Stairs");
    //                break;
    //            default:
    //                Debug.Log("NOTHING");
    //                break;
    //        }
    //    }

    //    canAChestBePlaced = false;
    //}
}


