using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiverScript : MonoBehaviour
{
    public enum Type { Door, Stairs, Light };

    public Type type;

    public List<GameObject> emitters = new List<GameObject>();

    public int numberOfEmittersNeeded;

    [HideInInspector]
    public int numberOfEmittersOn;

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

    public float shakeAmount;

    private void Start()
    {
        SetupEmitters();
    }

    public void SetupEmitters()
    {
        foreach (GameObject gO in emitters)
        {
            gO.GetComponent<EmitterScript>().receiverToActivate = gameObject;
        }

        for (int i = numberOfEmittersNeeded; i < emitters.Count; i++)
        {
            emitters[i].gameObject.SetActive(false);
        }

        openPositionVectorLocal = transform.TransformDirection(openPositionVector);

        //if (openTime > 0f)
        //{
        //    closedPosition = transform.position;
        //    openPosition = transform.position + (openPositionVectorLocal * openPositionDistance);

        //    OpenCoroutine = StartCoroutine(Open(openTime));
        //    StopCoroutine(OpenCoroutine);
        //    CloseCoroutine = StartCoroutine(Close(closeTime));
        //}
    }

    //public void SwitchToClose()
    //{
    //    StopCoroutine(OpenCoroutine);
    //    CloseCoroutine = StartCoroutine(Close(closeTime));
    //}

    public void SwitchToOpen()
    {
        //StopCoroutine(CloseCoroutine);
        OpenCoroutine = StartCoroutine(Open(openTime));
    }

    public void UpdateDoorLevel()
    {
        if(doorLevel)
        {
            doorLevel.fillAmount = (float)numberOfEmittersOn / (float)numberOfEmittersNeeded;
        }
    }

    public IEnumerator Open(float time)
    {
        Vector3 startingPos = transform.position;
        Vector3 finalPos = transform.position + (openPositionVectorLocal * openPositionDistance); ;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));

            transform.position += Random.insideUnitSphere * shakeAmount;

            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    //public IEnumerator Close(float time)
    //{
    //    Vector3 startingPos = transform.position;
    //    Vector3 finalPos = closedPosition;
    //    float elapsedTime = 0;

    //    while (elapsedTime<time)
    //    {
    //        transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //}
}
