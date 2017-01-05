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
    }

    public TutorialState currentState;

    GameObject playerObject;
    PlayerLogic playerScript;

    public GameObject step0Prompt;
    public GameObject step1Prompt;
    public GameObject step2Prompt;
    public GameObject step3Prompt;

    bool transition;
    bool triggered;
    bool prompting;

    bool ckey;
    bool ctrlkey;
    bool collected;

    Color promptColor;

	// Use this for initialization
	void Start () {

        currentState = TutorialState.step0;

        playerObject = GameObject.Find("PlayerController");
        playerScript = playerObject.GetComponent<PlayerLogic>();
        
        transition = false;
        triggered = true;
        prompting = true;

        ckey = false;
        ctrlkey = false;
        collected = false;

        step0Prompt.SetActive(true);
        step1Prompt.SetActive(false);
        step2Prompt.SetActive(false);
        step3Prompt.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

        if (!playerScript.pauseGame)
        {

            switch (currentState)
            {
                case TutorialState.step0:
                    if (triggered)
                    {
                        promptColor = step0Prompt.GetComponent<Image>().color;
                        promptColor.a += 0.02f;
                        StartCoroutine(FadeDelay());
                    }

                    if (transition)
                    {
                        promptColor = step0Prompt.GetComponent<Image>().color;
                        promptColor.a -= 0.02f;

                        if (promptColor.a == 0)
                        {
                            step0Prompt.SetActive(false);
                            StateTransition();
                        }
                    }

                    promptColor.a = Mathf.Clamp(promptColor.a, 0.0f, 1.0f);
                    step0Prompt.GetComponent<Image>().color = promptColor;

                    break;

                case TutorialState.step1:
                    if (triggered)
                    {
                        promptColor = step1Prompt.GetComponent<Image>().color;
                        promptColor.a += 0.02f;
                        StartCoroutine(FadeDelay());
                    }

                    if (transition)
                    {
                        promptColor = step1Prompt.GetComponent<Image>().color;
                        promptColor.a -= 0.02f;

                        if (promptColor.a == 0)
                        {
                            step1Prompt.SetActive(false);
                            StateTransition();
                        }
                    }

                    promptColor.a = Mathf.Clamp(promptColor.a, 0.0f, 1.0f);
                    step1Prompt.GetComponent<Image>().color = promptColor;
                    break;

                case TutorialState.step2:
                    if (triggered)
                    {
                        promptColor = step2Prompt.GetComponent<Image>().color;
                        promptColor.a += 0.02f;
                        StartCoroutine(FadeDelay());
                    }

                    if (transition)
                    {
                        promptColor = step2Prompt.GetComponent<Image>().color;
                        promptColor.a -= 0.02f;

                        if (promptColor.a == 0)
                        {
                            step2Prompt.SetActive(false);
                            StateTransition();
                        }
                    }

                    promptColor.a = Mathf.Clamp(promptColor.a, 0.0f, 1.0f);
                    step2Prompt.GetComponent<Image>().color = promptColor;
                    break;

                case TutorialState.step3:
                    if (triggered)
                    {
                        promptColor = step3Prompt.GetComponent<Image>().color;
                        promptColor.a += 0.02f;
                        StartCoroutine(FadeDelay());
                    }

                    if (transition)
                    {
                        promptColor = step3Prompt.GetComponent<Image>().color;
                        promptColor.a -= 0.02f;

                        if (promptColor.a == 0)
                        {
                            step3Prompt.SetActive(false);
                            StateTransition();
                        }
                    }

                    promptColor.a = Mathf.Clamp(promptColor.a, 0.0f, 1.0f);
                    step3Prompt.GetComponent<Image>().color = promptColor;
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DismissTutorial();
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
        }

        transition = false;
        triggered = false;
    }

    IEnumerator FadeDelay()
    {
        yield return new WaitForSeconds(10);
        transition = true;
        prompting = false;
        IsPrompting();
        StopCoroutine(FadeDelay());
    }

    public void DismissTutorial()
    {
        transition = true;
        prompting = false;
        IsPrompting();
        StopCoroutine(FadeDelay());
    }

    public void Collect()
    {
        collected = true;
    }

    public void IsPrompting()
    {
        playerScript.PromptTutorial();
    }

    public void Trigger()
    {
        triggered = true;
    }
}
