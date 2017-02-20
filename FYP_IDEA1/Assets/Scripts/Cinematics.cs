using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cinematics : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(9.7f);
        SceneManager.LoadScene("Level1");
    }
}
