using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverScript : MonoBehaviour
{
    public enum Type { Door, Stairs };

    public Type type;

    public List<GameObject> activatorsNeeded = new List<GameObject>();

    public int numberOfActivatorsOn;

    Vector3 closedPosition;
    Vector3 openPosition;

    private Coroutine OpenCoroutine = null;
    private Coroutine CloseCoroutine = null;

    private void Start()
    {
        foreach(GameObject gO in activatorsNeeded)
        {
            gO.GetComponent<EmitterScript>().receiverToActivate = gameObject;
        }

        OpenCoroutine = StartCoroutine(Open(3f));
        CloseCoroutine = StartCoroutine(Close(1f));

        closedPosition = transform.position;
        openPosition = transform.position + (transform.up * 15);
    }

    public void SwitchToClose()
    {
        StopCoroutine(OpenCoroutine);
        CloseCoroutine = StartCoroutine(Close(1f));
    }

    public void SwitchToOpen()
    {
        StopCoroutine(CloseCoroutine);
        OpenCoroutine = StartCoroutine(Open(3f));
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
