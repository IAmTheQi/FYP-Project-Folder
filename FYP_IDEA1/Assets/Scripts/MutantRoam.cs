using UnityEngine;
using System.Collections;

public class MutantRoam : MutantBaseClass {

    public GameObject[] targetsArray;

    byte currentIndex;

    bool forward;

	// Use this for initialization
	void Start () {

        base.Start();

        currentIndex = 0;

        currentState = MutantStates.Wander;

        forward = true;
	}
	
	// Update is called once per frame
	void Update () {

        base.Update();
	
        if (currentState == MutantStates.Wander)
        {
            transform.LookAt(new Vector3(targetsArray[currentIndex].transform.position.x, transform.position.y, targetsArray[currentIndex].transform.position.z));
        }
	}

    public override void CalmMutant()
    {
        currentState = MutantStates.Wander;
        playerLastPosition.SendMessage("Reset");
        noiseLastPosition.SendMessage("Reset");
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == targetsArray[currentIndex])
        {
            if (forward)
            {
                if (currentIndex == targetsArray.Length - 1)
                {
                    forward = false;
                    currentIndex -= 1;
                }
                else
                {
                    currentIndex += 1;
                }
            }
            else if (!forward)
            {
                if (currentIndex == 0)
                {
                    forward = true;
                    currentIndex = 1;
                }
                else
                {
                    currentIndex -= 1;
                }
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit collider)
    {
        if (collider.gameObject == playerLastPosition)
        {
            CalmMutant();
        }

        if (collider.gameObject == startPosition)
        {
            currentState = MutantStates.Wander;
        }

        if (collider.gameObject.tag == "Noise")
        {
            CalmMutant();
        }
    }
}
