using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cinematics : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Timer());
	}

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(36f);
        SceneManager.LoadScene("Level1");
    }
}
