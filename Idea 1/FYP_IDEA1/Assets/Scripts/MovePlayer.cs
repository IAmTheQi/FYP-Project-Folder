using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

    GameObject cam1;

    public byte playerSpeed;
    public float strafeSlow;
    public float sprintMultiplier;
    public float crouchMultiplier;
    public float jumpForce;

    bool isGrounded;

    Rigidbody rBody;

	// Use this for initialization
	void Start () {
        cam1 = GameObject.Find("Main Camera");

        playerSpeed = 3;
        strafeSlow = 0.5f;
        sprintMultiplier = 1.0f;
        crouchMultiplier = 1.0f;
        jumpForce = 200.0f;

        rBody = GetComponent<Rigidbody>();

        isGrounded = false;
	}
	
	// Update is called once per frame
	void Update () {

        //Movement keys and speed modifiers
        //WASD movement keys
        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * sprintMultiplier * playerSpeed * crouchMultiplier * Time.deltaTime);
        }

        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * playerSpeed * strafeSlow * crouchMultiplier * Time.deltaTime);
        }

        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * playerSpeed * crouchMultiplier * Time.deltaTime);
        }

        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * playerSpeed * crouchMultiplier * strafeSlow * Time.deltaTime);
        }

        //Sprinting key (Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKey("a") && !Input.GetKey("d"))
        {
            sprintMultiplier = 2.0f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprintMultiplier = 1.0f;
        }

        //Crouch key (Left Control)
        if (Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKey("a") && !Input.GetKey("d"))
        {
            crouchMultiplier = 0.25f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            crouchMultiplier = 1.0f;
        }


        //Jump key (spacebar)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            print("jump");
        }

        print(isGrounded);
	
	}

    //Jump Function
    void Jump()
    {
        rBody.AddForce(Vector3.up * jumpForce);
    }

    //Check Collisions
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = false;
        }
    }
}
