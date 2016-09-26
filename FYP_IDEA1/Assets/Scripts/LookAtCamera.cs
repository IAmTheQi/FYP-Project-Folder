using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

    GameObject mainCamera;

	// Use this for initialization
	void Start () {

        mainCamera = GameObject.Find("Main Camera");
    }
	
	// Update is called once per frame
	void Update () {

        transform.LookAt(mainCamera.transform);

	}
}
