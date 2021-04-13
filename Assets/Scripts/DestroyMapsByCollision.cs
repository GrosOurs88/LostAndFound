using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMapsByCollision : MonoBehaviour
{
    public ParticleSystem mapBurnedParticle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            Instantiate(mapBurnedParticle, collision.GetContact(0).point, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}
