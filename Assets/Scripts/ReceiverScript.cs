using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiverScript : MonoBehaviour
{
    public enum Type { Door, Stairs, Light };

    public Type type;

    public List<GameObject> activators = new List<GameObject>();

    public int numberOfActivatorsNeeded;

    [HideInInspector]
    public int numberOfActivatorsOn;

    Vector3 closedPosition;
    Vector3 openPosition;

    public Image doorLevel;

    public bool isItLockedWhenActivated;

    private Coroutine OpenCoroutine = null;
    private Coroutine CloseCoroutine = null;

    public Vector3 openPositionVector;
    private Vector3 openPositionVectorLocal;
    public float openPositionDistance;
    public float closeTime;
    public float openTime;

    private void Start()
    {
        foreach (GameObject gO in activators)
        {
            gO.GetComponent<EmitterScript>().receiverToActivate = gameObject;
        }

        for (int i = 0; i < numberOfActivatorsNeeded; i++)
        {
            activators[i].gameObject.SetActive(true);
        }

        openPositionVectorLocal = transform.TransformDirection(openPositionVector);

        if(openTime > 0f)
        {
            closedPosition = transform.position;
            openPosition = transform.position + (openPositionVectorLocal * openPositionDistance);

            OpenCoroutine = StartCoroutine(Open(openTime));
            StopCoroutine(OpenCoroutine);
            CloseCoroutine = StartCoroutine(Close(closeTime));
        }
    }

    public void SwitchToClose()
    {
        StopCoroutine(OpenCoroutine);
        CloseCoroutine = StartCoroutine(Close(closeTime));
    }

    public void SwitchToOpen()
    {
        StopCoroutine(CloseCoroutine);
        OpenCoroutine = StartCoroutine(Open(openTime));
    }

    public void UpdateDoorLevel()
    {
        print("ON" + numberOfActivatorsOn);

        doorLevel.fillAmount = (float)numberOfActivatorsOn / (float)numberOfActivatorsNeeded;
    }

    public IEnumerator Open(float time)
    {
        Vector3 startingPos = transform.position;
        Vector3 finalPos = openPosition;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator Close(float time)
    {
        Vector3 startingPos = transform.position;
        Vector3 finalPos = closedPosition;
        float elapsedTime = 0;

        while (elapsedTime<time)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
