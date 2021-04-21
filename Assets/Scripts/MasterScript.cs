using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterScript : MonoBehaviour
{
    public GameObject sectionStart;
    public List<GameObject> sectionsList = new List<GameObject>();

    public static MasterScript instance;

    void Awake()
    {
        instance = this;

        StartCoroutine(GlobalEnvironmentInstantiation());
    }

    public IEnumerator GlobalEnvironmentInstantiation()
    {
        for (int i = 0; i < sectionsList.Count; i++)
        {
            sectionsList[i].GetComponent<InstantiateObjectsScript>().SetupEnvironment(); //Instanciate chests in all the sections
        }

       // sectionStart.GetComponent<InstantiateObjectsStartScript>().SetupEnvironment(sectionsList); //Instanciate special chests in random sections

        yield return null;
    }
}
