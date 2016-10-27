using UnityEngine;
using System.Collections;

public class LightningScript : MonoBehaviour {

    public GameObject lighting;
    public float onValue;
    public float offValue;
    public float minTime;

    float lastTime;

	// Use this for initialization
	void Start () {
        
        minTime = 3.0f;
        onValue = 0.5f;
        offValue = 0.7f;

        lastTime = 0f;
	
	}
	
	// Update is called once per frame
	void Update () {

        if ((Time.time - lastTime) > minTime)
        {
            Debug.LogFormat("time:{0}       past:{1}", lastTime, (Time.time - lastTime));
            if (lighting.activeSelf == false && Random.value > onValue)
            {
                lighting.SetActive(true);
            }
            else if (lighting.activeSelf == true && Random.value > offValue)
            {
                lighting.SetActive(false);
                lastTime = Time.time;
            }
        }
	
	}
}
