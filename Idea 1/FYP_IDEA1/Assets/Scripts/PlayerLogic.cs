using UnityEngine;
using System.Collections;

public class PlayerLogic : MonoBehaviour {

    GameObject cam1;
    Transform camTransform;

    public Texture2D crossHair;

    public byte playerSpeed;
    public float strafeSlow;
    public float sprintMultiplier;
    public float crouchMultiplier;
    public float jumpForce;
    public float gravity;

    Vector3 moveDirection;
    
    CharacterController controller;

    RaycastHit hit;

	// Use this for initialization
	void Start () {
        cam1 = GameObject.Find("Main Camera");
        camTransform = cam1.transform;

        playerSpeed = 2;
        strafeSlow = 0.5f;
        sprintMultiplier = 1.0f;
        crouchMultiplier = 1.0f;
        jumpForce = 3.0f;
        gravity = 10.0f;

        controller = GetComponent<CharacterController>();
        
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	}
	
	// Update is called once per frame
	void Update () {

        //Movement Handler for when player is on and off the ground
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= playerSpeed * strafeSlow * sprintMultiplier * crouchMultiplier;

            //Jumping (Space)
            if (Input.GetKey(KeyCode.Space))
            {
                moveDirection.y = jumpForce;
            }
        }
        else if (!controller.isGrounded)
        {
            //Gravity drop
            moveDirection.y -= gravity * Time.deltaTime;
        }


        controller.Move(moveDirection * playerSpeed * crouchMultiplier * Time.deltaTime);

        
        
        //Sprinting key (Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.LeftControl))
        {
            sprintMultiplier = 2.0f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprintMultiplier = 1.0f;
        }

        //Crouch key (Left Control)
        if (Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            crouchMultiplier = 0.75f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            crouchMultiplier = 1.0f;
        }
        
        if (Input.GetMouseButton(0))
        {
            ShootRay();
        }
	}

    void ShootRay()
    {
        Ray ray = new Ray(camTransform.position, camTransform.forward);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            print(hit.collider.name);
            Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
        }
    }

    void OnGUI()
    {
        float xMin = (Screen.width / 2) - (50);
        float yMin = (Screen.height / 2) - (50);

        GUI.DrawTexture(new Rect(xMin, yMin, 100, 100), crossHair);
    }
}
