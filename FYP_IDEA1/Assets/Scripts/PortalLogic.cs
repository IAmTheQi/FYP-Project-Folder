using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalLogic : MonoBehaviour {

    public string targetScene;

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        int level = 0;

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            level = 1;
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            level = 2;
        }
        else if (SceneManager.GetActiveScene().name == "Level3")
        {
            level = 3;
        }
            if (collider.tag == "Player")
        {
            GameHandler.Save(collider.gameObject, level);
            collider.gameObject.GetComponent<PlayerLogic>().StopSoundInstances();
            SceneManager.LoadScene(targetScene);
        }
    }
}
