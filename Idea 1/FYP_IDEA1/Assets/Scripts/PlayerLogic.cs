using UnityEngine;
using System.Collections;

public class PlayerLogic : MonoBehaviour {

    GameObject cam1;
    Transform camTransform;
    enum Inventory
    {
        Rifle,
        Pistol,
        Knife,
        Katana,
        Sniper
    }
    
    [System.Serializable]
    public struct Weapons
    {
        public string weaponName;
        public byte currentAmmo;
        public byte magazineSize;
        public byte remainingAmmo;
        public byte totalAmmo;
        public float shootDelay;
        public float reloadDelay;
        public float range;
    }

    byte currentWeaponIndex;

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
    Inventory currentSelected;
    public Weapons[] weapons;
    float timeStamp;
    bool reloadState;

    // Use this for initialization
    void Start() {
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

        currentSelected = Inventory.Rifle;
        currentWeaponIndex = 0;

        timeStamp = Time.time;
        reloadState = false;
    }

    // Update is called once per frame
    void Update() {

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

        //Player movement modifier
        controller.Move(moveDirection * playerSpeed * crouchMultiplier * Time.deltaTime);


        //Weapon change key handler
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //If current weapon is not alredy the target weapon
            if (currentSelected != Inventory.Rifle)
            {
                currentSelected = Inventory.Rifle;
                currentWeaponIndex = 0;
                reloadState = false;
                timeStamp = Time.time;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //If current weapon is not alredy the target weapon
            if (currentSelected != Inventory.Pistol)
            {
                currentSelected = Inventory.Pistol;
                currentWeaponIndex = 1;
                reloadState = false;
                timeStamp = Time.time;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //If current weapon is not alredy the target weapon
            if (currentSelected != Inventory.Knife)
            {
                currentSelected = Inventory.Knife;
                currentWeaponIndex = 2;
                reloadState = false;
                timeStamp = Time.time;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //If current weapon is not alredy the target weapon
            if (currentSelected != Inventory.Katana)
            {
                currentSelected = Inventory.Katana;
                currentWeaponIndex = 3;
                reloadState = false;
                timeStamp = Time.time;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            //If current weapon is not alredy the target weapon
            if (currentSelected != Inventory.Sniper)
            {
                currentSelected = Inventory.Sniper;
                currentWeaponIndex = 4;
                reloadState = false;
                timeStamp = Time.time;
            }
        }

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

        //Left Mouse Button Shoot
        if (Input.GetMouseButton(0))
        {
            if (Time.time > (timeStamp + weapons[currentWeaponIndex].shootDelay) && !reloadState)
            {
                ShootRay();
                weapons[currentWeaponIndex].currentAmmo -= 1;
                timeStamp = Time.time;
            }
        }

        //Check for Reload need
        if (weapons[currentWeaponIndex].currentAmmo <= 0)
        {
            reloadState = true;
            timeStamp = Time.time;
        }

        if (reloadState)
        {
            if (Time.time >= timeStamp + weapons[currentWeaponIndex].reloadDelay)
            {
                ReloadWeapon();
            }
        }
    }

    //Shooting raycast check
    void ShootRay()
    {
        //Fires ray from camera, centre of screen into 3D space
        Ray ray = new Ray(camTransform.position, camTransform.forward);
        if (Physics.Raycast(ray, out hit, weapons[currentWeaponIndex].range))
        {
            print(hit.collider.name);
            Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
        }
    }

    //GUI Crosshair
    void OnGUI()
    {
        //Positions crosshair in the centre of the screen
        float xMin = (Screen.width / 2) - (50);
        float yMin = (Screen.height / 2) - (50);

        GUI.DrawTexture(new Rect(xMin, yMin, 100, 100), crossHair);
    }

    //Reload Weapon Function
    void ReloadWeapon()
    {
        //Check if remaining ammo is not enough to fill up a full magazine
        if (weapons[currentWeaponIndex].remainingAmmo < weapons[currentWeaponIndex].magazineSize)
        {
            weapons[currentWeaponIndex].currentAmmo = weapons[currentWeaponIndex].remainingAmmo;
            weapons[currentWeaponIndex].remainingAmmo = 0;
        }
        else //Normal reload
        {
            weapons[currentWeaponIndex].currentAmmo = weapons[currentWeaponIndex].magazineSize;
            weapons[currentWeaponIndex].remainingAmmo -= weapons[currentWeaponIndex].magazineSize;
        }
        
    }
    
    
    /*
    var bulletSpeed: float = 1000; // bullet speed in meters/second var shotSound: AudioClip;

    //Delayed raycast shooting (KIV)
    function Fire()
    {
        if (audio) audio.PlayOneShot(shotSound); // the sound plays immediately 
        var hit: RaycastHit; // do the exploratory raycast first: 
        if (Physics.Raycast(transform.position, transform.forward, hit))
        {
            var delay = hit.distance / bulletSpeed; // calculate the flight time 
            var hitPt = hit.point;
            hitPt.y -= delay * 9.8; // calculate the bullet drop at the target 
            var dir = hitPt - transform.position; // use this to modify the shot direction 
            yield WaitForSeconds(delay); // wait for the flight time 

            // then do the actual shooting: 
            if (Physics.Raycast(transform.position, dir, hit)
            {
                // do here the usual job when something is hit: 
                // apply damage, instantiate particles etc. 
            }
        }
    }*/
}
