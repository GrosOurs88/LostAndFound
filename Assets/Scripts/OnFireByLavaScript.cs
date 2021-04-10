using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFireByLavaScript : MonoBehaviour
{
    public bool isOnFire = false;
    public float timePlayerIsOnFire;
    public float timePlayerCannotTakeObjectsAfterBeingOnFire;
    public GameObject panelHands;
    public ParticleSystem fireParticleEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Lava") && isOnFire == false)
        {
            if (gameObject.transform.GetChild(0).GetComponent<SearchObjectScript>().isTakingSomething)
            {
                gameObject.transform.GetChild(0).GetComponent<SearchObjectScript>().LaunchTakenObject();
            }            

            StartCoroutine(avatarIsOnFire(timePlayerIsOnFire, timePlayerCannotTakeObjectsAfterBeingOnFire));
        }
    }

    private IEnumerator avatarIsOnFire(float _time, float _timeNoHands)
    {
        isOnFire = true;

        transform.GetChild(0).GetComponent<SearchObjectScript>().canTheAvatarDig = false;
        transform.GetChild(0).GetComponent<SearchObjectScript>().canTheAvatarTake = false;
        panelHands.gameObject.SetActive(true);

        transform.GetChild(0).GetComponent<MovementScript>().isAvatarOnFire = true;
        ParticleSystem newParticle = Instantiate(fireParticleEffect, transform.position, Quaternion.identity);
        newParticle.transform.parent = transform;

        yield return new WaitForSeconds(_time);

        isOnFire = false;
        transform.GetChild(0).GetComponent<MovementScript>().isAvatarOnFire = false;

        yield return new WaitForSeconds(_timeNoHands);
        transform.GetChild(0).GetComponent<SearchObjectScript>().canTheAvatarDig = true;
        transform.GetChild(0).GetComponent<SearchObjectScript>().canTheAvatarTake = true;
        panelHands.gameObject.SetActive(false);

        yield return null;
    }
}
