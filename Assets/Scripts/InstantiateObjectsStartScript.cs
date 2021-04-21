using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InstantiateObjectsScript;

public class InstantiateObjectsStartScript : MonoBehaviour
{
    [Header("InstantiatedElementParents")]
    public GameObject floorToInstantiateObjectsInto;
    public GameObject decorInstiatedFolder;
    public GameObject screenshotCamerasInstiatedFolder;
    public GameObject crossesInstiatedFolder;

    private float circleShapeRadius; //Only used for circle shape
    private float yOffsetForInstantiationRaycast = 30f; //Modify only if instanciation bugs

    [Header("Decor")]
    public int decorPlacementMaxIterationNumber = 5;
    private GameObject nextObjectToInstantiate;
    private Vector3 nextObjectToInstanciatePosition;
    private float yOffsetForDecorInstanciation = 0.25f; //Modify only if instanciation bugs
    private float decorInstanciationRaycastLength = 50f; //Modify only if instanciation bugs
    private int x;

    [Header("Crosses")]
    public int crossPlacementMaxIterationNumber = 5;
    public GameObject crossChestSpecial;
    public int numberOfCrossChestSpecial;
    private float chestInstanciationRaycastLength = 35f; //Modify only if instanciation bugs
    private float crossPlacementYOffset = 0.06f; //Modify only if instanciation bugs
    private Vector3 lastRaycastPosition;

    [Header("Maps")]
    public Shader shader;
    public Color color;
    public List<GameObject> mapObjectsToInstantiate = new List<GameObject>();
    public List<GameObject> maps = new List<GameObject>();
    private int newMapObjectToInstanciateIndex;
    private GameObject newMapObjectToInstanciate;
    private List<GameObject> mapsList = new List<GameObject>();
    private GameObject lastmapInstantiated;

    [Header("Cameras")]
    public GameObject screenshotCamera;
    public float cameraOrthographicSize = 5;
    private int randomSectionToPutSpecialChestOn; //Used only for SectionStart chests instantiation

    public void SetupEnvironment(List<GameObject> _sectionsList)
    {
        PlaceCrossesOnRandomSection(MasterScript.instance.sectionsList); //Place special chests

        for (int i = 0; i < numberOfCrossChestSpecial; i++)
        {
            TakeScreenshot(screenshotCamera, maps[i]); //Take a screenshot
        }
    }

    //Used for the start zone maps
    public void PlaceCrossesOnRandomSection(List<GameObject> _sectionsList)
    {
        for (int i = 0; i < numberOfCrossChestSpecial; i++)
        {
            //Take random section from sectionsList
            randomSectionToPutSpecialChestOn = Random.Range(0, _sectionsList.Count);
            GameObject randomSection = _sectionsList[randomSectionToPutSpecialChestOn];
            GameObject floorToInstantiate = _sectionsList[randomSectionToPutSpecialChestOn].GetComponent<InstantiateObjectsScript>().floorToInstantiateObjectsInto;

            switch (randomSection.GetComponent<InstantiateObjectsScript>().shape)
            {
                case ShapeType.Parallelepiped:
                    {

                        int x = 0;
                        while (x < crossPlacementMaxIterationNumber)
                        {
                            RaycastHit hit;
                            LayerMask combinedLayer = LayerMask.GetMask("Lava") | LayerMask.GetMask("Default") | LayerMask.GetMask("Cross");

                            if (Physics.Raycast(GetRandomSectionRandomParallelepipedRaycastPosition(floorToInstantiate), transform.TransformDirection(Vector3.down), out hit, chestInstanciationRaycastLength, combinedLayer)) //Si la croix est placée sur de la lave, un élément de décor ou une autre croix
                            {
                                x++; //Replace la croix à un autre endroit
                            }
                            else
                            {
                                InstantiateElement(crossChestSpecial, hit.point, Quaternion.identity, crossesInstiatedFolder); //Instanciate cross

                                InstanciateAndPlaceCamera(lastRaycastPosition); //Instanciate camera

                                break;
                            }
                        }
                    }
                    break;

                case ShapeType.Circle:
                    {

                        int x = 0;
                        while (x < crossPlacementMaxIterationNumber)
                        {
                            RaycastHit hit;
                            LayerMask combinedLayer = LayerMask.GetMask("Lava") | LayerMask.GetMask("Default") | LayerMask.GetMask("Cross");

                            if (Physics.Raycast(GetRandomSectionRandomCircleRaycastPosition(floorToInstantiate), transform.TransformDirection(Vector3.down), out hit, chestInstanciationRaycastLength, combinedLayer)) //Si la croix est placée sur de la lave, un élément de décor ou une autre croix
                            {
                                x++; //Replace la croix à un autre endroit
                            }
                            else
                            {
                                InstantiateElement(crossChestSpecial, hit.point, Quaternion.identity, crossesInstiatedFolder); //Instanciate cross

                                InstanciateAndPlaceCamera(lastRaycastPosition); //Instanciate camera

                                break;
                            }
                        }

                    }
                    break;
            }     
        }
    }

