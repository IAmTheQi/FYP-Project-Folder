using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LightningScript : MonoBehaviour {

    public GameObject buttons;
    public GameObject options;
    public GameObject credits;

    public GameObject creditsText;
    Vector3 originalPos;

    bool optionsMenu;
    bool creditsMenu;

    float lastTime;

    [FMODUnity.EventRef]
    public string clickFeedback = "event:/Click";

    // Use this for initialization
    void Start () {

        lastTime = 0f;

        buttons = GameObject.Find("Buttons");
        options = GameObject.Find("Options");
        credits = GameObject.Find("Credits");

        creditsText = GameObject.Find("CreditsText");
        originalPos = creditsText.transform.position;

        optionsMenu = false;
        creditsMenu = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (!optionsMenu && !creditsMenu)
        {
            buttons.SetActive(true);
            options.SetActive(false);
            credits.SetActive(false);
        }
        else if (optionsMenu)
        {
            buttons.SetActive(false);
            options.SetActive(true);
            credits.SetActive(false);
        }
        else if (creditsMenu)
        {
            buttons.SetActive(false);
            options.SetActive(false);
            credits.SetActive(true);

            creditsText.transform.Translate(Vector3.up * Time.deltaTime * 20);
        }

        Debug.LogFormat("credits:{0}      options:{1}", creditsMenu, optionsMenu);
	
	}

    public void PlayGame()
    {
        FMODUnity.RuntimeManager.PlayOneShot(clickFeedback);
        SceneManager.LoadScene("LevelPreloader");
    }

    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("LevelPreloader");

        while (!async.isDone)
        {
            yield return null;
        }
    }

    public void QuitGame()
    {
        FMODUnity.RuntimeManager.PlayOneShot(clickFeedback);
        Application.Quit();
    }

    public void OpenOptions()
    {
        FMODUnity.RuntimeManager.PlayOneShot(clickFeedback);
        optionsMenu = true;
    }

    public void OpenCredits()
    {
        FMODUnity.RuntimeManager.PlayOneShot(clickFeedback);
        creditsMenu = true;
    }

    public void ReturnFrom(string target)
    {
        FMODUnity.RuntimeManager.PlayOneShot(clickFeedback);
        if (target == "credits")
        {
            creditsText.transform.position = originalPos;
            creditsMenu = false;
        }
        else if (target == "options")
        {
            optionsMenu = false;
        }
    }

}
