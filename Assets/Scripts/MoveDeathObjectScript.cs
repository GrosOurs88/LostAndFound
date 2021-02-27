using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDeathObjectScript : MonoBehaviour
{
    public GameObject deathObject;
    public float speed;
    public float materialScrollSpeed;
    public Vector3 movement;
    private Material mat;
    private Vector3 deathObjectStartPosition;


    private void Start()
    {
        deathObjectStartPosition = deathObject.transform.position;
        mat = deathObject.GetComponent<Renderer>().material;
    }

    void Update()
    {
        deathObject.transform.position += movement * speed * Time.deltaTime;

        float offset = Time.time * materialScrollSpeed;
        mat.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }

    public void ResetPosition()
    {
        deathObject.transform.position = deathObjectStartPosition;
    }
}
