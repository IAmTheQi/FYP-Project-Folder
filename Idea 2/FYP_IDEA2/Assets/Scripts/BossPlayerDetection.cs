using UnityEngine;
using System.Collections;

public class BossPlayerDetection : MonoBehaviour {

    GameObject parentObject;
    BossOne parentScript;

    // Use this for initialization
    void Start () {
        parentObject = transform.parent.gameObject;
        parentScript = parentObject.GetComponent<BossOne>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            parentScript.Alert();
        }
    }
}
