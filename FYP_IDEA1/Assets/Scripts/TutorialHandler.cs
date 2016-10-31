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
	
	}
	
	// Update is called once per frame
	void Update () {

        if (!playerScript.pauseGame)
        {

            if (!transition && triggered)
            {
                alphaColor.a += 0.01f;
            }
            else if (transition)
            {
                alphaColor.a -= 0.05f;
            }
            alphaColor.a = Mathf.Clamp(alphaColor.a, 0.0f, 0.7f);

            switch (currentState)
            {
                case TutorialState.step1:
                    step1Prompt.GetComponent<Image>().color = alphaColor;
                    if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 && !activated)
                    {
                        StartCoroutine(StateTransition());
                        activated = true;
                    }
                    break;

                case TutorialState.step2:
                    step2Prompt.GetComponent<Image>().color = alphaColor;

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
                        StartCoroutine(StateTransition());
                        activated = true;
                    }
                    break;

                case TutorialState.step3:
                    step3Prompt.GetComponent<Image>().color = alphaColor;

                    if (exitWindow && !activated)
                    {
                        StartCoroutine(StateTransition());
                        activated = true;
                    }
                    break;

                case TutorialState.step4:
                    step4Prompt.GetComponent<Image>().color = alphaColor;

                    if (collected && !activated)
                    {
                        StartCoroutine(StateTransition());
                        activated = true;
                    }
                    break;

                case TutorialState.step5:
                    step5Prompt.GetComponent<Image>().color = alphaColor;

                    if (!mutantTarget.activeInHierarchy && !activated)
                    {
                        StartCoroutine(StateTransition());
                        activated = true;
                    }
                    exitWindow = false;
                    break;

                case TutorialState.step6:
                    step6Prompt.GetComponent<Image>().color = alphaColor;

                    if (Input.GetKeyDown(KeyCode.F) && !activated)
                    {
                        StartCoroutine(StateTransition());
                        activated = true;
                    }
                    break;

                case TutorialState.step7:
                    step7Prompt.GetComponent<Image>().color = alphaColor;

                    if (exitWindow && !activated)
                    {
                        StartCoroutine(StateTransition());
                        activated = true;
                    }
                    break;
            }
        }
    }

    IEnumerator StateTransition()
    {
        transition = true;
        yield return new WaitForSeconds(2.0f);

        if (transition)
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
        }

        triggered = false;
        activated = false;
        StopCoroutine(StateTransition());
    }

    public void ExitWindow()
    {
        exitWindow = true;
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
