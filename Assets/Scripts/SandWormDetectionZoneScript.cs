using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormDetectionZoneScript : MonoBehaviour
{
    public GameObject sandWorm;

    private SandWormScript sandWormScript;

    public float minChestMagnitudeToHunt = 10f;

    private void Awake()
    {
        sandWormScript = sandWorm.GetComponent<SandWormScript>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.transform.GetChild(0).GetComponent<MovementScript>().isAvatarRunning == true)
        {
            if (!CheckIfObjectAlreadyOnTheList(other.gameObject, sandWormScript.huntedElementsInTheDetectionZone))
            {
                sandWormScript.huntedElementsInTheDetectionZone.Add(other.gameObject);
                CheckForSandWormActivation();
            }
        }

        else if (other.gameObject.CompareTag("Chest") && other.gameObject.GetComponent<ChestScript>().isTaken == false && other.gameObject.GetComponent<Rigidbody>().velocity.magnitude > minChestMagnitudeToHunt)
        {
            if (!CheckIfObjectAlreadyOnTheList(other.gameObject, sandWormScript.huntedElementsInTheDetectionZone))
            {
                sandWormScript.huntedElementsInTheDetectionZone.Add(other.gameObject);
                CheckForSandWormActivation();
            }
        }
    }

    private bool CheckIfObjectAlreadyOnTheList(GameObject _gO, List<GameObject> _gOList)
    {
        return _gOList.Contains(_gO);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            sandWorm.GetComponent<SandWormScript>().huntedElementsInTheDetectionZone.Remove(other.gameObject);

            CheckForSandWormActivation();
        }

        else if (other.gameObject.CompareTag("Chest"))
        {
            if (CheckIfObjectAlreadyOnTheList(other.gameObject, sandWormScript.huntedElementsInTheDetectionZone))
            {
                sandWorm.GetComponent<SandWormScript>().huntedElementsInTheDetectionZone.Remove(other.gameObject);
                CheckForSandWormActivation();
            }
        }
    }

    private void CheckForSandWormActivation()
    {
        if(sandWorm.GetComponent<SandWormScript>().huntedElementsInTheDetectionZone.Count > 0)
        {
            sandWorm.GetComponent<SandWormScript>().isHuntingPlayer = true;
        }
        else if (sandWorm.GetComponent<SandWormScript>().huntedElementsInTheDetectionZone.Count == 0)
        {
            sandWorm.GetComponent<SandWormScript>().isHuntingPlayer = false;
        }
    }
}
