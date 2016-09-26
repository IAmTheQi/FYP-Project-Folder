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
        if (gameObject.tag == "Exit")
        {
            if (collider.gameObject == playerObject)
            {
                tutorialScript.ExitWindow();
                Destroy(this);
            }
        }
        else if (gameObject.tag == "Trigger")
        {
            if (collider.gameObject == playerObject)
            {
                tutorialScript.Trigger();
                Destroy(this);
            }
        }
    }
}
