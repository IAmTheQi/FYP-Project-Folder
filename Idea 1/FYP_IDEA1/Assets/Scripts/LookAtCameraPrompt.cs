using UnityEngine;
using System.Collections;

public class LookAtCameraPrompt : MonoBehaviour {

    GameObject mainCamera;

    GameObject player;

    GameObject child1;
    GameObject child2;

	// Use this for initialization
	void Start () {

        mainCamera = GameObject.Find("Main Camera");

        player = GameObject.Find("PlayerController");

        child1 = transform.GetChild(0).gameObject;
        child1.SetActive(false);

        child2 = transform.GetChild(1).gameObject;
        child2.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(transform.position, player.transform.position) < 10)
        {
            transform.LookAt(mainCamera.transform);
            child1.SetActive(true);
        }
        else
        {
            child1.SetActive(false);
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 5)
        {
            child2.SetActive(true);
        }
        else
        {
            child2.SetActive(false);
        }
	}
}
