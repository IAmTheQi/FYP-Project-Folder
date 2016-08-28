using UnityEngine;
using System.Collections;

public class PlayerDetect : MonoBehaviour {

    GameObject gameController;
    TutorialHandler tutorialScript;
    GameObject playerObject;

	// Use this for initialization
	void Start () {

        gameController = GameObject.Find("GameController");
        tutorialScript = gameController.GetComponent<TutorialHandler>();
        playerObject = GameObject.Find("PlayerController");
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == playerObject)
        {
            tutorialScript.ExitWindow();
            Destroy(this);
        }
    }
}
