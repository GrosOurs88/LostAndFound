using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class SearchObjectScript : MonoBehaviour
{
    public float raycastLength;

    public Camera screenshotCam;
    public float topViewCamFOVSizeStart = 5;
    public float topViewCamFOVSizeEnd = 25;
    public GameObject cross;

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

    private void Awake()
    {
        screenshotCam.orthographicSize = topViewCamFOVSizeStart;
    }

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
                hit.collider.transform.parent.parent = avatarHandChest;
                hit.collider.transform.parent.SetPositionAndRotation(avatarHandChest.position, avatarHandChest.rotation);
                hit.collider.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
                foreach (Transform trans in hit.collider.transform.parent)
                {
                    if (trans.GetComponent<Collider>())
                    {
                        trans.GetComponent<Collider>().enabled = false;
                    }
                }
                isTakingSomething = true;
                takenObjectIsAChest = true;
                takenObject = hit.collider.gameObject;
                hit.collider.transform.parent.GetComponent<ChestScript>().isTaken = true;
            }

            else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength, layerCross))
            {
                if (movementScript.staminaBarImage.fillAmount >= digStaminaDecreaseValueAmount)
                {
                    movementScript.staminaBarImage.fillAmount -= digStaminaDecreaseValueAmount;

                    GameObject newHole = Instantiate(holeWin, hit.point, Quaternion.Euler(90, 0, 0));
                    newHole.transform.position = new Vector3(newHole.transform.position.x, 0.01f, newHole.transform.position.z);

                    Destroy(hit.collider.gameObject); //Destroy cross

                    GameObject newChest = Instantiate(chest, hit.point, Quaternion.Euler(0, 0, 0));
                    newChest.GetComponent<Rigidbody>().isKinematic = true;
                    newChest.transform.position = new Vector3(newChest.transform.position.x, -0.4f, newChest.transform.position.z);
                }
            }

            else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength, layerFloor))
            {
                if (movementScript.staminaBarImage.fillAmount >= digStaminaDecreaseValueAmount)
                {
                    movementScript.staminaBarImage.fillAmount -= digStaminaDecreaseValueAmount;

                    GameObject newHole = Instantiate(hole, hit.point, Quaternion.Euler(90, 0, 0));
                    newHole.transform.position = new Vector3(newHole.transform.position.x, 0.01f, newHole.transform.position.z);
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
                launchObjectForce += launchObjectIncreaseValueAmount;
            }
            if (launchObjectForce > maxLaunchObjectForce)
            {
                launchObjectForce = maxLaunchObjectForce;
            }

            launchObjectBar.fillAmount = (launchObjectForce / maxLaunchObjectForce) /*- minLaunchObjectForce*/;
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
            takenObject.transform.parent.parent = null;
            takenObject.transform.parent.GetComponent<Rigidbody>().isKinematic = false;
            foreach (Transform trans in takenObject.transform.parent)
            {
                if (trans.GetComponent<Collider>())
                {
                    trans.GetComponent<Collider>().enabled = true;
                }
            }
            takenObject.transform.parent.GetComponent<Rigidbody>().AddForce(transform.forward * launchObjectForce);
            takenObjectIsAChest = false;
            isTakingSomething = false;
            takenObject.transform.parent.GetComponent<ChestScript>().isTaken = false;
        }

        PanelLaunchObjectForce.gameObject.SetActive(false);
    }

    public void Win()
    {
        transform.parent.transform.GetComponent<MeshRenderer>().enabled = false;
        transform.parent.transform.GetComponent<CapsuleCollider>().enabled = false;

        GetComponent<MovementScript>().canTheAvatarMove = false;
        GetComponent<CameraControlScript>().switchCursorVisible();
        StartCoroutine(WinCinematic(3f));
    }    

    public IEnumerator WinCinematic(float _transitionTime)
    {
        
        yield return null;
    }
}
