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

    float timer;
    float timerLimit;

    Color promptColor;

    bool low;
    [FMODUnity.EventRef]
    public string ammoLowSound = "event:/Player VO/AmmoLow";

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

        timer = 0f;
        timerLimit = 10f;

        step0Prompt.SetActive(true);
        step1Prompt.SetActive(false);
        step2Prompt.SetActive(false);
        step3Prompt.SetActive(false);

        low = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (!playerScript.pauseGame)
        {
            if (playerScript.weapons[playerScript.currentWeaponIndex].currentAmmo < 5 && !low)
            {
                FMODUnity.RuntimeManager.PlayOneShot(ammoLowSound);
                low = true;
            }

            switch (currentState)
            {
                case TutorialState.step0:
                    if (triggered)
                    {
                        promptColor = step0Prompt.GetComponent<Image>().color;
                        promptColor.a += 0.02f;
                        FadeDelay();
                    }

                    if (transition)
                    {
                        promptColor = step0Prompt.GetComponent<Image>().color;
                        promptColor.a -= 0.02f;
                    }

                    promptColor.a = Mathf.Clamp(promptColor.a, 0.0f, 1.0f);
                    step0Prompt.GetComponent<Image>().color = promptColor;

                    break;

                case TutorialState.step1:
                    if (triggered)
                    {
                        promptColor = step1Prompt.GetComponent<Image>().color;
                        promptColor.a += 0.02f;
                        FadeDelay();
                    }

                    if (transition)
                    {
                        promptColor = step1Prompt.GetComponent<Image>().color;
                        promptColor.a -= 0.02f;
                    }

                    promptColor.a = Mathf.Clamp(promptColor.a, 0.0f, 1.0f);
                    step1Prompt.GetComponent<Image>().color = promptColor;
                    break;

                case TutorialState.step2:
                    if (triggered)
                    {
                        promptColor = step2Prompt.GetComponent<Image>().color;
                        promptColor.a += 0.02f;
                        FadeDelay();
                    }

                    if (transition)
                    {
                        promptColor = step2Prompt.GetComponent<Image>().color;
                        promptColor.a -= 0.02f;
                    }

                    promptColor.a = Mathf.Clamp(promptColor.a, 0.0f, 1.0f);
                    step2Prompt.GetComponent<Image>().color = promptColor;
                    break;

                case TutorialState.step3:
                    if (triggered)
                    {
                        promptColor = step3Prompt.GetComponent<Image>().color;
                        promptColor.a += 0.02f;
                        FadeDelay();
                    }

                    if (transition)
                    {
                        promptColor = step3Prompt.GetComponent<Image>().color;
                        promptColor.a -= 0.02f;
                    }

                    promptColor.a = Mathf.Clamp(promptColor.a, 0.0f, 1.0f);
                    step3Prompt.GetComponent<Image>().color = promptColor;
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DismissTutorial();
            }

            Debug.Log(currentState + "," + triggered + "," + transition);
        }
    }

    void StateTransition()
    {
        switch (currentState)
        {
            case TutorialState.step0:
                step0Prompt.SetActive(false);
                step1Prompt.SetActive(true);
                currentState = TutorialState.step1;
                break;

            case TutorialState.step1:
                step1Prompt.SetActive(false);
                step2Prompt.SetActive(true);
                currentState = TutorialState.step2;
                break;

            case TutorialState.step2:
                step2Prompt.SetActive(false);
                currentState = TutorialState.step3;
                break;
        }

        transition = false;
        triggered = true;
        IsPrompting();
        prompting = true;
    }

    void FadeDelay()
    {
        Debug.Log(currentState);
        if (timer < timerLimit)
        {
            timer += Time.deltaTime;
        }
        else if (timer >= timerLimit)
        {
            transition = true;
            prompting = false;
            IsPrompting();
            timer = 0;
        }
    }

    public void DismissTutorial()
    {
        transition = true;
        prompting = false;
        IsPrompting();
        timer = 0;
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
        StateTransition();
    }
}
