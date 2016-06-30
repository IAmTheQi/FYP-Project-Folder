using UnityEngine;
using System.Collections;

public class SurfaceDetect : MonoBehaviour {

    GameObject playerObject;
    PlayerLogic playerScript;

	// Use this for initialization
	void Start () {

        playerObject = GameObject.Find("PlayerController");
        playerScript = playerObject.GetComponent<PlayerLogic>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        playerScript.ChangeSurface(collider.tag);

        Debug.Log(collider.tag);
    }
}
