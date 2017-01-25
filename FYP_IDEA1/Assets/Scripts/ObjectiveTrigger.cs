using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectiveTrigger : MonoBehaviour {

    public GameObject objectiveImage;

    Color objColor;
    bool triggered;
    bool fade;

	// Use this for initialization
	void Start () {

        objColor = objectiveImage.GetComponent<Image>().color;
        objColor.a = 0;
        objectiveImage.GetComponent<Image>().color = objColor;
        objectiveImage.SetActive(false);

        triggered = false;
        fade = false;
	
	}

    void Update()
    {
        if (triggered && !fade)
        {
            objColor = objectiveImage.GetComponent<Image>().color;
            objColor.a += 0.03f;
            objectiveImage.GetComponent<Image>().color = objColor;
        }
        else if (triggered && fade)
        {
            objColor = objectiveImage.GetComponent<Image>().color;
            objColor.a -= 0.03f;
            objectiveImage.GetComponent<Image>().color = objColor;

            if (objColor.a <= 0.05f)
            {
                objectiveImage.SetActive(false);
                Destroy(this.gameObject);
            }
        }


        objColor.a = Mathf.Clamp(objColor.a, 1.0f, 0.0f);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && !triggered)
        {
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        objectiveImage.SetActive(true);
        triggered = true;
        yield return new WaitForSeconds(10.0f);
        fade = true;
        StopCoroutine(Fade());
    }
}
