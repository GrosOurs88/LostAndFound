using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterScript : MonoBehaviour
{
    public Light globalLight;
    public List<Light> lights = new List<Light>();

    public List<GameObject> sectionsList = new List<GameObject>();

    void Start()
    {
        StartCoroutine(GLobalEnvironmentInstantiation());
    }

    public IEnumerator GLobalEnvironmentInstantiation()
    {
        globalLight.GetComponent<Light>().enabled = true;
        foreach(Light l in lights)
        {
            l.GetComponent<Light>().enabled = false;
        }

        for (int i = 0; i < sectionsList.Count; i++)
        {
            sectionsList[i].GetComponent<InstantiateObjectsScript>().SetupEnvironment();
        }

        yield return new WaitForSeconds(0.25f);

        globalLight.GetComponent<Light>().enabled = false;
        foreach (Light l in lights)
        {
            l.GetComponent<Light>().enabled = true;
        }

        yield return null;
    }
}
