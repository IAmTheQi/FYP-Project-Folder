using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlashLogic : MonoBehaviour {

    Color imageAlpha;

    bool fadeIn;
    bool fadeOut;

	// Use this for initialization
	void Start () {

        imageAlpha = GetComponent<Image>().color;
        imageAlpha.a = 0;
        GetComponent<Image>().color = imageAlpha;

        fadeIn = false;
        fadeOut = false;
	
	}
	
	// Update is called once per frame
	void Update () {

        if (fadeIn)
        {
            imageAlpha = GetComponent<Image>().color;
            imageAlpha.a += 0.05f;

            if (imageAlpha.a >= 0.9f)
            {
                fadeIn = false;
                StartCoroutine(Delay());
            }
        }

        if (fadeOut)
        {

            imageAlpha = GetComponent<Image>().color;
            imageAlpha.a -= 0.01f;
        }

        imageAlpha.a = Mathf.Clamp(imageAlpha.a, 0f, 1f);
        GetComponent<Image>().color = imageAlpha;
	}

    public void Activate()
    {
        imageAlpha.a = 0;
        GetComponent<Image>().color = imageAlpha;
        RandomTransform();
        fadeIn = true;
        fadeOut = false;
    }

    void RandomTransform()
    {
        float scaleFactor = Random.Range(0.5f, 1.0f);
        float offsetWidth = (Screen.width - GetComponent<RectTransform>().rect.width * scaleFactor) / 2;
        float offsetHeight = (Screen.height - GetComponent<RectTransform>().rect.height * scaleFactor) / 2;
        
        GetComponent<RectTransform>().anchoredPosition = new Vector2((Random.Range(0, Screen.width) - Screen.width / 2), Random.Range(0, Screen.height) - Screen.height / 2);
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        transform.localEulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        fadeOut = true;
    }
}
