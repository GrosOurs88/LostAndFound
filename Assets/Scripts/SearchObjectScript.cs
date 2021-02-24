using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SearchObjectScript : MonoBehaviour
{
    public float raycastLength;

    public Camera screenshotCam;
    public Camera topViewCam;
    public float topViewCamFOVSizeStart = 5;
    public float topViewCamFOVSizeEnd = 25;
    public Material mapMaterial;
    public GameObject cross;

    public GameObject hole;
    public GameObject holeWin;

    public string crossTag;

    private void Awake()
    {
        screenshotCam.orthographicSize = topViewCamFOVSizeStart;
    }

    private void Start()
    {
        PlaceCameraAndCross();
        StartCoroutine(TakeScreenshot());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GetComponent<MovementScript>().canTheAvatarMove)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastLength))
            {
                if (hit.collider.isTrigger && hit.transform.CompareTag(crossTag))
                {
                    GameObject newHole = Instantiate(holeWin, hit.point, Quaternion.Euler(90, 0, 0));
                    newHole.transform.position = new Vector3(newHole.transform.position.x, 0.01f, newHole.transform.position.z);

                    // MAKE THE CHEST APPEARS
                }

                else
                {
                    GameObject newHole = Instantiate(hole, hit.point, Quaternion.Euler(90, 0, 0));
                    newHole.transform.position = new Vector3(newHole.transform.position.x, 0.01f, newHole.transform.position.z);
                }
            }
        }
    }

    public void Win()
    {
        transform.parent.transform.GetComponent<MeshRenderer>().enabled = false;
        transform.parent.transform.GetComponent<CapsuleCollider>().enabled = false;

        GetComponent<MovementScript>().canTheAvatarMove = false;
        GetComponent<CameraControlScript>().switchCursorVisible();
        StartCoroutine(WinCinematic(3f));
    }

    public void PlaceCameraAndCross()
    {
        //Place the screenshot camera in a random X-Z position inside the floor gameobject box
        screenshotCam.transform.position = new Vector3(Random.Range(InstantiateObjectsScript.instance.objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.x + InstantiateObjectsScript.instance.instantiationOffsetCameraX, InstantiateObjectsScript.instance.objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.x - InstantiateObjectsScript.instance.instantiationOffsetCameraX),
                                                    (screenshotCam.transform.position.y),
                                                    Random.Range(InstantiateObjectsScript.instance.objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.z + InstantiateObjectsScript.instance.instantiationOffsetCameraZ, InstantiateObjectsScript.instance.objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.z - InstantiateObjectsScript.instance.instantiationOffsetCameraZ));

        //Place a new cross under the random X-Z screenshot camera position
        cross.transform.position = new Vector3(screenshotCam.transform.position.x, InstantiateObjectsScript.instance.objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.y + 0.1f, screenshotCam.transform.position.z);
    }

    public IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        cross.GetComponent<SpriteRenderer>().enabled = true;
        screenshotCam.gameObject.SetActive(true);

        RenderTexture renderTexture = null;
        screenshotCam.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;
        screenshotCam.Render();

        Texture2D texture = new Texture2D(screenshotCam.pixelWidth, screenshotCam.pixelHeight, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(0f, 0f, texture.width, texture.height), 0, 0);
        texture.Apply();
        mapMaterial.SetTexture("_MainTex", texture);

        RenderTexture.active = null;
        screenshotCam.targetTexture = null;

        screenshotCam.gameObject.SetActive(false);

        yield return null;
    }

    public IEnumerator WinCinematic(float _transitionTime)
    {
        topViewCam.gameObject.SetActive(true);

        Vector3 startPos = new Vector3(transform.parent.transform.position.x, topViewCam.transform.position.y, transform.parent.transform.position.z);
        topViewCam.transform.position = startPos;

        Vector3 finalPos = new Vector3(InstantiateObjectsScript.instance.objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.center.x,
                                       topViewCam.transform.position.y,
                                       InstantiateObjectsScript.instance.objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.center.z);


        float elapsedTime = 0;
        while (elapsedTime < _transitionTime)
        {
            topViewCam.transform.position = Vector3.Lerp(startPos, finalPos, (elapsedTime / _transitionTime));
            topViewCam.orthographicSize = Mathf.Lerp(topViewCamFOVSizeStart, topViewCamFOVSizeEnd, (elapsedTime / _transitionTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Add button replay

        yield return null;
    }
}
