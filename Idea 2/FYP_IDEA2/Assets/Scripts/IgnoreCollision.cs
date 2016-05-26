using UnityEngine;
using System.Collections;

public class IgnoreCollision : MonoBehaviour {

    public GameObject pointA;
    public GameObject pointB;
    public GameObject pointC;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Physics.IgnoreCollision(pointA.GetComponent<Collider>(), GetComponent<Collider>());
        Physics.IgnoreCollision(pointB.GetComponent<Collider>(), GetComponent<Collider>());
        Physics.IgnoreCollision(pointC.GetComponent<Collider>(), GetComponent<Collider>());
    }
}
