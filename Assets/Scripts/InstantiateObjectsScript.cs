using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObjectsScript : MonoBehaviour
{
    public GameObject objectToInstantiateObjectsInto;
    public int numberOfObjectsToInstantiate;
    public float instantiationOffsetX;
    public float instantiationOffsetY;
    public float instantiationOffsetZ;

    public float instantiationOffsetCameraX;
    public float instantiationOffsetCameraZ;

    public bool randomizeRotationX = false;
    public bool randomizeRotationY = false;
    public bool randomizeRotationZ = false;

    public List<GameObject> objectsToInstantiate = new List<GameObject>();
    private GameObject nextObjectToInstantiate;
    private Vector3 nextObjectToInstanciatePosition;
    public GameObject ObjectsInstiatedFolder;
    private int x;

    public List<GameObject> mapList = new List<GameObject>();
    public List<Material> mapMaterialList = new List<Material>();
    public List<Camera> cameraList = new List<Camera>();
    public List<GameObject> crossList = new List<GameObject>();

    public float maxScore;


    void Start()
    {
        InstantiateEnvironment();

        for (int i = 0; i < mapList.Count; i++)
        {
            PlaceCameraAndCross(i, i);
            StartCoroutine(TakeScreenshot(i, i, i, i));
        }
    }   

    public void InstantiateEnvironment()
    {
        //Remove previous objects if exists
        foreach(Transform trans in ObjectsInstiatedFolder.transform)
        {
            Destroy(trans.gameObject);
        }

        //Loop for all the objects that havce to be placed
        for (int i = 0; i < numberOfObjectsToInstantiate; i++)
        {
            //Choose an object to instantiate randomly
            x = Random.Range(0, objectsToInstantiate.Count-1);
            nextObjectToInstantiate = objectsToInstantiate[x];

            //Choose a random position to spawn the object
            nextObjectToInstanciatePosition = new Vector3(Random.Range(objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.x + instantiationOffsetX, objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.x - instantiationOffsetX),
                                                          Random.Range(objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.y + instantiationOffsetY, objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.y - instantiationOffsetY),
                                                          Random.Range(objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.z + instantiationOffsetZ, objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.z - instantiationOffsetZ));

            //Instantiate the object and rotate it randmly
            if (randomizeRotationX)
            {
                Instantiate(nextObjectToInstantiate, nextObjectToInstanciatePosition, Quaternion.Euler(Random.Range(0.0f, 360.0f), 0.0f, 0.0f), ObjectsInstiatedFolder.transform);
            }
            if (randomizeRotationY)
            {
                Instantiate(nextObjectToInstantiate, nextObjectToInstanciatePosition, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f), ObjectsInstiatedFolder.transform);
            }
            if (randomizeRotationZ)
            {
                Instantiate(nextObjectToInstantiate, nextObjectToInstanciatePosition, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)), ObjectsInstiatedFolder.transform);
            }
        }
    }

    public void PlaceCameraAndCross(int _cameraIndex, int _crossIndex)
    {
        //Place the screenshot camera in a random X-Z position inside the floor gameobject box
        cameraList[_cameraIndex].transform.position = new Vector3(Random.Range(objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.x + instantiationOffsetCameraX, objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.x - instantiationOffsetCameraX),
                                                    (cameraList[_cameraIndex].transform.position.y),
                                                    Random.Range(objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.z + instantiationOffsetCameraZ, objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.z - instantiationOffsetCameraZ));

        //Place a new cross under the random X-Z screenshot camera position
        crossList[_crossIndex].transform.position = new Vector3(cameraList[_cameraIndex].transform.position.x, objectToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.y + 0.1f, cameraList[_cameraIndex].transform.position.z);
    }

    public IEnumerator TakeScreenshot(int _materialIndex, int _mapIndex, int _cameraIndex, int _crossIndex)
    {
        yield return new WaitForEndOfFrame();

        crossList[_crossIndex].GetComponent<SpriteRenderer>().enabled = true;
        cameraList[_cameraIndex].gameObject.SetActive(true);

        RenderTexture renderTexture = null;
        cameraList[_cameraIndex].targetTexture = renderTexture;
        RenderTexture.active = renderTexture;
        cameraList[_cameraIndex].Render();

        Texture2D texture = new Texture2D(cameraList[_cameraIndex].pixelWidth, cameraList[_cameraIndex].pixelHeight, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(0f, 0f, texture.width, texture.height), 0, 0);
        texture.Apply();
        mapMaterialList[_materialIndex].SetTexture("_MainTex", texture);

        mapList[_mapIndex].GetComponent<Renderer>().material = mapMaterialList[_materialIndex];

        RenderTexture.active = null;
        cameraList[_cameraIndex].targetTexture = null;

        cameraList[_cameraIndex].gameObject.SetActive(false);

        yield return null;
    }
}
