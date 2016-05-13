using UnityEngine;
using System.Collections;

public class MutantLogic : MonoBehaviour {

    enum MutantStates
    {
        Wander,
        Alerted,
        Chase,
        Attacked
    }

    GameObject playerObject;

    float mutantSpeed;
    float gravity;

    Vector3 moveDirection;

    public float detectionRange;

    MutantStates currentState;

    CharacterController controller;

	// Use this for initialization
	void Start () {

        playerObject = GameObject.Find("PlayerController");

        mutantSpeed = 1.0f;
        gravity = 10.0f;

        moveDirection = new Vector3(0, 0, 0);

        currentState = MutantStates.Wander;

        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

        //Movement Handler for when mutant is on and off the ground
        if (controller.isGrounded && currentState == MutantStates.Chase)
        {
            moveDirection = new Vector3(0, 0, 1);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= mutantSpeed;
        }
        else if (!controller.isGrounded)
        {
            //Gravity drop
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //Mutant movement modifier
        controller.Move(moveDirection * Time.deltaTime);

        if (currentState == MutantStates.Alerted || currentState == MutantStates.Chase)
        {
            transform.LookAt(playerObject.transform);
        }
    }

    public void AlertMutant()
    {
        currentState = MutantStates.Chase;
    }
}
