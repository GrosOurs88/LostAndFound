﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFireByLavaScript : MonoBehaviour
{
    public ParticleSystem fireParticleEffect;
    public bool isOnFire = false;
    public GameObject noHandsIcon;

    public float timeOnFire;
    public float timeNoHandAfterOnFire;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Lava") && isOnFire == false)
        {
            if (gameObject.transform.GetChild(0).GetComponent<SearchObjectScript>().isTakingSomething)
            {
                gameObject.transform.GetChild(0).GetComponent<SearchObjectScript>().LaunchTakenObject();
            }            

            StartCoroutine(avatarIsOnFire(timeOnFire, timeNoHandAfterOnFire));
        }
    }

    private IEnumerator avatarIsOnFire(float _time, float _timeNoHands)
    {
        isOnFire = true;

        transform.GetChild(0).GetComponent<SearchObjectScript>().canTheAvatarDig = false;
        transform.GetChild(0).GetComponent<SearchObjectScript>().canTheAvatarTake = false;
        noHandsIcon.gameObject.SetActive(true);

        transform.GetChild(0).GetComponent<MovementScript>().isAvatarOnFire = true;
        ParticleSystem newParticle = Instantiate(fireParticleEffect, transform.position, Quaternion.identity);
        newParticle.transform.parent = transform;

        yield return new WaitForSeconds(_time);

        isOnFire = false;
        transform.GetChild(0).GetComponent<MovementScript>().isAvatarOnFire = false;

        yield return new WaitForSeconds(_timeNoHands);
        transform.GetChild(0).GetComponent<SearchObjectScript>().canTheAvatarDig = true;
        transform.GetChild(0).GetComponent<SearchObjectScript>().canTheAvatarTake = true;
        noHandsIcon.gameObject.SetActive(false);

        yield return null;
    }
}
