using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PreloaderScript : MonoBehaviour {

    bool loadScene;

    [SerializeField]
    string scene;

    AsyncOperation async;


    void Awake()
    {
        StartCoroutine(LoadScene());

        loadScene = false;
    }
	
	// Update is called once per frame
	void Update () {

        Debug.Log(async.progress);
    }

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;

        if (async.isDone)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                loadScene = true;
            }
        }

        while(loadScene)
        {
            async.allowSceneActivation = true;
            yield return null;
        }
    }
}
