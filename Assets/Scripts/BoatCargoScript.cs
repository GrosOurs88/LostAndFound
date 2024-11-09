using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCargoScript : MonoBehaviour
{
    public int numberOfChestsInTheBoat;
    public List<GameObject> chestsInTheBoat = new List<GameObject>();

    public static BoatCargoScript instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            numberOfChestsInTheBoat++;
            chestsInTheBoat.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            numberOfChestsInTheBoat--;
            chestsInTheBoat.Remove(other.gameObject);
        }
    }
}
