using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormScript : MonoBehaviour
{
    public GameObject sandWormMovePointsFolder;
    public float offsetToGoToNextPoint;
    public float speed;
    public float runSpeed;
    [HideInInspector]
    public bool isHuntingPlayer = false;
    [HideInInspector]
    public  List<GameObject> sandWormMovePoints;
    private int actualPointToMoveOn = 0;
    public float playersbumpForce;
    public float chestsbumpForce;
    [HideInInspector]
    public List<GameObject> huntedElementsInTheDetectionZone = new List<GameObject>();

    void Start()
    {
        SetMovePointsList();
        LookAtNextPoint();
    }

    void FixedUpdate()
    {
        if(isHuntingPlayer == false)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, sandWormMovePoints[actualPointToMoveOn].transform.position, step);

            if (Vector3.Distance(transform.position, sandWormMovePoints[actualPointToMoveOn].transform.position) < offsetToGoToNextPoint)
            {
                MoveToNextPoint();
                LookAtNextPoint();
            }
        }

        else if (isHuntingPlayer == true)
        {
            float step = runSpeed * Time.deltaTime; // calculate distance to move
            Vector3 pointToReach = new Vector3(huntedElementsInTheDetectionZone[0].transform.position.x, transform.position.y, huntedElementsInTheDetectionZone[0].transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, pointToReach, step);
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            print("Player touched");
            Vector3 bumpVector = (collision.transform.position - transform.position);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(bumpVector * playersbumpForce);

            if(collision.gameObject.transform.GetChild(0).GetComponent<SearchObjectScript>().isTakingSomething)
            {
                collision.gameObject.transform.GetChild(0).GetComponent<SearchObjectScript>().LaunchTakenObject();
            }
        }

        else if (collision.gameObject.CompareTag("Chest"))
        {
            print("Chest touched");
            Vector3 bumpVector = (collision.transform.position - transform.position);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(bumpVector * chestsbumpForce);
        }
    }
}
