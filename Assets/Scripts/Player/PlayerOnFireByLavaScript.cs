using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerOnFireByLavaScript : NetworkBehaviour
{
    public ParticleSystem fireParticleEffect;
    public bool isOnFire = false;
    public GameObject panelHands;
    private PlayerSearchObjectScript playerSearchObjectScript;
    private PlayerMovementScript playerMovementScript;

    private void Start()
    {
        playerSearchObjectScript = GetComponent<PlayerSearchObjectScript>();
        playerMovementScript = GetComponent<PlayerMovementScript>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Lava") && isOnFire == false)
        {
            if (playerSearchObjectScript.isTakingSomething)
            {
                playerSearchObjectScript.LaunchTakenObject();
            }            

            StartCoroutine(avatarIsOnFire(5f, 10f));
        }
    }

    private IEnumerator avatarIsOnFire(float _time, float _timeNoHands)
    {
        //isOnFire = true;

        //playerSearchObjectScript.canTheAvatarDig = false;
        //playerSearchObjectScript.canTheAvatarTake = false;
        //panelHands.gameObject.SetActive(true);

        //playerMovementScript.isAvatarOnFire = true;
        //ParticleSystem newParticle = Instantiate(fireParticleEffect, transform.position, Quaternion.identity);
        //newParticle.transform.parent = transform;

        //yield return new WaitForSeconds(_time);

        //isOnFire = false;
        //playerMovementScript.isAvatarOnFire = false;

        //yield return new WaitForSeconds(_timeNoHands);
        //playerSearchObjectScript.canTheAvatarDig = true;
        //playerSearchObjectScript.canTheAvatarTake = true;
        //panelHands.gameObject.SetActive(false);

        yield return null;
    }
}
