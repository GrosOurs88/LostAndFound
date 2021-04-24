using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPositionScript : MonoBehaviour
{
    public GameObject pingPoint;
    public float timeBeforePingPointScaleDown;
    public float timePingPointScaleDown;
    public GameObject pingPointParent;
    private LayerMask layerPingPoint;
    private Coroutine pingPointCoroutine = null;
    private GameObject playerPingPoint = null;

    private void Start()
    { 
        layerPingPoint = LayerMask.GetMask("Default") | LayerMask.GetMask("Floor") | LayerMask.GetMask("Map") | LayerMask.GetMask("Chest");

        playerPingPoint = Instantiate(pingPoint, Vector3.zero, Quaternion.identity, pingPointParent.transform);
        playerPingPoint.transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(2) && GetComponent<MovementScript>().canTheAvatarMove)
        {
            LaunchRaycast(layerPingPoint, playerPingPoint, Mathf.Infinity);
        }
    }
    
    private void LaunchRaycast(LayerMask _layer, GameObject _objectToInstantiate, float _raycastLength)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _raycastLength, _layer))
        {
            playerPingPoint.transform.position = hit.point;
            StopCoroutine(StartPingPoint(timeBeforePingPointScaleDown, timePingPointScaleDown));
            pingPointCoroutine = StartCoroutine(StartPingPoint(timeBeforePingPointScaleDown, timePingPointScaleDown));
        }
    }

    private IEnumerator StartPingPoint(float _timeWait, float _timeScaleDown)
    {
        playerPingPoint.transform.localScale = Vector3.one;
        float elapsedTime = 0;

        while (elapsedTime < _timeWait)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        Vector3 startingScale = Vector3.one;
        Vector3 endingScale = Vector3.zero;
        elapsedTime = 0;
        while (elapsedTime < _timeScaleDown)
        {
            playerPingPoint.transform.localScale = Vector3.Lerp(startingScale, endingScale, (elapsedTime / _timeScaleDown));

            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
