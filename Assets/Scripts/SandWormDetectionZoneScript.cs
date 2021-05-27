using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormDetectionZoneScript : MonoBehaviour
{
    public GameObject sandWorm;

    private SandWormScript sandWormScript;

    public float minChestMagnitudeToHunt = 10f;

    private GameObject objectTriggered;

    private void Awake()
    {
        sandWormScript = sandWorm.GetComponent<SandWormScript>();
    }

    private void OnTriggerStay(Collider other)
    {
        objectTriggered = other.gameObject;

        if (objectTriggered.CompareTag("Player") && objectTriggered.GetComponent<PlayerControllerScript>().speedMode == PlayerControllerScript.PlayerSpeed.Run)
        {
            if (!CheckIfObjectAlreadyOnTheList(objectTriggered, sandWormScript.huntedElementsInTheDetectionZone))
            {
                sandWormScript.huntedElementsInTheDetectionZone.Add(objectTriggered);
                CheckForSandWormActivation();
            }
        }

        else if (objectTriggered.CompareTag("Chest") && objectTriggered.GetComponent<ChestScript>().isTaken == false && objectTriggered.GetComponent<Rigidbody>().velocity.magnitude > minChestMagnitudeToHunt)
        {
            if (!CheckIfObjectAlreadyOnTheList(objectTriggered, sandWormScript.huntedElementsInTheDetectionZone))
            {
                sandWormScript.huntedElementsInTheDetectionZone.Add(objectTriggered);
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
        objectTriggered = other.gameObject;

        if (objectTriggered.CompareTag("Player"))
        {
            sandWorm.GetComponent<SandWormScript>().huntedElementsInTheDetectionZone.Remove(objectTriggered);

            CheckForSandWormActivation();
        }

        else if (objectTriggered.CompareTag("Chest"))
        {
            if (CheckIfObjectAlreadyOnTheList(objectTriggered, sandWormScript.huntedElementsInTheDetectionZone))
            {
                sandWorm.GetComponent<SandWormScript>().huntedElementsInTheDetectionZone.Remove(objectTriggered);
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
