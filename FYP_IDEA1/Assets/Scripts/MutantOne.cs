using UnityEngine;
using System.Collections;

public class MutantOne : MutantBaseClass {

	// Use this for initialization
	void Start () {

        health = 100.0f;
        
        base.Start();
	
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	
	}

    void OnControllerColliderHit(ControllerColliderHit collider)
    {
        if (collider.gameObject == playerLastPosition)
        {
            CalmMutant();
        }
        
        if (collider.gameObject == startPosition)
        {
            currentState = MutantStates.Idle;
        }

        if (collider.gameObject.tag == "Noise")
        {
            CalmMutant();
        }
    }
}
