using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersMasterScript : MonoBehaviour
{
    public List<string> controllersNamesList = new List<string>();

    public GameObject[] playersPrefabList;
    public Transform[] playersStartingPositionsList;

    void Update()
    {
        string[] names = Input.GetJoystickNames();

        for (int x = 0; x < names.Length; x++)
        {
            if(!controllersNamesList.Contains(names[x]))
            {
                controllersNamesList.Add(names[x]);
                Instantiate(playersPrefabList[names.Length - 1], playersStartingPositionsList[names.Length - 1].position, playersStartingPositionsList[names.Length - 1].rotation, transform);
            }            
        }
    }
}
