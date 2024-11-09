using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossChestTypeScript : MonoBehaviour
{
    public enum Type { Common, Big, Giant, Rare, Special };

    public Type type;
    public GameObject mapLinkedToCross; //public for test, is filled by the InstantiateObjectsScript

    public void DestroyCrossAndMap()
    {
        if(mapLinkedToCross.GetComponent<MapScript>().isCurrentlyTaken)
        {
            mapLinkedToCross.GetComponent<MapScript>().playerWhoOwnsTheMap.GetComponent<SearchObjectScript>().isTakingSomething = false;
            mapLinkedToCross.GetComponent<MapScript>().playerWhoOwnsTheMap.GetComponent<SearchObjectScript>().canTheAvatarTake = true;
        }

        Destroy(mapLinkedToCross);

        Destroy(gameObject);
    }
}
