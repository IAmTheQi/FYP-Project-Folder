using UnityEngine;
using System.Collections;

public class MutantDetectionLogic : MonoBehaviour {

    string targetTag;

    GameObject parentMutant;
    MutantLogic parentScript;

	// Use this for initialization
	void Start () {

        targetTag = "Player";

        parentMutant = transform.parent.gameObject;
        parentScript = parentMutant.GetComponent<MutantLogic>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == targetTag)
        {
            parentScript.AlertMutant();
        }
    }
}
