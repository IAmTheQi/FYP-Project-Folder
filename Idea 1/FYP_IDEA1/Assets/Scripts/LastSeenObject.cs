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

    public void Move(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    public void Reset()
    {
        transform.position = startPosition;
    }
}
