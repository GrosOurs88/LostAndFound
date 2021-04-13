using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObjectsScript : MonoBehaviour
{
    [Header("InstantiatedElementParents")]
    public GameObject floorToInstantiateObjectsInto;
    public GameObject decorInstiatedFolder;
    public GameObject screenshotCamerasInstiatedFolder;
    public GameObject crossesInstiatedFolder;

    [Header("Decor")]
    public int numberOfObjectsToInstantiate;
   // public bool randomizeRotationX = false;
    public bool randomizeRotationY = true;
   // public bool randomizeRotationZ = false;
    public List<GameObject> objectsToInstantiate = new List<GameObject>();
    private GameObject nextObjectToInstantiate;
    private Vector3 nextObjectToInstanciatePosition;
    private int x;

    [Header("Maps")]
    public Shader shader;
    public Color color;
    public GameObject mapsFolder;
    private List<GameObject> mapList = new List<GameObject>();

    [Header("Cameras")]
    public GameObject screenshotCamera;
    private int randomSectionToPutSpecialChestOn; //Used only for SectionStart chests instantiation
    private List<GameObject> cameraList = new List<GameObject>();
    public float cameraOrthographicSize = 5;
    public float instantiationOffsetCameraHeight = 50f;
    private float instantiationOffsetCameraX; //automatically calculated relatively to the cameraOrthographicSize
    private float instantiationOffsetCameraZ; //automatically calculated relatively to the cameraOrthographicSize

    [Header("Crosses")]
    public float crossPlacementYOffset;
    public int crossPlacementMaxIterationNumber = 5;
    private List<GameObject> crossList = new List<GameObject>();
    public GameObject crossChestCommon;
    public int numberOfCrossChestCommon;
    public GameObject crossChestBig;
    public int numberOfCrossChestBig;
    public GameObject crossChestGiant;
    public int numberOfCrossChestGiant;
    public GameObject crossChestRare;
    public int numberOfCrossChestRare;
    public GameObject crossChestSpecial;
    public int numberOfCrossChestSpecial;

    void Start()
    {
        instantiationOffsetCameraX = cameraOrthographicSize / 2; //automatically calculated relatively to the cameraOrthographicSize
        instantiationOffsetCameraZ = cameraOrthographicSize / 2; //automatically calculated relatively to the cameraOrthographicSize
    }   

    public void SetupEnvironment()
    {
        InstantiateEnvironment();
        CreateMapsList();
        CreateCrossesList();
        CreateScreenshotCamerasList(); //number of screenshotCameras = number of crosses

        for (int i = 0; i < crossList.Count; i++)
        {
            PlaceCross(i);
            PlaceCamera(i, i);
            StartCoroutine(TakeScreenshot(i, i, i));
        }
    }

    public void StartSectionSetupEnvironment(List<GameObject> _sectionsList)
    {
        InstantiateEnvironment();
        CreateMapsList();
        CreateCrossesList();
        CreateScreenshotCamerasList(); //number of screenshotCameras = number of crosses

        for (int i = 0; i < crossList.Count; i++)
        {
            PlaceCrossOnRandomSection(i, _sectionsList);
            PlaceCamera(i, i);
            StartCoroutine(TakeScreenshot(i, i, i));
        }
    }

    public void InstantiateEnvironment()
    {
        //Remove previous objects if exists
        foreach(Transform trans in decorInstiatedFolder.transform)
        {
            Destroy(trans.gameObject);
        }

        //Loop for all the objects that have to be placed
        for (int i = 0; i < numberOfObjectsToInstantiate; i++)
        {
            //Choose an object to instantiate randomly
            x = Random.Range(0, objectsToInstantiate.Count-1);
            nextObjectToInstantiate = objectsToInstantiate[x];

            //Choose a random position to spawn the object
            nextObjectToInstanciatePosition = new Vector3(Random.Range(floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.x, floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.x),
                                                          floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.y,
                                                          Random.Range(floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.z, floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.z));

            //Instantiate the object and rotate it randomly
            if (randomizeRotationY)
            {
                Instantiate(nextObjectToInstantiate, nextObjectToInstanciatePosition, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f), decorInstiatedFolder.transform);
            }
        }
    }

    private void CreateMapsList()
    {
        Transform[] childrens = GetComponentsInChildren<Transform>();

        foreach (Transform tr in childrens)
        {
            if (tr.CompareTag("Map"))
            {
                mapList.Add(tr.gameObject);
            }
        }
    }

    private void CreateCrossesList()
    {
        for(int i = 0; i < numberOfCrossChestCommon; i++)
        {
            GameObject nextCross = Instantiate(crossChestCommon, Vector3.zero, Quaternion.Euler(90.0f, Random.Range(0.0f, 360.0f), 0.0f), crossesInstiatedFolder.transform);
            crossList.Add(nextCross);
        }
        for (int i = 0; i < numberOfCrossChestBig; i++)
        {
            GameObject nextCross = Instantiate(crossChestBig, Vector3.zero, Quaternion.Euler(90.0f, Random.Range(0.0f, 360.0f), 0.0f), crossesInstiatedFolder.transform);
            crossList.Add(nextCross);
        }
        for (int i = 0; i < numberOfCrossChestGiant; i++)
        {
            GameObject nextCross = Instantiate(crossChestGiant, Vector3.zero, Quaternion.Euler(90.0f, Random.Range(0.0f, 360.0f), 0.0f), crossesInstiatedFolder.transform);
            crossList.Add(nextCross);
        }
        for (int i = 0; i < numberOfCrossChestRare; i++)
        {
            GameObject nextCross = Instantiate(crossChestRare, Vector3.zero, Quaternion.Euler(90.0f, Random.Range(0.0f, 360.0f), 0.0f), crossesInstiatedFolder.transform);
            crossList.Add(nextCross);
        }
        for (int i = 0; i < numberOfCrossChestSpecial; i++)
        {
            GameObject nextCross = Instantiate(crossChestSpecial, Vector3.zero, Quaternion.Euler(90.0f, Random.Range(0.0f, 360.0f), 0.0f), crossesInstiatedFolder.transform);
            crossList.Add(nextCross);
        }
    }

    private void CreateScreenshotCamerasList()
    {
        for (int i = 0; i < crossList.Count; i++)
        {
            GameObject nextCamera = Instantiate(screenshotCamera, Vector3.zero, Quaternion.Euler(Vector3.right*90), screenshotCamerasInstiatedFolder.transform);
            cameraList.Add(nextCamera);
        }
    }

    public void PlaceCross(int _crossIndex)
    {
        //layer for shooting raycast on ground
        RaycastHit hit;
        LayerMask layerLava = LayerMask.GetMask("Lava");
        LayerMask layerDefault = LayerMask.GetMask("Default");
        LayerMask layerCross = LayerMask.GetMask("Cross");
        LayerMask combinedLayer = layerLava | layerDefault | layerCross;

        int i = 0;
        while (i < crossPlacementMaxIterationNumber)
        {
            //Place the screenshot camera in a random X-Z position inside the floor gameobject box
            crossList[_crossIndex].transform.position = new Vector3(Random.Range(floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.x + instantiationOffsetCameraX, floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.x - instantiationOffsetCameraX),
                                                                    floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.y + crossPlacementYOffset,
                                                                    Random.Range(floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.z + instantiationOffsetCameraZ, floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.z - instantiationOffsetCameraZ));

            Vector3 crossPosWithYOffset = new Vector3(crossList[_crossIndex].transform.position.x, crossList[_crossIndex].transform.position.y + 2f, crossList[_crossIndex].transform.position.z); 

            if (Physics.Raycast(crossPosWithYOffset, transform.TransformDirection(Vector3.down), out hit, 3f, combinedLayer)) //Si la croix est placée sur un élément de décor, une autre croix ou de la lave
            {
                i++; //Replace la croix à un autre endroit
            }
            else
            {
                break;
            }
        }
    }

    //Used for the start zone maps
    public void PlaceCrossOnRandomSection(int _crossIndex, List<GameObject> _sectionsList)
    {
        //layer for shooting raycast on ground
        RaycastHit hit;
        LayerMask layerLava = LayerMask.GetMask("Lava");
        LayerMask layerDefault = LayerMask.GetMask("Default");
        LayerMask layerCross = LayerMask.GetMask("Cross");
        LayerMask combinedLayer = layerLava | layerDefault | layerCross;

        int i = 0;
        while (i < crossPlacementMaxIterationNumber)
        {
            //Take random section from sectionsList
            randomSectionToPutSpecialChestOn = Random.Range(0, _sectionsList.Count);

            //Place the screenshot camera in a random X-Z position inside the floor gameobject box
            crossList[_crossIndex].transform.position = new Vector3(Random.Range(_sectionsList[randomSectionToPutSpecialChestOn].GetComponent<InstantiateObjectsScript>().floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.x + instantiationOffsetCameraX, _sectionsList[randomSectionToPutSpecialChestOn].GetComponent<InstantiateObjectsScript>().floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.x - instantiationOffsetCameraX),
                                                                    _sectionsList[randomSectionToPutSpecialChestOn].GetComponent<InstantiateObjectsScript>().floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.y + crossPlacementYOffset,                                                                    
                                                                    Random.Range(_sectionsList[randomSectionToPutSpecialChestOn].GetComponent<InstantiateObjectsScript>().floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.z + instantiationOffsetCameraZ, _sectionsList[randomSectionToPutSpecialChestOn].GetComponent<InstantiateObjectsScript>().floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.z - instantiationOffsetCameraZ));
            
            Vector3 crossPosWithYOffset = new Vector3(crossList[_crossIndex].transform.position.x, crossList[_crossIndex].transform.position.y + 2f, crossList[_crossIndex].transform.position.z);

            if (Physics.Raycast(crossPosWithYOffset, transform.TransformDirection(Vector3.down), out hit, 3f, combinedLayer)) //Si la croix est placée sur un élément de décor, une autre croix ou de la lave
            {
                i++; //Replace la croix à un autre endroit
            }
            else
            {
                break;
            }
        }
    }

    //Used for all the maps, except the start zone ones
    public void PlaceCamera(int _cameraIndex, int _crossIndex)
    {
        cameraList[_cameraIndex].GetComponent<Camera>().orthographicSize = cameraOrthographicSize;

        cameraList[_cameraIndex].transform.position = new Vector3(crossList[_crossIndex].transform.position.x,
                                                                 (crossList[_crossIndex].transform.position.y + instantiationOffsetCameraHeight),
                                                                  crossList[_crossIndex].transform.position.z);        
    }

    public IEnumerator TakeScreenshot(int _mapIndex, int _cameraIndex, int _crossIndex)
    {
        yield return new WaitForEndOfFrame();

        crossList[_crossIndex].GetComponent<SpriteRenderer>().enabled = true;
        cameraList[_cameraIndex].gameObject.SetActive(true);

        RenderTexture renderTexture = null;
        cameraList[_cameraIndex].GetComponent<Camera>().targetTexture = renderTexture;
        RenderTexture.active = renderTexture;
        cameraList[_cameraIndex].GetComponent<Camera>().Render();

        Texture2D texture = new Texture2D(cameraList[_cameraIndex].GetComponent<Camera>().pixelWidth, cameraList[_cameraIndex].GetComponent<Camera>().pixelHeight, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(0f, 0f, texture.width, texture.height), 0, 0);
        texture.Apply();

        Renderer rend = mapList[_mapIndex].GetComponent<Renderer>();
        rend.material = new Material(shader);
        rend.material.mainTexture = texture;
        rend.material.color = color;

        RenderTexture.active = null;
        cameraList[_cameraIndex].GetComponent<Camera>().targetTexture = null;

        cameraList[_cameraIndex].gameObject.SetActive(false);

        yield return null;
    }
}
