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
    private bool takenObjectIsAMap;
    private bool takenObjectIsAChest;
    public bool canTheAvatarDig = true;
    public bool canTheAvatarTake = true;
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

    private void Start()
    {
        movementScript = gameObject.GetComponent<MovementScript>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GetComponent<MovementScript>().canTheAvatarMove && canTheAvatarDig)
        {
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
                isTakingSomething = true;
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
                    takenObjectIsAChest = true;
                    takenObject = hit.collider.gameObject;
                    hit.collider.GetComponent<ChestScript>().isTaken = true;

                    if (hit.collider.GetComponent<ChestScript>().emitterLinked != null)
                    {
                        hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().canAChestBePlaced = true;
                        hit.collider.GetComponent<ChestScript>().emitterLinked.gameObject.GetComponent<EmitterScript>().receiverToActivate.GetComponent<ReceiverScript>().numberOfEmittersOn--;
                    }
                }
            }

            else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength, layerCross))
            {
                if (movementScript.staminaBarImage.fillAmount >= digStaminaDecreaseValueAmount)
                {
                    movementScript.staminaBarImage.fillAmount -= digStaminaDecreaseValueAmount;

                    GameObject newHole = Instantiate(holeWin, hit.point, Quaternion.LookRotation(hit.normal));
                    newHole.transform.position += newHole.transform.TransformDirection(Vector3.forward) * holePlacementYOffset;

                    Destroy(hit.collider.gameObject); //Destroy cross

                    switch (hit.collider.GetComponent<CrossChestTypeScript>().type)
                    {
                        case CrossChestTypeScript.Type.Common:
                            SpawnChest(chestCommon, hit.point, -hit.normal);                            
                            break;
                        case CrossChestTypeScript.Type.Big:
                            SpawnChest(chestBig, hit.point, -hit.normal);
                            break;
                        case CrossChestTypeScript.Type.Giant:
                            SpawnChest(chestGiant, hit.point, -hit.normal);
                            break;
                        case CrossChestTypeScript.Type.Rare:
                            SpawnChest(chestRare, hit.point, -hit.normal);
                            break;
                        case CrossChestTypeScript.Type.Special:
                            SpawnChest(chestSpecial, hit.point, -hit.normal);
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

                    GameObject newHole = Instantiate(hole, hit.point, Quaternion.LookRotation(hit.normal)); //instanciate hole with surface normal rotation

                    newHole.transform.position += newHole.transform.TransformDirection(Vector3.forward) * holePlacementYOffset;
                }
            }

            else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength))
            {
                print("NEW : " + hit.collider.gameObject.name);
                print("NEWLAYER : " + hit.collider.gameObject.layer);
            }
        }

        if (Input.GetMouseButtonDown(1) && isTakingSomething && GetComponent<MovementScript>().canTheAvatarMove && canTheAvatarTake)
        {
            PanelLaunchObjectForce.gameObject.SetActive(true);
            launchObjectBar.fillAmount = 0f;
            launchObjectForce = minLaunchObjectForce;
        }

        if (Input.GetMouseButton(1) && isTakingSomething && GetComponent<MovementScript>().canTheAvatarMove && canTheAvatarTake)
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

    public void SpawnChest (GameObject _chestType, Vector3 _rayPoint, Vector3 _rayNormal)
    {
        GameObject newChest = Instantiate(_chestType, _rayPoint, Quaternion.LookRotation(_rayNormal));
        newChest.GetComponent<Rigidbody>().isKinematic = true;
        newChest.transform.Rotate(Vector3.left, 90f);
        newChest.transform.position -= newChest.transform.TransformDirection(Vector3.up) * (newChest.GetComponent<Collider>().bounds.extents.y);
    }

    public void LaunchTakenObject()
    {
        if (takenObjectIsAMap == true)
        {
            takenObject.transform.parent = null;
            takenObject.GetComponent<Rigidbody>().isKinematic = false;
            takenObject.GetComponent<Rigidbody>().AddForce(transform.forward * launchObjectForce);
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
    }
}
