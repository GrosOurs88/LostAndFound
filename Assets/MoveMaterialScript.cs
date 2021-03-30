using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMaterialScript : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    public bool moveOnXAxis;
    public bool moveOnYAxis;

    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;

        if(moveOnXAxis && moveOnYAxis)
        {
            rend.material.SetTextureOffset("_MainTex", new Vector2(offset, offset));
        }
        else if(moveOnXAxis)
        {
            rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
        else if(moveOnYAxis)
        {
            rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
        }      
    }
}
