using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerMasterScript : MonoBehaviour
{
    public int gameTimeInSeconds;
    public TextMeshProUGUI timerText;
    private float remainingTime;

    private void Start()
    {
        remainingTime = gameTimeInSeconds;

        StartCoroutine(CountDown(gameTimeInSeconds));
    }

    private IEnumerator CountDown(int time)
    {
        while (time > 0)
        {
            time--;
            float minutes = Mathf.FloorToInt(time / 60) % 60;
            float seconds = Mathf.FloorToInt(time % 60);

            timerText.text = string.Format("{0:00} : {1:00}",minutes, seconds);

            yield return new WaitForSeconds(1);
        }

        foreach (GameObject section in MasterScript.instance.sectionsList)
        {
            section.GetComponent<SinkingIslandScript>().SinkIsland();
        }
    }

    //private void Update()
    //{
    //    remainingTime -= Time.deltaTime;


    //    if(remainingTime <= 0)
    //    {
    //        foreach(GameObject section in MasterScript.instance.sectionsList)
    //        {
    //            section.GetComponent<SinkingIslandScript>().SinkIsland();
    //        }
    //    }
    //}
}
