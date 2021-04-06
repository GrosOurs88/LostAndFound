using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public enum Type { Common, Big, Giant, Rare, Special };
    public Type type;

    public bool isTaken = false;

    public bool canBeTaken = true;

    public GameObject emitterLinked;
}
