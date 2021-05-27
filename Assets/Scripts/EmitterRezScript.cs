using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterRezScript : MonoBehaviour
{
    public GameObject antiAvatarCollision;
    public ParticleSystem resurectionParticleSystem;

    public bool isResurrectionReady = false;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Chest") && isResurrectionReady == false)
        {
            isResurrectionReady = true;
            Destroy(other.gameObject);
            resurectionParticleSystem.Play();
        }

        if (other.gameObject.CompareTag("PlayerDead") && isResurrectionReady == true)
        {
            antiAvatarCollision.SetActive(false);
            resurectionParticleSystem.Stop();
            other.gameObject.GetComponent<PlayerDeathScript>().TurnAvatarAlive();
        }
    }
}
