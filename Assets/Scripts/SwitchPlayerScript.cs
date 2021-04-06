using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlayerScript : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();

    int index = 0;

    public static SwitchPlayerScript instance;

    private void Awake()
    {
        instance = this;
    }

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
            gO.transform.GetChild(0).gameObject.GetComponent<MovementScript>().canTheAvatarMove = false;
            gO.transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = false;
            gO.transform.GetChild(1).gameObject.SetActive(false);
        }

        players[activatedPlayerIndex].gameObject.transform.GetChild(0).gameObject.GetComponent<MovementScript>().canTheAvatarMove = true;
        players[activatedPlayerIndex].gameObject.transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = true;

        if(players[activatedPlayerIndex].gameObject.GetComponent<AvatarDeathScript>().isThePlayerDead == false)
        {
            players[activatedPlayerIndex].gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
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
