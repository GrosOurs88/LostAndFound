using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    public GameObject door;
    public Image doorGaugeImage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Chest") && other.transform.parent.GetComponent<ChestScript>().isTaken == false)
        {
            other.transform.parent.parent = transform;
            other.transform.parent.SetPositionAndRotation(transform.position, transform.rotation);

            door.GetComponent<DoorManagerScript>().numberOfChestPlaced++;
            door.GetComponent<DoorManagerScript>().doorCompletionLevel = (float)door.GetComponent<DoorManagerScript>().numberOfChestPlaced / (float)door.GetComponent<DoorManagerScript>().numberOfChestToOpenDoor;
            doorGaugeImage.fillAmount = door.GetComponent<DoorManagerScript>().doorCompletionLevel;

            other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
            foreach (Transform trans in other.transform.parent)
            {
                if (trans.GetComponent<Collider>())
                {
                    trans.GetComponent<Collider>().enabled = false;
                }
            }

            other.transform.parent.GetComponent<ChestScript>().isTaken = false;

            if (door.GetComponent<DoorManagerScript>().doorCompletionLevel == 1)
            {
                StartCoroutine(OpenDoor(3f));
            }

            transform.GetComponent<Collider>().enabled = false;
        }
    }    

    public IEnumerator OpenDoor(float time)
    {
        Vector3 startingPos = door.transform.position;
        Vector3 finalPos = door.transform.position + (transform.up * 9);
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            door.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }    
}


