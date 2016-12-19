using UnityEngine;
using System.Collections;

public class MutantPatrol : MutantSimple {

    public Transform[] targetsArray;

    byte currentIndex;

    bool forward;

    // Use this for initialization
    void Start () {

        base.Start();

        currentIndex = 0;

        currentState = MutantStates.Roam;

        forward = true;
    }
	
	// Update is called once per frame
	void Update () {

        base.Update();

        if (currentState == MutantStates.Roam)
        {
            mutantAgent.destination = targetsArray[currentIndex].position;

            if (Vector3.Distance(transform.position, targetsArray[currentIndex].position) <= 2f)
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
    }
}
