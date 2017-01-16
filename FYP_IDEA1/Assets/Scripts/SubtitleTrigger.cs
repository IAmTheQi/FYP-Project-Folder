using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubtitleTrigger : MonoBehaviour {

    GameObject subtitlePanel;

    public string script;
    public float seconds;

	// Use this for initialization
	void Start () {

        subtitlePanel = GameObject.Find("SubtitlePanel");
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {

        }
    }
    IEnumerator ShowSubtitle()
    {
        subtitlePanel.GetComponent<Text>().text = script;
        yield return new WaitForSeconds(seconds);
        subtitlePanel.GetComponent<Text>().text = "";
        Destroy(gameObject);
    }
}
