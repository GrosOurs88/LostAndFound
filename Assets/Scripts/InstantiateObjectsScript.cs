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

    public List<GameObject> objectsToInstantiate = new List<GameObject>();
    private GameObject nextObjectToInstantiate;
    private Vector3 nextObjectToInstanciatePosition;
    public GameObject ObjectsInstiatedFolder;
    private int x;

    public float maxScore;

    public static InstantiateObjectsScript instance;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InstantiateEnvironment();
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

            //Instantiate the object
            Instantiate(nextObjectToInstantiate, nextObjectToInstanciatePosition, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f), ObjectsInstiatedFolder.transform);
        }
    }    
}
