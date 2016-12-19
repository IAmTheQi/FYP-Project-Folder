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
	
        if (parentMutant.GetComponent<MutantSimple>().IsDead())
        {
            Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == playerObject)
        {
            parentMutant.GetComponent<MutantSimple>().PlayerEnter();
        }
    }
}
