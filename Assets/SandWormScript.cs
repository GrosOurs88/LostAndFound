using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormScript : MonoBehaviour
{
    public GameObject sandWormMovePointsFolder;
    public float offsetToGoToNextPoint;
    public float speed;
    [HideInInspector]
    public  List<GameObject> sandWormMovePoints;
    private int actualPointToMoveOn = 0;

    void Start()
    {
        SetMovePointsList();
        LookAtNextPoint();
    }

    void FixedUpdate()
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, sandWormMovePoints[actualPointToMoveOn].transform.position, step);

        if (Vector3.Distance(transform.position, sandWormMovePoints[actualPointToMoveOn].transform.position) < offsetToGoToNextPoint)
        {
            MoveToNextPoint();
            LookAtNextPoint();
        }
    }

    private void SetMovePointsList()
    {
        foreach (Transform tr in sandWormMovePointsFolder.transform)
        {
            sandWormMovePoints.Add(tr.gameObject);
        }
        transform.LookAt(sandWormMovePoints[actualPointToMoveOn].transform.position);
    }

    private void MoveToNextPoint()
    {
        if(actualPointToMoveOn < sandWormMovePoints.Count-1)
        {
            actualPointToMoveOn++;
        }

        else
        {
            actualPointToMoveOn = 0;
        }
    }

    private void LookAtNextPoint()
    {
        transform.LookAt(sandWormMovePoints[actualPointToMoveOn].transform.position);
    }
}
