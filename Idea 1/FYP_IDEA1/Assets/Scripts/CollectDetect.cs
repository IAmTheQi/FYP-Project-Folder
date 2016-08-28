using UnityEngine;
using System.Collections;

public class CollectDetect : MonoBehaviour
{

    GameObject gameController;
    TutorialHandler tutorialScript;

    // Use this for initialization
    void Start()
    {

        gameController = GameObject.Find("GameController");
        tutorialScript = gameController.GetComponent<TutorialHandler>();

    }

    void OnDisable()
    {
        tutorialScript.Collect();
    }
}
