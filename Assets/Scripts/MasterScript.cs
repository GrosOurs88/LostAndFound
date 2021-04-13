using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterScript : MonoBehaviour
{
    public GameObject sectionStart;
    public List<GameObject> sectionsList = new List<GameObject>();

    void Awake()
    {
        StartCoroutine(GlobalEnvironmentInstantiation());
    }

    public IEnumerator GlobalEnvironmentInstantiation()
    {
        for (int i = 0; i < sectionsList.Count; i++)
        {
            sectionsList[i].GetComponent<InstantiateObjectsScript>().SetupEnvironment();
        }

        sectionStart.GetComponent<InstantiateObjectsScript>().StartSectionSetupEnvironment(sectionsList);

        yield return null;
    }
}
