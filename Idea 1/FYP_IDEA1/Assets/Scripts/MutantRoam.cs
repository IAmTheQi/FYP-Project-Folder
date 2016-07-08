using UnityEngine;
using System.Collections;

public class MutantRoam : MutantBaseClass {

    public GameObject[] targetsArray;

    byte currentIndex;

	// Use this for initialization
	void Start () {

        base.Start();

        currentIndex = 0;

        currentState = MutantStates.Wander;
	
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
            if (currentIndex == targetsArray.Length - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex += 1;
            }
        }
    }
}
