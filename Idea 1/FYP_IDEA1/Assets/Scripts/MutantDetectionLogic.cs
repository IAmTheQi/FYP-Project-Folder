using UnityEngine;
using System.Collections;

public class MutantDetectionLogic : MonoBehaviour {

    GameObject parentMutant;

    GameObject playerObject;
    PlayerLogic playerScript;

    bool playerInSight;
    float fieldOfViewAngle;
    SphereCollider col;

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
	
	}

    void OnTriggerEnter(Collider collider)
    {
        /*if (gameObject.CompareTag("Boundary"))
        {
            if (collider.gameObject == playerObject)
            {
                parentMutant.SendMessage("AlertMutant");
            }
        }
        else
        {
            if (playerScript.IsWalking() && collider.gameObject == playerObject)
            {
                parentMutant.SendMessage("AlertMutant");
            }
        }*/
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInSight = false;
            
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            
            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;
                
                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                {
                    if (hit.collider.gameObject == playerObject)
                    {
                        playerInSight = true;
                    }
                }
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
    }
}
