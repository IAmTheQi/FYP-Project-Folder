using UnityEngine;
using System.Collections;

public class LocationHandler : MonoBehaviour {

    public GameObject[] waypointArray;

	// Use this for initialization
	void Start () {

        waypointArray = GameObject.FindGameObjectsWithTag("LocationWaypoint");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
