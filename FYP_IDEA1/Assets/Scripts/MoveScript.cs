using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

    Vector3 position;

	// Use this for initialization
	void Start () {

        position = transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {


        position.x += 0.15f;
        transform.position = position;
	
	}
}
