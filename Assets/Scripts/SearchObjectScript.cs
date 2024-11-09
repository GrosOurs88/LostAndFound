using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class SearchObjectScript : MonoBehaviour
{
    public string inputTakeAndLaunchObject;

    public float raycastLength;

    public GameObject hole;
    public GameObject holeWin;

    public GameObject chestCommon;
    public GameObject chestBig;
    public GameObject chestGiant;
    public GameObject chestRare;
    public GameObject chestSpecial;

    public Transform avatarHandMap;
    public Transform avatarHandSmallChest;
    public Transform avatarHandBigChest;
    public Transform avatarHandGiantChest;

    public float digStaminaDecreaseValueAmount;

    [HideInInspector]
    public bool isTakingSomething = false;
    public bool canLaunch = false;
    public bool hasLaunched = false;

    private bool takenObjectIsAMap;
    private bool takenObjectIsAChest;
    // [HideInInspector]
    public bool canTheAvatarDig = true;
    public bool isTheAvatarDigging = false;
    // [HideInInspector]
    public bool canTheAvatarTake = true;
    public bool isTheAvatarTaking = false;
    private GameObject takenObject;

    public float launchObjectForce;
    public float minLaunchObjectForce = 200f;
    public float maxLaunchObjectForce = 600f;
    public float launchObjectIncreaseValueAmount;
    public GameObject PanelLaunchObjectForce;
    public Image launchObjectBar;

    public float smallChestWeight;
    public float bigChestWeight;
    public float giantChestWeight;

    MovementScript movementScript;

    public float holePlacementYOffset;
    public float holeWinPlacementYOffset;

    public float chestSpawnedYOffset;

    private void Start()
    {
        movementScript = gameObject.GetComponent<MovementScript>();
    }

    void Update()
    {
        if (Input.GetAxis(inputTakeAndLaunchObject) == 1)
        {
            if (GetComponent<MovementScript>().canTheAvatarMove && canTheAvatarDig && isTheAvatarDigging == false)
            {
                isTheAvatarDigging = true;

                RaycastHit hit;
                LayerMask layerDefault = LayerMask.GetMask("Default");
                LayerMask layerCross = LayerMask.GetMask("Cross");
                LayerMask layerFloor = LayerMask.GetMask("Floor");
                LayerMask layerMap = LayerMask.GetMask("Map");
                LayerMask layerChest = LayerMask.GetMask("Chest");

                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength, layerMap) && isTakingSomething == false)
                {
                    hit.collider.transform.parent = avatarHandMap;
                    hit.collider.transform.SetPositionAndRotation(avatarHandMap.position, avatarHandMap.rotation);
                    hit.collider.GetComponent<Rigidbody>().isKinematic = true;
                    hit.collider.gameObject.GetComponent<MapScript>().isCurrentlyTaken = true;
                    hit.collider.gameObject.GetComponent<MapScript>().playerWhoOwnsTheMap = gameObject;
                    isTakingSomething = true;
                    canTheAvatarTake = false;
                    canTheAvatarDig = false;
                    takenObjectIsAMap = true;
                    takenObject = hit.collider.gameObject;
                }

                else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength, layerChest) && isTakingSomething == false)
                {
                    if (hit.collider.transform.GetComponent<ChestScript>().canBeTaken)
                    {
                        if (hit.collider.GetComponent<Rigidbody>().mass == smallChestWeight)
                        {
                            hit.collider.transform.parent = avatarHandSmallChest;
                            hit.collider.transform.SetPositionAndRotation(avatarHandSmallChest.position, avatarHandSmallChest.rotation);
                        }
                        else if (hit.collider.GetComponent<Rigidbody>().mass == bigChestWeight)
                        {
                            hit.collider.transform.parent = avatarHandBigChest;
                            hit.collider.transform.SetPositionAndRotation(avatarHandBigChest.position, avatarHandBigChest.rotation);
                        }
                        else if (hit.collider.GetComponent<Rigidbody>().mass == giantChestWeight)
                        {
                            hit.collider.transform.parent = avatarHandGiantChest;
                            hit.collider.transform.SetPositionAndRotation(avatarHandGiantChest.position, avatarHandGiantChest.rotation);
                        }

                        hit.collider.GetComponent<Rigidbody>().isKinematic = true;

                        isTakingSomething = true;
                        canTheAvatarTake = false;
                        canTheAvatarDig = false;
                        takenObjectIsAChest = true;
                        takenObject = hit.collider.gameObject;
                        hit.collider.GetComponent<ChestScript>().isTaken = true;

                        if (hit.collider.GetComponent<ChestScript>().emitterLinked != null)
                        {
                            hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().canAChestBePlaced = true;
                            hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().receiverToActivate.GetComponent<ReceiverScript>().numberOfEmittersOn--;

                            if (hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().receiverToActivate.GetComponent<ReceiverScript>().numberOfEmittersOn == hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().receiverToActivate.GetComponent<ReceiverScript>().emitters.Count - 1
                                && hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().receiverToActivate.GetComponent<ReceiverScript>().isItLockedWhenActivated == false)
                            {
                                switch (hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().receiverToActivate.GetComponent<ReceiverScript>().type)
                                {
                                    case ReceiverScript.Type.Door:
                                        //hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().receiverToActivate.GetComponent<ReceiverScript>().SwitchToClose();
                                        break;
                                    case ReceiverScript.Type.Stairs:
                                        break;
                                    default:
                                        Debug.Log("NOTHING");
                                        break;
                                }
                            }
                        }
                    }
                }

                else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength, layerCross))
                {
                    if (movementScript.staminaBarImage.fillAmount >= digStaminaDecreaseValueAmount)
                    {
                        movementScript.staminaBarImage.fillAmount -= digStaminaDecreaseValueAmount;

                        GameObject newHole = Instantiate(holeWin, hit.point, Quaternion.LookRotation(hit.normal));
                        newHole.transform.position = new Vector3(newHole.transform.position.x, hit.point.y + holeWinPlacementYOffset, newHole.transform.position.z);

                        hit.collider.gameObject.GetComponent<CrossChestTypeScript>().DestroyCrossAndMap(); //Destroy cross and linked map

                        switch (hit.collider.GetComponent<CrossChestTypeScript>().type)
                        {
                            case CrossChestTypeScript.Type.Common:
                                GameObject newChestCommon = Instantiate(chestCommon, hit.point, Quaternion.Euler(0, 0, 0));
                                newChestCommon.GetComponent<Rigidbody>().isKinematic = true;
                                newChestCommon.transform.position = new Vector3(hit.point.x, hit.point.y + chestSpawnedYOffset, hit.point.z);
                                break;
                            case CrossChestTypeScript.Type.Big:
                                GameObject newChestBig = Instantiate(chestBig, hit.point, Quaternion.Euler(0, 0, 0));
                                newChestBig.GetComponent<Rigidbody>().isKinematic = true;
                                newChestBig.transform.position = new Vector3(hit.point.x, hit.point.y + chestSpawnedYOffset, hit.point.z);
                                break;
                            case CrossChestTypeScript.Type.Giant:
                                GameObject newChestGiant = Instantiate(chestGiant, hit.point, Quaternion.Euler(0, 0, 0));
                                newChestGiant.GetComponent<Rigidbody>().isKinematic = true;
                                newChestGiant.transform.position = new Vector3(hit.point.x, hit.point.y + chestSpawnedYOffset, hit.point.z);
                                break;
                            case CrossChestTypeScript.Type.Rare:
                                GameObject newChestRare = Instantiate(chestRare, hit.point, Quaternion.Euler(0, 0, 0));
                                newChestRare.GetComponent<Rigidbody>().isKinematic = true;
                                newChestRare.transform.position = new Vector3(hit.point.x, hit.point.y + chestSpawnedYOffset, hit.point.z);
                                break;
                            case CrossChestTypeScript.Type.Special:
                                GameObject newChestSpecial = Instantiate(chestSpecial, hit.point, Quaternion.Euler(0, 0, 0));
                                newChestSpecial.GetComponent<Rigidbody>().isKinematic = true;
                                newChestSpecial.transform.position = new Vector3(hit.point.x, hit.point.y + chestSpawnedYOffset, hit.point.z);
                                break;
                            default:
                                Debug.Log("NOTHING");
                                break;
                        }
                    }
                }

                else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength, layerDefault))
                {
                    return;
                }

                else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength, layerFloor))
                {
                    if (movementScript.staminaBarImage.fillAmount >= digStaminaDecreaseValueAmount)
                    {
                        movementScript.staminaBarImage.fillAmount -= digStaminaDecreaseValueAmount;

                        GameObject newHole = Instantiate(hole, hit.point, Quaternion.LookRotation(hit.normal));
                        newHole.transform.position = new Vector3(newHole.transform.position.x, hit.point.y + holePlacementYOffset, newHole.transform.position.z);
                    }
                }

                else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength))
                {
                    print("NEW : " + hit.collider.gameObject.name);
                    print("NEWLAYER : " + hit.collider.gameObject.layer);
                }
            }

            if (isTakingSomething && GetComponent<MovementScript>().canTheAvatarMove && canTheAvatarTake == false && isTheAvatarTaking == false)
            {
                isTheAvatarTaking = true;
                canTheAvatarDig = false;                
                launchObjectBar.fillAmount = 0f;
                launchObjectForce = minLaunchObjectForce;
            }

            if (isTakingSomething && GetComponent<MovementScript>().canTheAvatarMove && canTheAvatarTake == false && hasLaunched)
            {
                PanelLaunchObjectForce.gameObject.SetActive(true);

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

            if(canLaunch)
            {
                hasLaunched = true;
            }
        }

        if (Input.GetAxis(inputTakeAndLaunchObject) == 0)
        {
            if(isTakingSomething)
            {
                canLaunch = true;
            }

            if(hasLaunched)
            {
                LaunchTakenObject();

                isTakingSomething = false;
                canLaunch = false;
                hasLaunched = false;
            }

            isTheAvatarDigging = false;
        }
    }

    public void LaunchTakenObject()
    {
        if (takenObjectIsAMap == true)
        {
            takenObject.transform.parent = null;
            takenObject.GetComponent<Rigidbody>().isKinematic = false;
            takenObject.GetComponent<Rigidbody>().AddForce(transform.forward * launchObjectForce);
            takenObject.gameObject.GetComponent<MapScript>().isCurrentlyTaken = false;
            takenObject.gameObject.GetComponent<MapScript>().playerWhoOwnsTheMap = null;
            takenObjectIsAMap = false;
            isTakingSomething = false;
        }

        else if (takenObjectIsAChest == true)
        {
            takenObject.transform.parent = null;
            takenObject.GetComponent<Rigidbody>().isKinematic = false;
            takenObject.GetComponent<Rigidbody>().AddForce(transform.forward * launchObjectForce);
            takenObjectIsAChest = false;
            isTakingSomething = false;
            takenObject.GetComponent<ChestScript>().isTaken = false;
        }

        PanelLaunchObjectForce.gameObject.SetActive(false);
        isTheAvatarTaking = false;
        canTheAvatarDig = true;
    }
}
