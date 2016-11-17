using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour {

    public enum TutorialState
    {
        step0,
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

    public GameObject step0Prompt;
    public GameObject step1Prompt;
    public GameObject step2Prompt;
    public GameObject step3Prompt;
    public GameObject step4Prompt;
    public GameObject step5Prompt;
    public GameObject step6Prompt;
    public GameObject step7Prompt;
    public GameObject step8Prompt;

    bool transition;
    bool triggered;

    bool ckey;
    bool ctrlkey;
    bool collected;

	// Use this for initialization
	void Start () {

        currentState = TutorialState.step0;

        playerObject = GameObject.Find("PlayerController");
        playerScript = playerObject.GetComponent<PlayerLogic>();
        
        transition = false;
        triggered = true;

        ckey = false;
        ctrlkey = false;
        collected = false;

        step0Prompt.SetActive(true);
        step1Prompt.SetActive(false);
        step2Prompt.SetActive(false);
        step3Prompt.SetActive(false);
        step4Prompt.SetActive(false);
        step5Prompt.SetActive(false);
        step6Prompt.SetActive(false);
        step7Prompt.SetActive(false);
        step8Prompt.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

        Debug.LogFormat("triggered:{0}      transition:{1}", triggered, transition);

        if (!playerScript.pauseGame)
        {

            switch (currentState)
            {
                case TutorialState.step0:
                    if (transition)
                    {
                        step0Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;
                case TutorialState.step1:

                    if (triggered)
                    {
                        step1Prompt.SetActive(true);
                    }

                    if (transition)
                    {
                        step1Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;

                case TutorialState.step2:
                    if (triggered)
                    {
                        step2Prompt.SetActive(true);
                    }

                    if (transition)
                    {
                        step2Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;

                case TutorialState.step3:
                    if (triggered)
                    {
                        step3Prompt.SetActive(true);
                    }

                    if (transition)
                    {
                        step3Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;

                case TutorialState.step4:
                    if (triggered)
                    {
                        step4Prompt.SetActive(true);
                    }

                    if (transition)
                    {
                        step4Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;

                case TutorialState.step5:
                    if (triggered)
                    {
                        step5Prompt.SetActive(true);
                    }

                    if (transition)
                    {
                        step5Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;

                case TutorialState.step6:
                    if (triggered)
                    {
                        step6Prompt.SetActive(true);
                    }

                    if (transition)
                    {
                        step6Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;

                case TutorialState.step7:
                    if (triggered)
                    {
                        step7Prompt.SetActive(true);
                    }

                    if (transition)
                    {
                        step7Prompt.SetActive(false);
                        StateTransition();
                    }
                    break;
            }
        }

        if (triggered)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Transition();
            }
        }
    }

    void StateTransition()
    {
            switch (currentState)
            {
                case TutorialState.step0:
                    currentState = TutorialState.step1;
                    break;
                case TutorialState.step1:
                    currentState = TutorialState.step2;
                    break;

                case TutorialState.step2:
                    currentState = TutorialState.step3;
                    break;

                case TutorialState.step3:
                    currentState = TutorialState.step4;
                    break;

                case TutorialState.step4:
                    currentState = TutorialState.step5;
                    break;

                case TutorialState.step5:
                    currentState = TutorialState.step6;
                    break;

                case TutorialState.step6:
                    currentState = TutorialState.step7;
                    break;

                case TutorialState.step7:
                    currentState = TutorialState.step8;
                    break;
            }

        transition = false;
        triggered = false;
    }

    public void Collect()
    {
        collected = true;
    }

    public void Trigger()
    {
        triggered = true;
        playerScript.TutorialPause();
    }

    public bool TutorialTriggered()
    {
        return triggered;
    }

    public void Transition()
    {
        transition = true;
        playerScript.TutorialPause();
    }
}
