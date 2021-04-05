using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedByLavaScript : MonoBehaviour
{
    public ParticleSystem smokeParticle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            Instantiate(smokeParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
