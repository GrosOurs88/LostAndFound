using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlayerWithCameraScript : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();

    int index = 0;

    void Start()
    {
        ActivateSpecificPlayer(index);
    }

    public void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            SwitchNextPlayer();
        }

        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            SwitchPreviousPlayer();
        }
    }

    public void ActivateSpecificPlayer(int activatedPlayerIndex)
    {
        foreach (GameObject gO in players)
        {
            gO.GetComponent<PlayerMovementScript>().canTheAvatarMove = false;
        }

        players[activatedPlayerIndex].gameObject.GetComponent<PlayerMovementScript>().canTheAvatarMove = true;
    }

    public void SwitchPreviousPlayer()
    {
        if(index > 0)
        {
            index--;
        }
        else
        {
            index = players.Count-1;
        }

        ActivateSpecificPlayer(index);
    }

    public void SwitchNextPlayer()
    {
        if (index < players.Count-1)
        {
            index++;
        }
        else
        {
            index = 0;
        }

        ActivateSpecificPlayer(index);
    }
}
