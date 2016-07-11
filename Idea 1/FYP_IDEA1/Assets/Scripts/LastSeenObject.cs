using UnityEngine;
using System.Collections;

public class LastSeenObject : MonoBehaviour {

    Vector3 startPosition;

	// Use this for initialization
	void Start () {

        startPosition = transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Reset()
    {
        transform.position = startPosition;
    }
}
