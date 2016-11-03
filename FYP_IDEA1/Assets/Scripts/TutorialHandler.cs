using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour {

    public enum TutorialState
    {
        step1,
        step2,
        step3,
        step4,
        step5,
        step6,
        step7,
        step8
    }

    public TutorialState currentState;

    GameObject playerObject;
    PlayerLogic playerScript;

    public GameObject step1Prompt;
    public GameObject step2Prompt;
    public GameObject step3Prompt;
    public GameObject step4Prompt;
    public GameObject step5Prompt;
    public GameObject step6Prompt;
    public GameObject step7Prompt;
    Color alphaColor;
    bool transition;
    bool triggered;

    bool activated;

    bool ckey;
    bool ctrlkey;
    bool exitWindow;
    public GameObject aritfactTarget;
    bool collected;
    public GameObject mutantTarget;

	// Use this for initialization
	void Start () {

        currentState = TutorialState.step1;

        playerObject = GameObject.Find("PlayerController");
        playerScript = playerObject.GetComponent<PlayerLogic>();

        alphaColor = step1Prompt.GetComponent<Image>().color;
        transition = false;
        triggered = true;

        activated = false;

        ckey = false;
        ctrlkey = false;
        exitWindow = false;
        collected = false;

        step1Prompt.SetActive(true);
        step2Prompt.SetActive(false);
        step3Prompt.SetActive(false);
        step4Prompt.SetActive(false);
        step5Prompt.SetActive(false);
        step6Prompt.SetActive(false);
        step7Prompt.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

        if (!playerScript.pauseGame)
        {

            switch (currentState)
            {
                case TutorialState.step1:

                    if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 && !activated)
                    {
                        activated = true;
                        step1Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;

                case TutorialState.step2:
                    if (triggered)
                    {
                        step2Prompt.SetActive(true);
                    }

                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        ckey = true;
                    }

                    if (Input.GetKeyDown(KeyCode.LeftControl))
                    {
                        ctrlkey = true;
                    }

                    if ((ckey || ctrlkey) && !activated)
                    {
                        activated = true;
                        step2Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;

                case TutorialState.step3:
                    if (triggered)
                    {
                        step3Prompt.SetActive(true);
                    }

                    if (Input.GetKeyDown(KeyCode.Space) && !activated)
                    {
                        activated = true;
                        step3Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;

                case TutorialState.step4:
                    if (triggered)
                    {
                        step4Prompt.SetActive(true);
                    }

                    if (Input.GetKeyDown(KeyCode.F) && !activated)
                    {
                        activated = true;
                        step4Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;

                case TutorialState.step5:
                    if (triggered)
                    {
                        step5Prompt.SetActive(true);
                    }

                    if (Input.GetMouseButtonDown(0) && !activated)
                    {
                        activated = true;
                        step5Prompt.SetActive(false);
                        StateTransition();
                    }
                    exitWindow = false;
                    break;

                case TutorialState.step6:
                    if (triggered)
                    {
                        step6Prompt.SetActive(true);
                    }

                    if (Input.GetKeyDown(KeyCode.F) && !activated)
                    {
                        activated = true;
                        step6Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;

                case TutorialState.step7:
                    if (triggered)
                    {
                        step7Prompt.SetActive(true);
                    }

                    if (Input.GetKeyDown(KeyCode.Space) && !activated)
                    {
                        activated = true;
                        step7Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;
            }
        }
    }

    void StateTransition()
    {
            switch (currentState)
            {
                case TutorialState.step1:
                    currentState = TutorialState.step2;
                    transition = false;
                    break;

                case TutorialState.step2:
                    currentState = TutorialState.step3;
                    transition = false;
                    break;

                case TutorialState.step3:
                    currentState = TutorialState.step4;
                    transition = false;
                    break;

                case TutorialState.step4:
                    currentState = TutorialState.step5;
                    transition = false;
                    break;

                case TutorialState.step5:
                    currentState = TutorialState.step6;
                    transition = false;
                    break;

                case TutorialState.step6:
                    currentState = TutorialState.step7;
                    transition = false;
                    break;

                case TutorialState.step7:
                    currentState = TutorialState.step8;
                    transition = false;
                    break;
            }

        triggered = false;
        activated = false;
    }

    public void Collect()
    {
        collected = true;
    }

    public void Trigger()
    {
        triggered = true;
    }
}
