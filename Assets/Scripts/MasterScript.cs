using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterScript : MonoBehaviour
{
    public List<GameObject> sectionsList = new List<GameObject>();

    void Start()
    {
        StartCoroutine(GlobalEnvironmentInstantiation());
    }

    public IEnumerator GlobalEnvironmentInstantiation()
    {
        for (int i = 0; i < sectionsList.Count; i++)
        {
            sectionsList[i].GetComponent<InstantiateObjectsScript>().SetupEnvironment();
        }

        yield return null;
    }
}
