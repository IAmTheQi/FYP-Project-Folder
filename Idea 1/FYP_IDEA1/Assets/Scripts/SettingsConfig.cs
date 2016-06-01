using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SettingsConfig : MonoBehaviour {

    public byte mouseSense;
    public byte mouseDrag;

	// Use this for initialization
	void Awake () {

	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScene();
        }
	}

    public void ResetScene()
    {
        SceneManager.LoadScene("Level1");
    }
}
