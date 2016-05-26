using UnityEngine;
using System.Collections;

public class BossBaseClass: MonoBehaviour{

    public enum BossStates
    {
        Idle,
        Wander,
        Chase,
        Attack
    }

    public BossStates currentState;

    public GameObject target;
    public GameObject playerObject;

    public float health;
    public GameObject pointA;
    public GameObject pointB;
    public GameObject pointC;

    public float bossSpeed;
    public float speedModifier;
    public float gravity;

    Vector3 moveDirection;
    Vector3 relativePosition;

    Quaternion rotation;

    CharacterController controller;

	// Use this for initialization
	protected void Start () {

        target = pointA;

        playerObject = GameObject.Find("PlayerController");

        bossSpeed = 10.0f;
        speedModifier = 1.0f;
        gravity = 10.0f;

        moveDirection = new Vector3(0, 0, 0);

        currentState = BossStates.Wander;

        controller = GetComponent<CharacterController>();

        relativePosition = target.transform.position - transform.position;
        rotation = Quaternion.LookRotation(relativePosition);

	}
	
	// Update is called once per frame
	protected void Update () {

        //Movement Handler for when boss is on and off the ground
        if (controller.isGrounded && (currentState == BossStates.Wander || currentState == BossStates.Chase))
        {
            moveDirection = new Vector3(0, 0, 1);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= bossSpeed;
        }
        else if (!controller.isGrounded)
        {
            //Gravity drop
            moveDirection.y -= gravity * Time.deltaTime;
        }
        
        switch (currentState)
        {
            case BossStates.Wander:
                speedModifier = 1.0f;
                break;

            case BossStates.Chase:
                speedModifier = 2.0f;
                break;

            default:
                speedModifier = 0.0f;
                break;
        }

        //Boss movement modifier
        controller.Move(moveDirection * speedModifier * Time.deltaTime);
        /*rotation = Quaternion.LookRotation(relativePosition);
        transform.rotation = rotation;*/
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y , target.transform.position.z));

        //Debug.LogFormat("Grounded:{0}       modifier:{1}        state:{2}", controller.isGrounded, speedModifier, currentState);
    }

    public void TakeDamage(float value)
    {
        if ((health - value) > 0)
        {
            health -= value;
        }
        else if ((health - value) < 0)
        {
            health = 0;
            Die();
        }
    }

    public void Attack()
    {

    }

    public void Die()
    {

    }

    public void Alert()
    {
        target = playerObject;
        currentState = BossStates.Chase;
    }

    void OnControllerColliderHit(ControllerColliderHit collider)
    {
        if (currentState != BossStates.Attack || currentState != BossStates.Chase)
        {
            if (collider.gameObject == pointA)
            {
                target = pointB;
                currentState = BossStates.Wander;
            }
            else if (collider.gameObject == pointB)
            {
                target = pointC;
                currentState = BossStates.Wander;
            }
            else if (collider.gameObject == pointC)
            {
                target = pointA;
                currentState = BossStates.Wander;
            }
        }

        if (currentState == BossStates.Chase)
        {
            if (collider.gameObject.tag == "Player")
            {
                Attack();
                currentState = BossStates.Attack;
            }
        }
    }
}
