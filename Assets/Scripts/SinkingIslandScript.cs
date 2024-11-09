using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingIslandScript : MonoBehaviour
{
    public Transform sinkingPosition;
    public float sinkingSpeed;
    public bool isSinking = false;

    public void Update()
    {
        if(isSinking == true)
        {
            if((sinkingPosition.position - gameObject.transform.position).magnitude > 0.1f) //0.1f by default to avoid trying to reach perfect target position forever
            {
                gameObject.transform.position += (sinkingPosition.position - gameObject.transform.position).normalized * sinkingSpeed * Time.deltaTime;
            }
            else
            {
                isSinking = false;
            }
        }
    }

    public void SinkIsland()
    {
        isSinking = true;
    }
}
