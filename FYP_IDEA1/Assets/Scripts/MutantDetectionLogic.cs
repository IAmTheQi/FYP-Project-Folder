using UnityEngine;
using System.Collections;

public class MutantDetectionLogic : MonoBehaviour {

    GameObject parentMutant;

    GameObject playerObject;
    PlayerLogic playerScript;

    bool playerInSight;
    float fieldOfViewAngle;
    SphereCollider col;

    float angle;

	// Use this for initialization
	void Start () {
        playerObject = GameObject.Find("PlayerController");
        playerScript = playerObject.GetComponent<PlayerLogic>();

        parentMutant = transform.parent.gameObject;

        playerInSight = false;
        fieldOfViewAngle = 70.0f;

        col = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(playerInSight);
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (gameObject.CompareTag("Boundary"))
        {
            if (collider.gameObject == playerObject)
            {
                parentMutant.SendMessage("PlayerTouch");
            }
        }
        else
        {
            if (playerScript.IsWalking() && collider.gameObject == playerObject)
            {
                parentMutant.SendMessage("AlertMutant");
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            Vector3 direction = other.transform.position - transform.position;
            angle = Vector3.Angle(direction, transform.forward);
            
            if (angle < fieldOfViewAngle * 0.5f && !playerInSight)
            {
                RaycastHit hit;

                if (Physics.Raycast(other.transform.position, -direction.normalized, out hit, col.radius))
                {
                    if (hit.collider.tag == "Mutant")
                    {
                        parentMutant.SendMessage("AlertMutant");
                    }
                    //Debug.LogFormat("bool:{0}       angle:{1}       hit:{2}", playerInSight, angle, hit.collider.tag);
                }
            }
            else if (angle > fieldOfViewAngle * 0.5f && playerInSight)
            {
                parentMutant.SendMessage("RecordLastSeen", other.transform);
                parentMutant.SendMessage("LosePlayer");
            }

        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (!gameObject.CompareTag("Boundary"))
        {
            if (collider.gameObject == playerObject)
            {
                parentMutant.SendMessage("RecordLastSeen", collider.transform);
                parentMutant.SendMessage("LosePlayer");
            }
        }

        if (gameObject.CompareTag("Boundary"))
        {
            if (collider.gameObject == playerObject)
            {
                parentMutant.SendMessage("PlayerLeave");
            }
        }
    }
}
