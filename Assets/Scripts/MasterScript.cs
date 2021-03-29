using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterScript : MonoBehaviour
{
    public Light globalLight;

    public List<GameObject> sectionsList = new List<GameObject>();

    void Start()
    {
        StartCoroutine(GLobalEnvironmentInstantiation());
    }

    public IEnumerator GLobalEnvironmentInstantiation()
    {
        globalLight.GetComponent<Light>().enabled = true;

        for (int i = 0; i < sectionsList.Count; i++)
        {
            sectionsList[i].GetComponent<InstantiateObjectsScript>().SetupEnvironment();
        }

        yield return new WaitForSeconds(1f);
        globalLight.GetComponent<Light>().enabled = false;

        yield return null;
    }
}
