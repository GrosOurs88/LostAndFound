using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class AvatarDeathScript : MonoBehaviour
{
    public bool isThePlayerDead = false;
    public ParticleSystem turnAliveParticleSystem;

    public Material avatarMaterialDead;
    private Material avatarNormalMaterial;
    public GameObject glasses;
    public GameObject hat;
    public GameObject glassesDead;
    public GameObject hatDead;

    LayerMask layerPostProcessDeath;
    LayerMask layerPostProcess;

    public bool canTurnDead = false;

    private void Start()
    {
        layerPostProcessDeath = LayerMask.GetMask("PostProcessingDeath");
        layerPostProcess = LayerMask.GetMask("PostProcessing");

        avatarNormalMaterial = gameObject.GetComponent<Renderer>().material;
    }

    public void Update()
    {
        if (canTurnDead)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TurnAvatarDead();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                TurnAvatarAlive();
            }
        }
    }

    public void TurnAvatarDead()
    {
        isThePlayerDead = true;

        gameObject.transform.GetChild(0).GetComponent<SearchObjectScript>().canTheAvatarTake = false;
        gameObject.transform.GetChild(0).GetComponent<SearchObjectScript>().canTheAvatarDig = false;
        gameObject.transform.GetChild(0).GetComponent<PostProcessLayer>().volumeLayer = layerPostProcessDeath;

        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(0).GetComponent<MovementScript>().canTheAvatarRun = false;

        gameObject.GetComponent<Renderer>().material = avatarMaterialDead;
        gameObject.layer = LayerMask.NameToLayer("PlayerDead");
        gameObject.tag = "PlayerDead";
        
        glasses.SetActive(false);
        hat.SetActive(false);
        glassesDead.SetActive(true);
        hatDead.SetActive(true);
    }

    public void TurnAvatarAlive()
    {
        isThePlayerDead = false;

        Instantiate(turnAliveParticleSystem, transform.position, Quaternion.identity);

        gameObject.transform.GetChild(0).GetComponent<SearchObjectScript>().canTheAvatarTake = true;
        gameObject.transform.GetChild(0).GetComponent<SearchObjectScript>().canTheAvatarDig = true;
        gameObject.transform.GetChild(0).GetComponent<PostProcessLayer>().volumeLayer = layerPostProcess;

        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).GetComponent<MovementScript>().canTheAvatarRun = true;

        gameObject.GetComponent<Renderer>().material = avatarNormalMaterial;
        gameObject.layer = LayerMask.NameToLayer("Player");
        gameObject.tag = "Player";

        glasses.SetActive(true);
        hat.SetActive(true);
        glassesDead.SetActive(false);
        hatDead.SetActive(false);
    }
}
