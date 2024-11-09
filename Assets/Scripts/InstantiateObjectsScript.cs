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
    public GameObject mapsInstiatedFolder;
    public enum ShapeType { Parallelepiped, Circle };
    public ShapeType shape;
    public float borderOffsetInstantiation;
    private float circleShapeRadius; //Only used for circle shape
    private float yOffsetForInstantiationRaycast = 30f; //Modify only if instanciation bugs

    [Header("Decor")]
    public int numberOfObjectsToInstantiate;
    public List<GameObject> decorObjectsToInstantiateList = new List<GameObject>();
    public int decorPlacementMaxIterationNumber = 5;
    private GameObject nextObjectToInstantiate;
    private Vector3 nextObjectToInstanciatePosition;
    private float yOffsetForDecorInstanciation = 0.25f; //Modify only if instanciation bugs
    private float decorInstanciationRaycastLength = 50f; //Modify only if instanciation bugs
    private int x;

    [Header("Crosses")]
    public int crossPlacementMaxIterationNumber = 5;
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
    private float crossPlacementYOffset = 0.06f; //Modify only if instanciation bugs
    private List<GameObject> crossesList = new List<GameObject>();

    [Header("Maps")]
    public Shader shader;
    public Color color;
    public List<GameObject> mapObjectsToInstantiateList = new List<GameObject>();
    private int numberOfTotalChestsToInstanciate;
    private List<GameObject> mapsList = new List<GameObject>();

    [Header("Cameras")]
    public GameObject screenshotCameraPrefab;
    private GameObject screenshotCamera;
    private Camera screenshotCameraComponent;
    public float cameraOrthographicSize = 5;

    [Header("Other")]
    //private LayerMask layerCheckForObstacles;
    private LayerMask layerFloor;

    public void SetupEnvironment()
    {
        SetupGlobalValues();

        InstantiateEnvironment();

        PlaceMaps(numberOfTotalChestsToInstanciate);

        PlaceCrosses(numberOfCrossChestCommon, crossChestCommon);
        PlaceCrosses(numberOfCrossChestBig, crossChestBig);
        PlaceCrosses(numberOfCrossChestGiant, crossChestGiant);
        PlaceCrosses(numberOfCrossChestRare, crossChestRare);

        GetAllMaps();
        GetAllCrosses();

        InstanciateScreenshotCamera();

        for (int i = 0; i < numberOfTotalChestsToInstanciate; i++)
        {
            StartCoroutine(TakeScreenshot(crossesList[i], mapsList[i])); //Take a screenshot
        }
    }

    public void InstantiateEnvironment()
    {
        //Loop for all the objects that have to be placed
        switch (shape)
        {
            case ShapeType.Parallelepiped:
                for (int i = 0; i < numberOfObjectsToInstantiate; i++)
                {
                    //Choose an object to instantiate randomly
                    nextObjectToInstantiate = GetRandomElement(decorObjectsToInstantiateList);

                    //Choose a random position to spawn the object
                    nextObjectToInstanciatePosition = GetRandomParallelepipedRaycastPosition(yOffsetForInstantiationRaycast);

                    //Check for floor to spawn the object
                    FireRaycastAndInstantiateElement(layerFloor, nextObjectToInstantiate, nextObjectToInstanciatePosition, transform.TransformDirection(Vector3.down), decorInstanciationRaycastLength, yOffsetForDecorInstanciation, decorInstiatedFolder);
                }
                break;

            case ShapeType.Circle:
                for (int i = 0; i < numberOfObjectsToInstantiate; i++)
                {
                    //Choose an object to instantiate randomly
                    nextObjectToInstantiate = GetRandomElement(decorObjectsToInstantiateList);

                    //Choose a random position to spawn the object
                    nextObjectToInstanciatePosition = GetRandomCircleRaycastPosition(yOffsetForInstantiationRaycast);

                    //Check for floor to spawn the object
                    FireRaycastAndInstantiateElement(layerFloor, nextObjectToInstantiate, nextObjectToInstanciatePosition, transform.TransformDirection(Vector3.down), decorInstanciationRaycastLength, yOffsetForDecorInstanciation, decorInstiatedFolder);
                }
                break;
        }
    }

    public void PlaceMaps(int _numberOfMaps) //*****ADD PRINT WITH LAYER ENCOUNTERED
    {
        switch (shape)
        {
            case ShapeType.Parallelepiped:
                {
                    for (int i = 0; i < _numberOfMaps; i++)
                    {
                        int x = 0;
                        while (x < crossPlacementMaxIterationNumber)
                        {
                            //Choose an object to instantiate randomly
                            nextObjectToInstantiate = GetRandomElement(mapObjectsToInstantiateList);

                            //Choose a random position to spawn the object
                            nextObjectToInstanciatePosition = GetRandomParallelepipedRaycastPosition(yOffsetForInstantiationRaycast);

                            //Check for floor with a raycast
                            if(CheckForObstaclesWithRaycast(layerFloor, nextObjectToInstanciatePosition, transform.TransformDirection(Vector3.down), decorInstanciationRaycastLength) == true)
                            {
                                //spawn the object on the floor
                                FireRaycastAndInstantiateElement(layerFloor, nextObjectToInstantiate, nextObjectToInstanciatePosition, transform.TransformDirection(Vector3.down), decorInstanciationRaycastLength, yOffsetForDecorInstanciation, mapsInstiatedFolder);
                                break;
                            }

                            else 
                            {
                                x++; //Replace the map to other location
                            }
                        }
                    }
                }
                break;

            case ShapeType.Circle:
                {
                    for (int i = 0; i < _numberOfMaps; i++)
                    {
                        int x = 0;
                        while (x < crossPlacementMaxIterationNumber)
                        {
                            //Choose an object to instantiate randomly
                            nextObjectToInstantiate = GetRandomElement(mapObjectsToInstantiateList);

                            //Choose a random position to spawn the object
                            nextObjectToInstanciatePosition = GetRandomCircleRaycastPosition(yOffsetForInstantiationRaycast);

                            //Check for floor with a raycast
                            if (CheckForObstaclesWithRaycast(layerFloor, nextObjectToInstanciatePosition, transform.TransformDirection(Vector3.down), decorInstanciationRaycastLength) == true)
                            {
                                //Check for floor to spawn the object
                                FireRaycastAndInstantiateElement(layerFloor, nextObjectToInstantiate, nextObjectToInstanciatePosition, transform.TransformDirection(Vector3.down), decorInstanciationRaycastLength, yOffsetForDecorInstanciation, mapsInstiatedFolder);
                                break;
                            }

                            else
                            {
                                x++; //Replace the map to other location
                            }
                        }
                    }
                }
                break;
        }
    }

    public void PlaceCrosses(int _numberOfChest, GameObject _crossChest) //*****ADD PRINT WITH LAYER ENCOUNTERED
    {
        switch (shape)
        {
            case ShapeType.Parallelepiped:
                {
                    for (int i = 0; i < _numberOfChest; i++)
                    {
                        int x = 0;
                        while (x < crossPlacementMaxIterationNumber)
                        {
                            //Choose a random position to spawn the object
                            nextObjectToInstanciatePosition = GetRandomParallelepipedRaycastPosition(yOffsetForInstantiationRaycast);

                            //Check for obstacles with a raycast
                            if (CheckForObstaclesWithRaycast(layerFloor, nextObjectToInstanciatePosition, transform.TransformDirection(Vector3.down), decorInstanciationRaycastLength) == true)
                            {
                                //Check for floor to spawn the object
                                FireRaycastAndInstantiateCross(layerFloor, _crossChest, nextObjectToInstanciatePosition, transform.TransformDirection(Vector3.down), decorInstanciationRaycastLength, crossPlacementYOffset, crossesInstiatedFolder);
                                break;                               
                            }

                            else
                            {                                                               
                                x++; //Replace the map to other location
                            }
                        }
                    }
                }
                break;

            case ShapeType.Circle:
                {
                    for (int i = 0; i < _numberOfChest; i++)
                    {
                        int x = 0;
                        while (x < crossPlacementMaxIterationNumber)
                        {
                            //Choose a random position to spawn the object
                            nextObjectToInstanciatePosition = GetRandomCircleRaycastPosition(yOffsetForInstantiationRaycast);

                            //Check for obstacles with a raycast
                            if (CheckForObstaclesWithRaycast(layerFloor, nextObjectToInstanciatePosition, transform.TransformDirection(Vector3.down), decorInstanciationRaycastLength) == true)
                            {
                                //Check for floor to spawn the object
                                FireRaycastAndInstantiateCross(layerFloor, _crossChest, nextObjectToInstanciatePosition, transform.TransformDirection(Vector3.down), decorInstanciationRaycastLength, crossPlacementYOffset, crossesInstiatedFolder);
                                break;
                            }

                            else
                            {
                                x++; //Replace the map to other location
                            }
                        }
                    }
                }
                break;
        }
    }

    private void SetupGlobalValues()
    {
        circleShapeRadius = floorToInstantiateObjectsInto.transform.localScale.x / 2;
        numberOfTotalChestsToInstanciate = numberOfCrossChestCommon + numberOfCrossChestBig + numberOfCrossChestGiant + numberOfCrossChestRare + numberOfCrossChestSpecial;
       // layerCheckForObstacles = LayerMask.GetMask("Lava") | LayerMask.GetMask("Default") | LayerMask.GetMask("Cross");
        layerFloor = LayerMask.GetMask("Floor");
    }

    private GameObject GetRandomElement(List<GameObject> _listGo)
    {
        x = Random.Range(0, _listGo.Count - 1);
        nextObjectToInstantiate = _listGo[x];
        return nextObjectToInstantiate;
    }

    private Vector3 GetRandomParallelepipedRaycastPosition(float _yOffsetForInstantiationRaycast)
    {
        return nextObjectToInstanciatePosition = new Vector3(Random.Range(floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.x + borderOffsetInstantiation, floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.x - borderOffsetInstantiation),
                                                             floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.y + _yOffsetForInstantiationRaycast,
                                                             Random.Range(floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.z + borderOffsetInstantiation, floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.z - borderOffsetInstantiation));
    }

    private Vector3 GetRandomCircleRaycastPosition(float _yOffsetForInstantiationRaycast)
    {
        Vector2 circlePos = Random.insideUnitCircle * (circleShapeRadius - borderOffsetInstantiation);

        return nextObjectToInstanciatePosition = new Vector3(floorToInstantiateObjectsInto.transform.position.x + circlePos.x,
                                                             floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.y + _yOffsetForInstantiationRaycast,
                                                             floorToInstantiateObjectsInto.transform.position.z + circlePos.y); //y because circlePos is Vector2  
    }

    public void InstanciateScreenshotCamera()
    {
        screenshotCamera = Instantiate(screenshotCameraPrefab, Vector3.zero, Quaternion.Euler(90.0f, Random.Range(0.0f, 360.0f), 0.0f), screenshotCamerasInstiatedFolder.transform);
        screenshotCameraComponent = screenshotCamera.GetComponent<Camera>();
    }

    public void GetAllCrosses()
    {
        foreach(Transform tr in crossesInstiatedFolder.transform)
        {
            crossesList.Add(tr.gameObject);
        }
    }

    public void GetAllMaps()
    {
        foreach (Transform tr in mapsInstiatedFolder.transform)
        {
            mapsList.Add(tr.GetChild(0).gameObject);
        }
    }

    private bool CheckForObstaclesWithRaycast(LayerMask _layer, Vector3 _nextObjectToInstanciatePosition, Vector3 _transformDirection, float _raycastLength)
    {
        RaycastHit hit;
        if (Physics.Raycast(_nextObjectToInstanciatePosition, _transformDirection, out hit, _raycastLength, _layer))
        {
            return true; //true : there is an obstacle
        }
        else
        {
            return false; //false : no obstacle
        }
    }

    private void FireRaycastAndInstantiateElement(LayerMask _layer, GameObject _nextObjectToInstantiate, Vector3 _nextObjectToInstanciatePosition, Vector3 _transformDirection, float _raycastLength, float _yNegativeOffset, GameObject _parent)
    {
        RaycastHit hit;
        if (Physics.Raycast(_nextObjectToInstanciatePosition, _transformDirection, out hit, _raycastLength, _layer))
        {
            Vector3 decorInstanciationNewPos = new Vector3(hit.point.x, hit.point.y - _yNegativeOffset, hit.point.z);
            Instantiate(_nextObjectToInstantiate, decorInstanciationNewPos, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f), _parent.transform); //Instantiate the object and rotate it randomly
        }       
    }

    private void FireRaycastAndInstantiateCross(LayerMask _layer, GameObject _nextObjectToInstantiate, Vector3 _nextObjectToInstanciatePosition, Vector3 _transformDirection, float _raycastLength, float _yPositiveOffset, GameObject _parent)
    {
        RaycastHit hit;
        if (Physics.Raycast(_nextObjectToInstanciatePosition, _transformDirection, out hit, _raycastLength, _layer))
        {
            Vector3 decorInstanciationNewPos = new Vector3(hit.point.x, hit.point.y + _yPositiveOffset, hit.point.z);
            Instantiate(_nextObjectToInstantiate, decorInstanciationNewPos, Quaternion.LookRotation(-hit.normal), _parent.transform); //Instantiate the object and rotate it randomly
        }
    }

    public IEnumerator TakeScreenshot(GameObject _cross, GameObject _map)
    {
        yield return new WaitForEndOfFrame();

        screenshotCamera.gameObject.SetActive(true);

        screenshotCamera.transform.position = _cross.transform.position + Vector3.up * yOffsetForInstantiationRaycast;
        screenshotCamera.transform.rotation = Quaternion.Euler(90.0f, Random.Range(0.0f, 360.0f), 0.0f);
        screenshotCameraComponent.orthographicSize = cameraOrthographicSize;

        RenderTexture renderTexture = null;
        screenshotCameraComponent.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;
        screenshotCameraComponent.Render();

        Texture2D texture = new Texture2D(screenshotCameraComponent.pixelWidth, screenshotCameraComponent.pixelHeight, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(0f, 0f, texture.width, texture.height), 0, 0);
        texture.Apply();

        Renderer rend = _map.GetComponent<Renderer>(); //_map ?? OR _camera ?
        rend.material = new Material(shader);
        rend.material.mainTexture = texture;
        rend.material.color = color;

        RenderTexture.active = null;
        screenshotCameraComponent.targetTexture = null;

        screenshotCamera.gameObject.SetActive(false);

        yield return null;
    }
}

