using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour {

    PlayerLogic playerScript;

    bool activated;
    bool fade;

    Color endColor;

    public GameObject background;
    public GameObject endImage;

	// Use this for initialization
	void Start () {

        playerScript = GameObject.Find("PlayerController").GetComponent<PlayerLogic>();

        activated = false;
        fade = false;

        endColor = endImage.GetComponent<Image>().color;
        endColor.a = 0;

        endImage.GetComponent<Image>().color = endColor;

        background.SetActive(false);
        endImage.SetActive(false);
	
	}

    void Update()
    {
        if (activated && !fade)
        {
            endColor = endImage.GetComponent<Image>().color;
            endColor.a += 0.01f;
            endImage.GetComponent<Image>().color = endColor;

            if (endColor.a >= 0.95f)
            {
                fade = true;
                background.SetActive(true);
            }
        }
        else if (activated && fade)
        {
            endColor = endImage.GetComponent<Image>().color;
            endColor.a -= 0.01f;
            endImage.GetComponent<Image>().color = endColor;

            if (endColor.a <= 0.05f)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        endColor.a = Mathf.Clamp(endColor.a, 0.0f, 1.0f);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && !activated)
        {
            playerScript.EndGame();
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1.5f);
        activated = true;
        endImage.SetActive(true);
        StopCoroutine(EndGame());
    }
}
