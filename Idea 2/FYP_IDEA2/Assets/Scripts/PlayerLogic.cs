using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLogic : MonoBehaviour {

    GameObject cam1;
    SmoothMouseLook lookScript;
    Transform camTransform;
    Vector3 startPosition;

    GameObject animationObject;
    Animator playerAnimator;

    enum PlayerStates
    {
        Run,
        Walk,
        Crouch,
        Prone,
        Idle,
        Jump
    }

    enum Weapons
    {
        SwordAndShield,
        SpearAndShield,
        BowAndArrow,
        MagicBook
    }

    float walkHeight;
    float crouchHeight;
    float proneHeight;

    bool isWalking;
    bool idleState;

    bool attackOne;
    bool attackTwo;

    float attackWindow;
    float attackOneLength;
    float attackTwoLength;

    public Texture2D crossHair;

    public byte playerSpeed;
    public float strafeSlow;
    public float speedModifier;
    public float jumpForce;
    public float gravity;

    float initialVelocity;
    float currentVelocity;
    float maxVelocity;
    float forwardAccelerationRate;
    float reverseAccelerationRate;
    float deccelerationRate;

    Vector3 moveDirection;

    CharacterController controller;

    RaycastHit hit;
    PlayerStates currentState;
    Weapons currentWeapon;
    float timeStamp;

    // Use this for initialization
    void Start() {
        cam1 = GameObject.Find("Main Camera");
        camTransform = cam1.transform;
        lookScript = cam1.GetComponent<SmoothMouseLook>();

        animationObject = GameObject.Find("WomanWarrior");
        playerAnimator = animationObject.GetComponent<Animator>();

        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        

        playerSpeed = 2;
        strafeSlow = 0.5f;
        speedModifier = 1.0f;
        jumpForce = 3.0f;
        gravity = 10.0f;

        crouchHeight = 1.75f;
        walkHeight = 2.5f;
        proneHeight = 1.0f;

        isWalking = false;
        idleState = true;

        attackOne = false;
        attackTwo = false;

        attackWindow = 0.2f;
        attackOneLength = 0.8f;
        attackTwoLength = 0.75f;

        controller = GetComponent<CharacterController>();

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        currentState = PlayerStates.Idle;

        initialVelocity = 1.0f;
        currentVelocity = 0.0f;
        maxVelocity = 5.0f;
        forwardAccelerationRate = 2.0f;
        reverseAccelerationRate = 0.5f;
        deccelerationRate = 4.0f;

        timeStamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        //Movement Handler for when player is on and off the ground
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= playerSpeed * speedModifier;
            currentState = PlayerStates.Walk;

            //Jumping (Space)
            if (Input.GetKey(KeyCode.Space))
            {
                moveDirection.y = jumpForce;
                currentState = PlayerStates.Jump;

            }
        }
        else if (!controller.isGrounded)
        {
            //Gravity drop
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //Input key Handler
        if (Input.GetKey(KeyCode.W) && !attackOne)
        {
            idleState = false;
            currentVelocity += forwardAccelerationRate * Time.deltaTime;
            maxVelocity = 5.0f;
        }
        else if (Input.GetKey(KeyCode.S) && !attackOne)
        {
            idleState = false;
            currentVelocity += reverseAccelerationRate * Time.deltaTime;
            maxVelocity = 2.0f;
        }
        else
        {
            currentVelocity -= deccelerationRate * Time.deltaTime;
            if (currentVelocity < 4.5)
            {
                idleState = true;
            }
        }

        currentVelocity = Mathf.Clamp(currentVelocity, initialVelocity, maxVelocity);

        //Player movement modifier
        controller.Move(moveDirection * currentVelocity * Time.deltaTime);

        playerAnimator.SetFloat("Velocity", currentVelocity);
        playerAnimator.SetBool("Idle", idleState);

        //Sprinting key (Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.Z))
        {
            currentState = PlayerStates.Run;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentState = PlayerStates.Idle;
        }

        //Crouch key (Left Control)
        if (Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.Z))
        {
            currentState = PlayerStates.Crouch;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            currentState = PlayerStates.Idle;
        }

        //Prone key ("Z" key)
        if (Input.GetKeyDown(KeyCode.Z) && !Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentState = PlayerStates.Prone;
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            currentState = PlayerStates.Idle;
        }

        //Left Mouse Button Shoot
        if (Input.GetMouseButton(0))
        {
            print("click");
            if (!attackOne)
            {
                attackOne = true;
                timeStamp = Time.time;
            }

            if (attackOne)
            {
                if (Time.time > timeStamp + attackOneLength - attackWindow && Time.time < timeStamp + attackOneLength)
                {
                    attackTwo = true;
                    timeStamp = Time.time;
                }
            }
        }

        if (attackOne)
        {
            if (Time.time > (timeStamp + attackOneLength))
            {
                attackOne = false;
            }
            
        }

        if (attackTwo)
        {
            if (Time.time > (timeStamp + attackTwoLength))
            {
                attackTwo = false;
            }
        }

        Debug.LogFormat("Time.time: {0}     timeStamp: {1}      attackone:{2}       attacktwo:{3}", Time.time, timeStamp, attackOne, attackTwo);

        playerAnimator.SetBool("Attack1", attackOne);
        playerAnimator.SetBool("Attack2", attackTwo);

        //Check current movement state to adjust speed multiplier accordingly
        switch (currentState)
        {
            case PlayerStates.Walk:
                speedModifier = 1.0f;
                controller.height = walkHeight;
                lookScript.minimumY = -80f;
                lookScript.maximumY = 80f;
                break;
            case PlayerStates.Run:
                speedModifier = 2.0f;
                controller.height = walkHeight;
                lookScript.minimumY = -80f;
                lookScript.maximumY = 80f;
                break;
            case PlayerStates.Crouch:
                speedModifier = 0.75f;
                controller.height = crouchHeight;
                lookScript.minimumY = -40f;
                lookScript.maximumY = 80f;
                break;
            case PlayerStates.Prone:
                speedModifier = 0.5f;
                controller.height = proneHeight;
                lookScript.minimumY = 0f;
                lookScript.maximumY = 40f;
                break;
            case PlayerStates.Idle:
                speedModifier = 2.0f;
                controller.height = walkHeight;
                lookScript.minimumY = -80f;
                lookScript.maximumY = 80f;
                break;
            case PlayerStates.Jump:
                speedModifier = 2.0f;
                controller.height = walkHeight;
                lookScript.minimumY = -80f;
                lookScript.maximumY = 80f;
                break;
        }
    }

    //Enemy detection
    void OnControllerColliderHit(ControllerColliderHit collider)
    {
        if (collider.gameObject.tag == "Mutant")
        {
            KillPlayer();
        }
    }

    //Die function
    void KillPlayer()
    {
        transform.position = startPosition;
    }

    public bool IsWalking()
    {
        if (currentState == PlayerStates.Walk || currentState == PlayerStates.Run)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        return isWalking;
    }
}
