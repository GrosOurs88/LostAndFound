using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManagerScript : MonoBehaviour
{
    public int numberOfChestToOpenDoor;

    [HideInInspector]
    public int numberOfChestPlaced;
    [HideInInspector]
    public float doorCompletionLevel;
}
