using UnityEngine;
using System.Collections;

public class TriggerActivate : MonoBehaviour {

    public GameObject[] targets;

	// Use this for initialization
	void Start () {

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].SetActive(false);
        }
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].SetActive(true);
            }
        }
    }
}
