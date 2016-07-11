using UnityEngine;
using System.Collections;

public class MutantDetectionLogic : MonoBehaviour {

    GameObject parentMutant;

    GameObject playerObject;
    PlayerLogic playerScript;

	// Use this for initialization
	void Start () {
        playerObject = GameObject.Find("PlayerController");
        playerScript = playerObject.GetComponent<PlayerLogic>();

        parentMutant = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (gameObject.CompareTag("Boundary"))
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
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (!gameObject.CompareTag("Boundary"))
        {
            if (playerScript.IsWalking() && collider.gameObject == playerObject)
            {
                parentMutant.SendMessage("AlertMutant");
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
