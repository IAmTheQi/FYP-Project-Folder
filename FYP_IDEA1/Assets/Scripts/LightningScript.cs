using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LightningScript : MonoBehaviour {

    public GameObject buttons;
    public GameObject options;
    public GameObject credits;

    public GameObject creditContent;

    bool optionsMenu;
    bool creditsMenu;
    bool scrollStart;

    RectTransform contentTransform;
    Vector2 contentlocalPosition;

    [FMODUnity.EventRef]
    public string clickFeedback = "event:/Click";

    // Use this for initialization
    void Start () {

        optionsMenu = false;
        creditsMenu = false;
        scrollStart = false;
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
        }

        if (creditsMenu)
        {
            if (!scrollStart && creditContent.GetComponent<RectTransform>().localPosition.y == -1170f)
            {
                scrollStart = true;
            }
            else if (!scrollStart && creditContent.GetComponent<RectTransform>().localPosition.y != -1170f)
            {
                contentlocalPosition = creditContent.GetComponent<RectTransform>().localPosition;
                contentlocalPosition.y = -1170f;
                creditContent.GetComponent<RectTransform>().localPosition = contentlocalPosition;
            }

            if (scrollStart && creditContent.GetComponent<RectTransform>().localPosition.y < 1730f)
            {
                creditContent.transform.Translate(Vector3.up * Time.deltaTime * 50f);
            }
            else if (scrollStart && creditContent.GetComponent<RectTransform>().localPosition.y >= 1730f)
            {
                contentlocalPosition = creditContent.GetComponent<RectTransform>().localPosition;
                contentlocalPosition.y = -1170f;
                creditContent.GetComponent<RectTransform>().localPosition = contentlocalPosition;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ReturnFrom("credits");
            }
        }

        Debug.LogFormat("credits:{0}      options:{1}", creditsMenu, optionsMenu);
	}

    public void PlayGame()
    {
        FMODUnity.RuntimeManager.PlayOneShot(clickFeedback);
        SceneManager.LoadScene("Cinematics");
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
            creditsMenu = false;
        }
        else if (target == "options")
        {
            optionsMenu = false;
        }
    }

}
