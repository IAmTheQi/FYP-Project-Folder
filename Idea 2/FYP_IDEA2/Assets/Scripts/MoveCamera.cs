using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

    Transform pointA;
    Transform pointB;
    Transform pointC;

    Transform startPosition;
    Transform targetPosition;

    bool lerpNow;

    float lerpTime;
    float lerpStart;

    float perc; 
    
	// Use this for initialization
	void Start () {

        pointA = GameObject.Find("PlayerController/PointA").transform;
        pointB = GameObject.Find("PlayerController/PointB").transform;
        pointC = GameObject.Find("PlayerController/PointC").transform;

        lerpNow = false;

        lerpTime = 1.0f;
        lerpStart = 0f;

        perc = 0f;

        startPosition = transform;
        targetPosition = pointB;
    }
	
	// Update is called once per frame
	void Update () {

	    if (lerpNow)
        {
            lerpStart += Time.deltaTime;
            if (lerpStart >= lerpTime)
            {
                lerpStart = lerpTime;
            }

            perc = lerpStart / lerpTime;
            transform.position = Vector3.Lerp(startPosition.position, targetPosition.position, perc);

            if (perc == 1)
            {
                lerpNow = false;
            }
        }
	}

    public void LerpCamera(string target)
    {
        switch (target)
        {
            case "A":
                if (transform.position != pointA.position)
                {
                    startPosition = transform;
                    targetPosition = pointA;
                    lerpStart = 0f;
                    lerpNow = true;
                }
                break;

            case "B":
                if (transform.position != pointB.position)
                {
                    startPosition = transform;
                    targetPosition = pointB;
                    lerpStart = 0f;
                    lerpNow = true;
                }
                break;

            case "C":
                if (transform.position != pointB.position)
                {
                    startPosition = transform;
                    targetPosition = pointC;
                    lerpStart = 0f;
                    lerpNow = true;
                }
                break;
        }
    }
}
