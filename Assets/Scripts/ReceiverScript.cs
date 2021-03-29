using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverScript : MonoBehaviour
{
    public enum Type { Door, Stairs, Light };

    public Type type;

    public List<GameObject> activatorsNeeded = new List<GameObject>();

    public int numberOfActivatorsOn;

    Vector3 closedPosition;
    Vector3 openPosition;

    public bool isItLockedWhenActivated;

    public Light lightToActivate;

    private Coroutine OpenCoroutine = null;
    private Coroutine CloseCoroutine = null;

    public Vector3 openPositionVector;
    private Vector3 openPositionVectorLocal;
    public float openPositionDistance;
    public float closeTime;
    public float openTime;

    private void Start()
    {
        foreach (GameObject gO in activatorsNeeded)
        {
            gO.GetComponent<EmitterScript>().receiverToActivate = gameObject;
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

    public void SwitchToLightOff()
    {
        lightToActivate.GetComponent<Light>().enabled = false;
    }

    public void SwitchToLightOn()
    {
        lightToActivate.GetComponent<Light>().enabled = true;
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
