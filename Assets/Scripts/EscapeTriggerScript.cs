using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EscapeTriggerScript : MonoBehaviour
{
    public string inputExit;

    public bool canEscape;
    public List<GameObject> playersInTheBoat = new List<GameObject>();
    public List<ParticleSystem> fXsBoatWater = new List<ParticleSystem>();
    public List<GameObject> players = new List<GameObject>();
    public Canvas escapeCanvas;
    public Canvas scoreCanvas;
    public TextMeshProUGUI commonChestNumber;
    private int commonChestNumberAmount;
    public TextMeshProUGUI bigChestNumber;
    private int bigChestNumberAmount;
    public TextMeshProUGUI giantChestNumber;
    private int giantChestNumberAmount;
    public TextMeshProUGUI rareChestNumber;
    private int rareChestNumberAmount;
    public TextMeshProUGUI specialChestNumber;
    private int specialChestNumberAmount;
    private bool canBoatMove;
    public GameObject boat;
    public float boatSpeed;
    public Camera escapeCamera;    

    public static EscapeTriggerScript instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {     
        escapeCamera.enabled = false;
        escapeCanvas.enabled = false;
        scoreCanvas.enabled = false;
    }

    private void Update()
    {
        if(Input.GetButtonDown(inputExit) && canEscape == true)
        {
            foreach (GameObject gO in players)
            {
                gO.transform.GetChild(1).gameObject.SetActive(false);
                gO.transform.GetChild(0).GetComponent<MovementScript>().canTheAvatarMove = false;
            }

            foreach(ParticleSystem pS in fXsBoatWater)
            {
                pS.Play();
            }

            CountChestsType();
            DisplayChestsType();

            escapeCamera.enabled = true;
            escapeCanvas.enabled = false;
            scoreCanvas.enabled = true;
            canBoatMove = true;
        }        
    }

    private void CountChestsType()
    {
        foreach (GameObject gO in BoatCargoScript.instance.chestsInTheBoat)
        {
            switch (gO.GetComponent<ChestScript>().type)
            {
                case ChestScript.Type.Common:
                    commonChestNumberAmount++;
                    break;
                case ChestScript.Type.Big:
                    bigChestNumberAmount++;
                    break;
                case ChestScript.Type.Giant:
                    giantChestNumberAmount++;
                    break;
                case ChestScript.Type.Rare:
                    rareChestNumberAmount++;
                    break;
                case ChestScript.Type.Special:
                    specialChestNumberAmount++;
                    break;
                default:
                    Debug.Log("NOTHING");
                    break;
            }
        }
    }

    private void DisplayChestsType()
    {
        commonChestNumber.text = "x" + commonChestNumberAmount.ToString();
        bigChestNumber.text = "x" + bigChestNumberAmount.ToString();
        giantChestNumber.text = "x" + giantChestNumberAmount.ToString();
        rareChestNumber.text = "x" + rareChestNumberAmount.ToString();
        specialChestNumber.text = "x" + specialChestNumberAmount.ToString();
    }

    private void FixedUpdate()
    {
        if (canBoatMove)
        {
            foreach(GameObject gO in playersInTheBoat)
            {
                gO.transform.parent = boat.transform;
            }

            foreach (GameObject gO in BoatCargoScript.instance.chestsInTheBoat)
            {
                gO.transform.parent = boat.transform;
            }

            boat.transform.Translate(Vector3.left * boatSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playersInTheBoat.Add(other.gameObject);
            players.Add(other.gameObject);

            CheckPlayersCount();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInTheBoat.Remove(other.gameObject);
            players.Remove(other.gameObject);

            CheckPlayersCount();
        }
    }

    public void CheckPlayersCount()
    {
        if (playersInTheBoat.Count == Input.GetJoystickNames().Length)
        {
            canEscape = true;
            escapeCanvas.enabled = true;
        }

        else if (players.Count < Input.GetJoystickNames().Length)
        {
            canEscape = false;
            escapeCanvas.enabled = false;
        }
    }
}
