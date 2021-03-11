using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class SearchObjectScript : MonoBehaviour
{
    public float raycastLength;

    public GameObject hole;
    public GameObject holeWin;

    public GameObject chest;

    public Transform avatarHandMap;
    public Transform avatarHandChest;

    public float digStaminaDecreaseValueAmount;

    private bool isTakingSomething = false;
    private bool takenObjectIsAMap;
    private bool takenObjectIsAChest;
    private GameObject takenObject;

    public float launchObjectForce;
    public float minLaunchObjectForce = 200f;
    public float maxLaunchObjectForce = 600f;
    public float launchObjectIncreaseValueAmount;
    public GameObject PanelLaunchObjectForce;
    public Image launchObjectBar;

    MovementScript movementScript;

    public float holePlacementYOffset;
    public float holeCrossPlacementYOffset;

    private void Start()
    {
        movementScript = gameObject.GetComponent<MovementScript>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GetComponent<MovementScript>().canTheAvatarMove)
        {
            RaycastHit hit;
            LayerMask layerDefault = LayerMask.GetMask("Default");
            LayerMask layerCross = LayerMask.GetMask("Cross");
            LayerMask layerFloor = LayerMask.GetMask("Floor");
            LayerMask layerMap = LayerMask.GetMask("Map");
            LayerMask layerChest = LayerMask.GetMask("Chest");

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength, layerDefault))
            {
                return;
            }

            else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength, layerMap) && isTakingSomething == false)
            {
                hit.collider.transform.parent = avatarHandMap;
                hit.collider.transform.SetPositionAndRotation(avatarHandMap.position, avatarHandMap.rotation);
                hit.collider.GetComponent<Rigidbody>().isKinematic = true;
                hit.collider.GetComponent<BoxCollider>().enabled = false;
                isTakingSomething = true;
                takenObjectIsAMap = true;
                takenObject = hit.collider.gameObject;
            }

            else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength, layerChest) && isTakingSomething == false)
            {
                hit.collider.transform.parent = avatarHandChest;
                hit.collider.transform.SetPositionAndRotation(avatarHandChest.position, avatarHandChest.rotation);
                hit.collider.GetComponent<Rigidbody>().isKinematic = true;
                hit.collider.GetComponent<Collider>().enabled = false;

                isTakingSomething = true;
                takenObjectIsAChest = true;
                takenObject = hit.collider.gameObject;
                hit.collider.GetComponent<ChestScript>().isTaken = true;

                if(hit.collider.GetComponent<ChestScript>().emitterLinked != null)
                {
                    hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().canAChestBePlaced = true;
                    hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().receiverToActivate.GetComponent<ReceiverScript>().numberOfActivatorsOn--;

                    if(hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().receiverToActivate.GetComponent<ReceiverScript>().numberOfActivatorsOn == hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().receiverToActivate.GetComponent<ReceiverScript>().activatorsNeeded.Count-1 
                        && hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().receiverToActivate.GetComponent<ReceiverScript>().isItLockedWhenActivated == false)
                    {                     
                        hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().receiverToActivate.GetComponent<ReceiverScript>().SwitchToClose();
                    }
                }                
            }

            else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength, layerCross))
            {
                if (movementScript.staminaBarImage.fillAmount >= digStaminaDecreaseValueAmount)
                {
                    movementScript.staminaBarImage.fillAmount -= digStaminaDecreaseValueAmount;

                    GameObject newHole = Instantiate(holeWin, hit.point, Quaternion.Euler(90, 0, 0));
                    newHole.transform.position = new Vector3(newHole.transform.position.x, hit.point.y + holeCrossPlacementYOffset, newHole.transform.position.z);

                    Destroy(hit.collider.gameObject); //Destroy cross

                    GameObject newChest = Instantiate(chest, hit.point, Quaternion.Euler(0, 0, 0));
                    newChest.GetComponent<Rigidbody>().isKinematic = true;
                    newChest.transform.position = new Vector3(newChest.transform.position.x, hit.point.y, newChest.transform.position.z);
                }
            }

            else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength, layerFloor))
            {
                if (movementScript.staminaBarImage.fillAmount >= digStaminaDecreaseValueAmount)
                {
                    movementScript.staminaBarImage.fillAmount -= digStaminaDecreaseValueAmount;

                    GameObject newHole = Instantiate(hole, hit.point, Quaternion.Euler(90, 0, 0));
                    newHole.transform.position = new Vector3(newHole.transform.position.x, hit.point.y + holePlacementYOffset, newHole.transform.position.z);
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && isTakingSomething && GetComponent<MovementScript>().canTheAvatarMove)
        {
            PanelLaunchObjectForce.gameObject.SetActive(true);
            launchObjectBar.fillAmount = 0f;
            launchObjectForce = minLaunchObjectForce;
        }

        if (Input.GetMouseButton(1) && isTakingSomething && GetComponent<MovementScript>().canTheAvatarMove)
        {
            if (launchObjectForce < maxLaunchObjectForce)
            {
                launchObjectForce += launchObjectIncreaseValueAmount * Time.deltaTime;
            }
            if (launchObjectForce > maxLaunchObjectForce)
            {
                launchObjectForce = maxLaunchObjectForce;
            }

            launchObjectBar.fillAmount = (launchObjectForce / maxLaunchObjectForce);
        }

        if (Input.GetMouseButtonUp(1) && isTakingSomething && GetComponent<MovementScript>().canTheAvatarMove)
        {
            LaunchTakenObject();
        }
    }

    public void LaunchTakenObject()
    {
        if (takenObjectIsAMap == true)
        {
            takenObject.transform.parent = null;
            takenObject.GetComponent<Rigidbody>().isKinematic = false;
            takenObject.GetComponent<BoxCollider>().enabled = true;
            takenObject.GetComponent<Rigidbody>().AddForce(transform.forward * launchObjectForce);
            takenObjectIsAMap = false;
            isTakingSomething = false;
        }

        else if (takenObjectIsAChest == true)
        {
            takenObject.transform.parent = null;
            takenObject.GetComponent<Rigidbody>().isKinematic = false;
            takenObject.GetComponent<Collider>().enabled = true;
            takenObject.GetComponent<Rigidbody>().AddForce(transform.forward * launchObjectForce);
            takenObjectIsAChest = false;
            isTakingSomething = false;
            takenObject.GetComponent<ChestScript>().isTaken = false;
        }

        PanelLaunchObjectForce.gameObject.SetActive(false);
    }
}
