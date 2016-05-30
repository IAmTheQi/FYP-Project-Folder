using UnityEngine;
using System.Collections;

public class MutantBaseClass : MonoBehaviour {

    public float health;

    enum MutantStates
    {
        Wander,
        Alerted,
        Chase,
        Attacked,
        Lost
    }

    GameObject playerObject;

    float mutantSpeed;
    float gravity;

    bool dead;

    Vector3 moveDirection;

    MutantStates currentState;

    CharacterController controller;

    protected GameObject playerLastPosition;

    // Use this for initialization
    protected void Start () {
        dead = false;

        playerObject = GameObject.Find("PlayerController");

        mutantSpeed = 0.0f;
        gravity = 10.0f;

        moveDirection = new Vector3(0, 0, 0);

        currentState = MutantStates.Wander;

        controller = GetComponent<CharacterController>();

        playerLastPosition = GameObject.Find("PlayerLastSeen");

    }
	
	// Update is called once per frame
	protected void Update () {

        //Movement Handler for when mutant is on and off the ground
        if (controller.isGrounded)
        {
            if (currentState == MutantStates.Lost || currentState == MutantStates.Chase)
            {
                moveDirection = new Vector3(0, 0, 1);
            }
            else if (currentState == MutantStates.Wander)
            {
                moveDirection = new Vector3(0, 0, 0);
            }

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= mutantSpeed;
        }
        else if (!controller.isGrounded)
        {
            //Gravity drop
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (currentState == MutantStates.Chase || currentState == MutantStates.Lost)
        {
            mutantSpeed = 1.0f;
        }

        //Mutant movement modifier
        controller.Move(moveDirection * Time.deltaTime);

        if ((currentState == MutantStates.Alerted || currentState == MutantStates.Chase) && currentState != MutantStates.Lost)
        {
            transform.LookAt(new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z));
        }
        else if (currentState == MutantStates.Lost)
        {
            transform.LookAt(new Vector3(playerLastPosition.transform.position.x, transform.position.y, playerLastPosition.transform.position.z));
        }
    }

    public void AlertMutant()
    {
        currentState = MutantStates.Chase;
    }

    public void LosePlayer()
    {
        currentState = MutantStates.Lost;
    }

    public void TakeDamage(float value)
    {
        if ((health - value) > 0)
        {
            health -= value;
        }
        else if ((health - value) <= 0)
        {
            health = 0;
            Die();
        }
    }

    protected void CalmMutant()
    {
        currentState = MutantStates.Wander;
        playerLastPosition.SendMessage("Reset");
    }

    protected void Die()
    {
        gameObject.SetActive(false);
    }
}
