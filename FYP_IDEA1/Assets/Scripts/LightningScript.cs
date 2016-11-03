using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LightningScript : MonoBehaviour {

    public GameObject lighting;
    public GameObject lighting2;
    public float onValue;
    public float offValue;
    public float minTime;

    public GameObject buttons;
    public GameObject options;
    public GameObject credits;

    public GameObject creditsText;
    Vector3 originalPos;

    bool optionsMenu;
    bool creditsMenu;

    float lastTime;

    [FMODUnity.EventRef]
    public string lightningSound = "event:/Lightning";

    [FMODUnity.EventRef]
    public string rainSound = "event:/Rain";
    public FMOD.Studio.EventInstance rainInstance;

	// Use this for initialization
	void Start () {
        
        minTime = 3.0f;
        onValue = 0.5f;
        offValue = 0.7f;

        lastTime = 0f;

        rainInstance = FMODUnity.RuntimeManager.CreateInstance(rainSound);
        rainInstance.start();

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

        if ((Time.time - lastTime) > minTime)
        {
            //Debug.LogFormat("time:{0}       past:{1}", lastTime, (Time.time - lastTime));
            if (lighting.activeSelf == false && Random.value > onValue)
            {
                lighting.SetActive(true);
                lighting2.SetActive(true);
                FMODUnity.RuntimeManager.PlayOneShot(lightningSound);
            }
            else if (lighting.activeSelf == true && Random.value > offValue)
            {
                lighting.SetActive(false);
                lighting2.SetActive(false);
                lastTime = Time.time;
            }
        }

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
        SceneManager.LoadScene("LevelPreloader");
        rainInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
        Application.Quit();
    }

    public void OpenOptions()
    {
        optionsMenu = true;
    }

    public void OpenCredits()
    {
        creditsMenu = true;
    }

    public void ReturnFrom(string target)
    {
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
