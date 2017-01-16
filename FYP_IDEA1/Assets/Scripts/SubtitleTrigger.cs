using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubtitleTrigger : MonoBehaviour {

    GameObject subtitlePanel;

    public string script;
    public float seconds;
    bool activated;

	// Use this for initialization
	void Start () {

        subtitlePanel = GameObject.Find("SubtitlePanel");
        activated = false;
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && !activated)
        {
            StartCoroutine(ShowSubtitle());
        }
    }

    IEnumerator ShowSubtitle()
    {
        activated = true;
        subtitlePanel.GetComponent<Text>().text = script;
        yield return new WaitForSeconds(seconds);
        subtitlePanel.GetComponent<Text>().text = "";
        Destroy(gameObject);
    }
}
