using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreenScript : MonoBehaviour {

    public GameObject logo;

    Color logoAlpha;

    bool fade;

	// Use this for initialization
	void Start () {

        logoAlpha = logo.GetComponent<Image>().color;
        fade = false;
	
	}
	
	// Update is called once per frame
	void Update () {


        if (!fade)
        {
            logoAlpha.a += 0.05f;
            logo.GetComponent<Image>().color = logoAlpha;
        }
        else if (fade)
        {
            logoAlpha.a -= 0.05f;
            logo.GetComponent<Image>().color = logoAlpha;
        }

        if (logo.GetComponent<Image>().color.a >= 1)
        {
            StartCoroutine(Delay());
        }

        if (logo.GetComponent<Image>().color.a <= 0 && fade)
        {
            SceneManager.LoadScene("MainMenu");
        }
	
	}

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.0f);
        fade = true;
    }
}