    //Used for all the maps, except the start zone ones
    public void InstanciateAndPlaceCamera(Vector3 _vec)
    {
        GameObject newScreenshotCamera = Instantiate(screenshotCamera, _vec, Quaternion.Euler(0f, 90f, 0f), screenshotCamerasInstiatedFolder.transform);

        newScreenshotCamera.GetComponent<Camera>().orthographicSize = cameraOrthographicSize;
    }

    private void SetupGlobalValues()
    {
        circleShapeRadius = floorToInstantiateObjectsInto.transform.localScale.x / 2;
    }

    private GameObject GetRandomElement(List<GameObject> _listGo)
    {
        x = Random.Range(0, _listGo.Count - 1);
        nextObjectToInstantiate = _listGo[x];
        return nextObjectToInstantiate;
    }

    private Vector3 GetRandomParallelepipedRaycastPosition()
    {
        nextObjectToInstanciatePosition = new Vector3(Random.Range(floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.x, floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.x),
                                                     floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.y + yOffsetForInstantiationRaycast,
                                                     Random.Range(floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.min.z, floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.z));

        lastRaycastPosition = nextObjectToInstanciatePosition;

        return nextObjectToInstanciatePosition;
    }

    private Vector3 GetRandomCircleRaycastPosition()
    {
        Vector2 circlePos = Random.insideUnitCircle * circleShapeRadius;

        nextObjectToInstanciatePosition = new Vector3(floorToInstantiateObjectsInto.transform.position.x + circlePos.x,
                                                      floorToInstantiateObjectsInto.GetComponent<Collider>().bounds.max.y + yOffsetForInstantiationRaycast,
                                                      floorToInstantiateObjectsInto.transform.position.z + circlePos.y); //y because circlePos is Vector2  

        lastRaycastPosition = nextObjectToInstanciatePosition;

        return nextObjectToInstanciatePosition;
    }

    private Vector3 GetRandomSectionRandomParallelepipedRaycastPosition(GameObject _sectionFloor)
    {
        nextObjectToInstanciatePosition = new Vector3(Random.Range(_sectionFloor.GetComponent<Collider>().bounds.min.x, _sectionFloor.GetComponent<Collider>().bounds.max.x),
                                                     _sectionFloor.GetComponent<Collider>().bounds.max.y + yOffsetForInstantiationRaycast,
                                                     Random.Range(_sectionFloor.GetComponent<Collider>().bounds.min.z, _sectionFloor.GetComponent<Collider>().bounds.max.z));

        lastRaycastPosition = nextObjectToInstanciatePosition;

        return nextObjectToInstanciatePosition;
    }

    private Vector3 GetRandomSectionRandomCircleRaycastPosition(GameObject _sectionFloor)
    {
        Vector2 circlePos = Random.insideUnitCircle * circleShapeRadius;

        nextObjectToInstanciatePosition = new Vector3(_sectionFloor.transform.position.x + circlePos.x,
                                                      _sectionFloor.GetComponent<Collider>().bounds.max.y + yOffsetForInstantiationRaycast,
                                                      _sectionFloor.transform.position.z + circlePos.y); //y because circlePos is Vector2  

        lastRaycastPosition = nextObjectToInstanciatePosition;

        return nextObjectToInstanciatePosition;
    }

    private void InstantiateElement(GameObject _gO, Vector3 pos, Quaternion _quat, GameObject _parent)
    {
        GameObject nextCross = Instantiate(_gO, pos, _quat, _parent.transform);
    }

    private void FireRaycastAndInstantiateElement(string _layerName, GameObject _nextObjectToInstantiate, Vector3 _nextObjectToInstanciatePosition, Vector3 _transformDirection, float _raycastLength, GameObject _parent)
    {
        RaycastHit hit;
        LayerMask layerToCheck = LayerMask.GetMask(_layerName);

        if (Physics.Raycast(_nextObjectToInstanciatePosition, _transformDirection, out hit, _raycastLength, layerToCheck))
        {
            Vector3 decorInstanciationNewPos = new Vector3(hit.point.x, hit.point.y - yOffsetForDecorInstanciation, hit.point.z);
            Instantiate(_nextObjectToInstantiate, decorInstanciationNewPos, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f), _parent.transform); //Instantiate the object and rotate it randomly
        }
    }

    public IEnumerator TakeScreenshot(GameObject _camera, GameObject _map)
    {
        yield return new WaitForEndOfFrame();

        RenderTexture renderTexture = null;
        _camera.GetComponent<Camera>().targetTexture = renderTexture;
        RenderTexture.active = renderTexture;
        _camera.GetComponent<Camera>().Render();

        Texture2D texture = new Texture2D(_camera.GetComponent<Camera>().pixelWidth, _camera.GetComponent<Camera>().pixelHeight, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(0f, 0f, texture.width, texture.height), 0, 0);
        texture.Apply();

        Renderer rend = _map.GetComponent<Renderer>(); //_map ?? OR _camera ?
        rend.material = new Material(shader);
        rend.material.mainTexture = texture;
        rend.material.color = color;

        RenderTexture.active = null;
        _camera.GetComponent<Camera>().targetTexture = null;

        _camera.gameObject.SetActive(false);

        yield return null;
    }
}