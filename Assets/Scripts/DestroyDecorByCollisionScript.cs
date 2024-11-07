using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDecorByCollisionScript : MonoBehaviour
{
    public float timer = 2f;

    private void Update()
    {
        if(Time.time == timer)
        {
            MakesObjectKinematicThenRemoveScript();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Decor"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void MakesObjectKinematicThenRemoveScript()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        Destroy(gameObject.GetComponent<DestroyDecorByCollisionScript>());
    }
}
