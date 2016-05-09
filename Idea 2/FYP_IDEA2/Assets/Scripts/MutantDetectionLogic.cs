using UnityEngine;
using System.Collections;

public class MutantDetectionLogic : MonoBehaviour {

    string targetTag;

    GameObject parentMutant;
    MutantLogic parentScript;

    GameObject playerObject;
    PlayerLogic playerScript;

    // Use this for initialization
    void Start () {

        playerObject = GameObject.Find("PlayerController");
        playerScript = playerObject.GetComponent<PlayerLogic>();

        targetTag = "Player";

        parentMutant = transform.parent.gameObject;
        parentScript = parentMutant.GetComponent<MutantLogic>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == targetTag && playerScript.IsWalking())
        {
            parentScript.AlertMutant();
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == targetTag && playerScript.IsWalking())
        {
            parentScript.AlertMutant();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == targetTag)
        {
            parentScript.CalmMutant();
        }
    }
}
