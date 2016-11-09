using UnityEngine;
using System.Collections;

public class LastSeenObject : MonoBehaviour {

    Vector3 startPosition;

	// Use this for initialization
	void Start ()
    {
        startPosition = transform.position;
	}

    public void Reset()
    {
        transform.position = startPosition;
    }

    public void Move(Vector3 target)
    {
        transform.position = target;
    }
}
