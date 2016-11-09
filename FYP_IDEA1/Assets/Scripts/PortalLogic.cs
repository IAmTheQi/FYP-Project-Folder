using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalLogic : MonoBehaviour {

    public string targetScene;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}
