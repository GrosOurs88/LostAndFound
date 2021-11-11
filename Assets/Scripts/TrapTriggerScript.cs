using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTriggerScript : MonoBehaviour
{
    public GameObject trap;
    public List<ParticleSystem> poisonThrowers;
    public ParticleSystem PoisonousGaz;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            trap.GetComponent<MoveDeathObjectScript>().isFalling = true;

            foreach(ParticleSystem pS in poisonThrowers)
            {
                pS.Play();
            }

            PoisonousGaz.Play();

            gameObject.SetActive(false);
        }
    }
}
