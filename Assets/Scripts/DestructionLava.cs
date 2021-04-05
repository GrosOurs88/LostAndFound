using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionLava : MonoBehaviour
{
    public ParticleSystem smokeParticle;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Map") || collision.gameObject.CompareTag("Decor"))
        {
            if (smokeParticle != null)
            {
                Instantiate(smokeParticle, collision.GetContact(0).point, Quaternion.identity);
            }            
            Destroy(collision.gameObject);
        }
    }
}
