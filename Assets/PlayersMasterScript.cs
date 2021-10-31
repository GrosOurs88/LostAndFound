using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayersMasterScript : MonoBehaviour
{
    public GameObject[] playersPrefabList;
    public Transform[] playersStartingPositionsList;

    public TextMeshProUGUI controllersDebug;

    void Start()
    {
        string[] names = Input.GetJoystickNames();

        print("There are : " + names.Length + " controllers connected");
        controllersDebug.text = names.ToString();

        for (int x = 0; x < names.Length; x++)
        {
            Instantiate(playersPrefabList[x], playersStartingPositionsList[x].position, playersStartingPositionsList[x].rotation, transform);          
        }
    }
}
