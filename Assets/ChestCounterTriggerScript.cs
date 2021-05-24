using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestCounterTriggerScript : MonoBehaviour
{
    public bool displayChestsNumber;

    public TextMeshProUGUI commonChestsText;
    public int commonChestsNumber;
    public TextMeshProUGUI bigChestsText;
    public int bigChestsNumber;
    public TextMeshProUGUI giantChestsText;
    public int giantChestsNumber;
    public TextMeshProUGUI rareChestsText;
    public int rareChestsNumber;
    public TextMeshProUGUI specialChestsText;
    public int specialChestsNumber;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            switch (other.GetComponent<ChestScript>().type)
            {
                case ChestScript.Type.Common:
                    commonChestsNumber++;
                    commonChestsText.text = commonChestsNumber.ToString();
                    break;
                case ChestScript.Type.Big:
                    bigChestsNumber++;
                    bigChestsText.text = bigChestsNumber.ToString();
                    break;
                case ChestScript.Type.Giant:
                    giantChestsNumber++;
                    giantChestsText.text = giantChestsNumber.ToString();
                    break;
                case ChestScript.Type.Rare:
                    rareChestsNumber++;
                    rareChestsText.text = rareChestsNumber.ToString();
                    break;
                case ChestScript.Type.Special:
                    specialChestsNumber++;
                    specialChestsText.text = specialChestsNumber.ToString();
                    break;
                default:
                    Debug.Log("UNKNOWN CHEST TYPE");
                    break;
            }               
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            switch (other.GetComponent<ChestScript>().type)
            {
                case ChestScript.Type.Common:
                    commonChestsNumber--;
                    commonChestsText.text = commonChestsNumber.ToString();
                    break;
                case ChestScript.Type.Big:
                    bigChestsNumber--;
                    bigChestsText.text = bigChestsNumber.ToString();
                    break;
                case ChestScript.Type.Giant:
                    giantChestsNumber--;
                    giantChestsText.text = giantChestsNumber.ToString();
                    break;
                case ChestScript.Type.Rare:
                    rareChestsNumber--;
                    rareChestsText.text = rareChestsNumber.ToString();
                    break;
                case ChestScript.Type.Special:
                    specialChestsNumber--;
                    specialChestsText.text = specialChestsNumber.ToString();
                    break;
                default:
                    Debug.Log("UNKNOWN CHEST TYPE");
                    break;
            }
        }
    }    
}
