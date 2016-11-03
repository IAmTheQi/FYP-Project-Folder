using UnityEngine;
using System.Collections;

public class PlayerDetect : MonoBehaviour {

    GameObject gameController;
    TutorialHandler tutorialScript;

	// Use this for initialization
	void Start () {

        gameController = GameObject.Find("GameController");
        tutorialScript = gameController.GetComponent<TutorialHandler>();
	
	}

    void OnTriggerEnter(Collider collider)
    {
            if (collider.tag == "Player")
            {
                tutorialScript.Trigger();
                Destroy(this.gameObject);
            }
    }
}
