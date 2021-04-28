using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestCounterTriggerScript : MonoBehaviour
{
    // public int numberOfChests;
    // public List<GameObject> chests = new List<GameObject>();
    public bool displayChestsNumber;

    public TextMeshProUGUI commonChestsText;
    private int commonChestsNumber;
    public TextMeshProUGUI bigChestsText;
    private int bigChestsNumber;
    public TextMeshProUGUI giantChestsText;
    private int giantChestsNumber;
    public TextMeshProUGUI rareChestsText;
    private int rareChestsNumber;
    public TextMeshProUGUI specialChestsText;
    private int specialChestsNumber;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            //numberOfChests++;
            //chests.Add(other.gameObject);

            switch (other.GetComponent<ChestScript>().type)
            {
                case ChestScript.Type.Common:
                    UpdateNumber(commonChestsText, commonChestsNumber, true);
                    break;
                case ChestScript.Type.Big:
                    UpdateNumber(bigChestsText, bigChestsNumber, true);
                    break;
                case ChestScript.Type.Giant:
                    UpdateNumber(giantChestsText, giantChestsNumber, true);
                    break;
                case ChestScript.Type.Rare:
                    UpdateNumber(rareChestsText, rareChestsNumber, true);
                    break;
                case ChestScript.Type.Special:
                    UpdateNumber(specialChestsText, specialChestsNumber, true);
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
            //numberOfChests --;
            //chests.Remove(other.gameObject);

            switch (other.GetComponent<ChestScript>().type)
            {
                case ChestScript.Type.Common:
                    UpdateNumber(commonChestsText, commonChestsNumber, false);
                    break;
                case ChestScript.Type.Big:
                    UpdateNumber(bigChestsText, bigChestsNumber, false);
                    break;
                case ChestScript.Type.Giant:
                    UpdateNumber(giantChestsText, giantChestsNumber, false);
                    break;
                case ChestScript.Type.Rare:
                    UpdateNumber(rareChestsText, rareChestsNumber, false);
                    break;
                case ChestScript.Type.Special:
                    UpdateNumber(specialChestsText, specialChestsNumber, false);
                    break;
                default:
                    Debug.Log("UNKNOWN CHEST TYPE");
                    break;
            }
        }
    }

    private void UpdateNumber(TextMeshProUGUI _text, int _number, bool _numberIncrease)
    {
        if (_numberIncrease)
        { _number++; }
        else
        { _number--; }

        _text.text = _number.ToString();
    }
}
