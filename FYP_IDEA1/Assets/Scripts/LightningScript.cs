using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LightningScript : MonoBehaviour {

    public GameObject lighting;
    public GameObject lighting2;
    public float onValue;
    public float offValue;
    public float minTime;

    float lastTime;

    [FMODUnity.EventRef]
    public string lightningSound = "event:/Lightning";

    [FMODUnity.EventRef]
    public string rainSound = "event:/Rain";
    public FMOD.Studio.EventInstance rainInstance;

	// Use this for initialization
	void Start () {
        
        minTime = 3.0f;
        onValue = 0.5f;
        offValue = 0.7f;

        lastTime = 0f;

        rainInstance = FMODUnity.RuntimeManager.CreateInstance(rainSound);
        rainInstance.start();
	}
	
	// Update is called once per frame
	void Update () {

        if ((Time.time - lastTime) > minTime)
        {
            Debug.LogFormat("time:{0}       past:{1}", lastTime, (Time.time - lastTime));
            if (lighting.activeSelf == false && Random.value > onValue)
            {
                lighting.SetActive(true);
                lighting2.SetActive(true);
                FMODUnity.RuntimeManager.PlayOneShot(lightningSound);
            }
            else if (lighting.activeSelf == true && Random.value > offValue)
            {
                lighting.SetActive(false);
                lighting2.SetActive(false);
                lastTime = Time.time;
            }
        }
	
	}

    public void PlayGame()
    {
        SceneManager.LoadScene("LevelPreloader");
        rainInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("LevelPreloader");

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
