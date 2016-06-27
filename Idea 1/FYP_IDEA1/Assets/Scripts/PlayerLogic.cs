using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLogic : MonoBehaviour {

    GameObject cam1;
    MoveCamera cameraLerpScript;
    SmoothMouseLook lookScript;
    Transform camTransform;
    Vector3 startPosition;

    public ParticleSystem gunParticle;

    public bool pauseGame;
    public bool weaponSelect;

    GameObject ammoText;
    GameObject reloadText;
    GameObject weaponIcon;

    GameObject healthBar;

    GameObject weaponWheel;
    GameObject riflePanel;
    GameObject pistolPanel;
    GameObject knifePanel;
    GameObject katanaPanel;
    GameObject sniperPanel;

    public enum Inventory
    {
        Rifle,
        Pistol,
        Knife,
        Katana,
        Sniper
    }

    enum PlayerStates
    {
        Run,
        Walk,
        Crouch,
        Prone,
        Idle,
        Jump
    }
    
    [System.Serializable]
    public struct Weapons
    {
        public string weaponName;
        public Sprite weaponSprite;
        public Sprite selectedPanel;
        public Sprite unselectedPanel;
        public Sprite lockedPanel;
        public bool locked;
        public byte currentAmmo;
        public byte magazineSize;
        public byte remainingAmmo;
        public byte totalAmmo;
        public float shootDelay;
        public float reloadDelay;
        public float range;
        public float damageValue;
    }

    float walkHeight;
    float crouchHeight;
    float proneHeight;

    bool isWalking;

    public byte currentWeaponIndex;

    public Texture2D crossHair;

    public float playerHealth;
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
    RaycastHit interactHit;

    public Inventory currentSelected;
    PlayerStates currentState;
    public Weapons[] weapons;
    public float timeStamp;
    public bool reloadState;

    [FMODUnity.EventRef]
    public string footsteps = "event:/PlayerFootstep";
    FMOD.Studio.EventInstance walkingEv;
    FMOD.Studio.ParameterInstance walkingParam;

    float param;

    // Use this for initialization
    void Start() {
        cam1 = GameObject.Find("Main Camera");
        camTransform = cam1.transform;
        cameraLerpScript = cam1.GetComponent<MoveCamera>();
        lookScript = cam1.GetComponent<SmoothMouseLook>();

        pauseGame = false;
        weaponSelect = false;

        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        ammoText = GameObject.Find("AmmoText");
        reloadText = GameObject.Find("ReloadText");
        reloadText.SetActive(false);
        weaponIcon = GameObject.Find("Weapon Icon");

        healthBar = GameObject.Find("Health Bar");

        weaponWheel = GameObject.Find("Weapon Wheel");
        riflePanel = GameObject.Find("Rifle Panel");
        pistolPanel = GameObject.Find("Pistol Panel");
        knifePanel = GameObject.Find("Knife Panel");
        katanaPanel = GameObject.Find("Katana Panel");
        sniperPanel = GameObject.Find("Sniper Panel");

        playerHealth = 100;
        playerSpeed = 2;
        strafeSlow = 0.5f;
        speedModifier = 1.0f;
        jumpForce = 3.0f;
        gravity = 10.0f;

        crouchHeight = 1.75f;
        walkHeight = 2.5f;
        proneHeight = 1.0f;

        isWalking = false;

        controller = GetComponent<CharacterController>();

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        currentSelected = Inventory.Rifle;
        currentState = PlayerStates.Idle;
        currentWeaponIndex = 0;

        initialVelocity = 1.0f;
        currentVelocity = 0.0f;
        maxVelocity = 5.0f;
        forwardAccelerationRate = 2.5f;
        reverseAccelerationRate = 0.5f;
        deccelerationRate = 4.5f;

        timeStamp = Time.time;
        reloadState = false;

        walkingEv = FMODUnity.RuntimeManager.CreateInstance(footsteps);
        walkingEv.getParameter("Speed", out walkingParam);
        walkingEv.start();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseGame)
        {

        }
        else if (weaponSelect)
        {
            weaponWheel.SetActive(true);

            if (Input.GetKeyUp(KeyCode.Q))
            {
                weaponSelect = false;
            }
        }
        else
        {
            weaponWheel.SetActive(false);

            //Movement Handler for when player is on and off the ground
            if (controller.isGrounded)
            {
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    currentVelocity += forwardAccelerationRate * Time.deltaTime;

                    if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.LeftControl))
                    {
                        currentState = PlayerStates.Walk;
                    }

                    moveDirection = transform.TransformDirection(moveDirection);

                    //Sprinting key (Left Shift)
                    if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.Z))
                    {
                        currentState = PlayerStates.Run;
                    }
                }
                else if ((Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.LeftControl))
                {
                    currentState = PlayerStates.Idle;
                    currentVelocity -= deccelerationRate * Time.deltaTime;
                }

                //Crouch key (Left Control)
                if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.Z))
                {
                    currentState = PlayerStates.Crouch;
                    cameraLerpScript.LerpCamera("B");
                }
                else if (Input.GetKeyUp(KeyCode.LeftControl))
                {
                    cameraLerpScript.LerpCamera("A");
                }

                //Prone key ("Z" key)
                if (Input.GetKey(KeyCode.Z) && !Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.LeftShift))
                {
                    currentState = PlayerStates.Prone;
                    cameraLerpScript.LerpCamera("C");
                }
                else if (Input.GetKeyUp(KeyCode.Z))
                {
                    cameraLerpScript.LerpCamera("A");
                }

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

            currentVelocity = Mathf.Clamp(currentVelocity, initialVelocity, maxVelocity);

            //Player movement modifier
            controller.Move(moveDirection * speedModifier * currentVelocity * Time.deltaTime);
            walkingParam.setValue(speedModifier * currentVelocity);

            //Debug.LogFormat("State:{0}      velo:{1}        param:{2}", currentState, (speedModifier * currentVelocity));

            //Weapon change key handler
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //If current weapon is not alredy the target weapon
                if (currentSelected != Inventory.Rifle)
                {
                    SwitchWeapon(0);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //If current weapon is not alredy the target weapon
                if (currentSelected != Inventory.Pistol)
                {
                    SwitchWeapon(1);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //If current weapon is not alredy the target weapon
                if (currentSelected != Inventory.Knife)
                {
                    SwitchWeapon(2);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha4) && !weapons[3].locked)
            {
                //If current weapon is not alredy the target weapon
                if (currentSelected != Inventory.Katana)
                {
                    SwitchWeapon(3);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha5) && !weapons[4].locked)
            {
                //If current weapon is not alredy the target weapon
                if (currentSelected != Inventory.Sniper)
                {
                    SwitchWeapon(4);
                }
            }

            //Reload key ("R" key)
            if (Input.GetKeyDown(KeyCode.R) && !reloadState)
            {
                reloadState = true;
                timeStamp = Time.time;
            }

            //Left Mouse Button Shoot
            if (Input.GetMouseButton(0))
            {
                if (Time.time > (timeStamp + weapons[currentWeaponIndex].shootDelay) && !reloadState)
                {
                    ShootRay();
                    weapons[currentWeaponIndex].currentAmmo -= 1;
                    gunParticle.Emit(1);
                    timeStamp = Time.time;
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                weaponSelect = true;
            }

            //Check for Reload need
            if (weapons[currentWeaponIndex].currentAmmo <= 0 && !reloadState)
            {
                reloadState = true;
                timeStamp = Time.time;
            }

            if (reloadState)
            {
                reloadText.SetActive(true);
                if (Time.time >= timeStamp + weapons[currentWeaponIndex].reloadDelay)
                {
                    ReloadWeapon();
                }
            }

            //Check current movement state to adjust speed multiplier accordingly
            switch (currentState)
            {
                case PlayerStates.Walk:
                    controller.height = walkHeight;
                    lookScript.minimumY = -80f;
                    lookScript.maximumY = 80f;
                    speedModifier = 1.0f;
                    break;
                case PlayerStates.Run:
                    controller.height = walkHeight;
                    lookScript.minimumY = -80f;
                    lookScript.maximumY = 80f;
                    speedModifier = 2.0f;
                    break;
                case PlayerStates.Crouch:
                    controller.height = crouchHeight;
                    lookScript.minimumY = -40f;
                    lookScript.maximumY = 80f;
                    speedModifier = 0.5f;
                    break;
                case PlayerStates.Prone:
                    controller.height = proneHeight;
                    lookScript.minimumY = 0f;
                    lookScript.maximumY = 40f;
                    speedModifier = 0.25f;
                    break;
                case PlayerStates.Idle:
                    controller.height = walkHeight;
                    lookScript.minimumY = -80f;
                    lookScript.maximumY = 80f;
                    speedModifier = 1.0f;
                    break;
                case PlayerStates.Jump:
                    controller.height = walkHeight;
                    lookScript.minimumY = -80f;
                    lookScript.maximumY = 80f;
                    speedModifier = 1.0f;
                    break;
            }

            Ray ray = new Ray(camTransform.position, camTransform.forward);
            if (Physics.Raycast(ray, out interactHit, 10))
            {
                if (interactHit.collider.tag == "Interactable")
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        interactHit.collider.gameObject.SendMessage("Activate");
                    }
                }
            }

            playerHealth -= 0.05f;
        }
        


        //UI Elements
        //Ammo Text
        ammoText.GetComponent<Text>().text = weapons[currentWeaponIndex].currentAmmo + "|" + weapons[currentWeaponIndex].remainingAmmo;

        //HUD Weapon Icon
        weaponIcon.GetComponent<Image>().sprite = weapons[currentWeaponIndex].weaponSprite;

        //Health Bar
        healthBar.GetComponent<Image>().fillAmount = (playerHealth / 100) * 0.61f;
        healthBar.GetComponent<Image>().fillAmount = Mathf.Clamp(healthBar.GetComponent<Image>().fillAmount, 0f, 0.66f) + 0.05f;

        //Debug.LogFormat("health:{0}        Fill:{1}", playerHealth, healthBar.GetComponent<Image>().fillAmount);

        //Weapon Wheel
        switch (currentSelected)
        {
            case Inventory.Rifle:
                riflePanel.GetComponent<Image>().sprite = weapons[0].selectedPanel;
                pistolPanel.GetComponent<Image>().sprite = weapons[1].unselectedPanel;
                knifePanel.GetComponent<Image>().sprite = weapons[2].unselectedPanel;

                if (weapons[3].locked)
                {
                    katanaPanel.GetComponent<Image>().sprite = weapons[3].lockedPanel;
                }
                else
                {
                    katanaPanel.GetComponent<Image>().sprite = weapons[3].unselectedPanel;
                }

                if (weapons[4].locked)
                {
                    sniperPanel.GetComponent<Image>().sprite = weapons[4].lockedPanel;
                }
                else
                {
                    sniperPanel.GetComponent<Image>().sprite = weapons[4].unselectedPanel;
                }
                break;

            case Inventory.Pistol:
                riflePanel.GetComponent<Image>().sprite = weapons[0].unselectedPanel;
                pistolPanel.GetComponent<Image>().sprite = weapons[1].selectedPanel;
                knifePanel.GetComponent<Image>().sprite = weapons[2].unselectedPanel;

                if (weapons[3].locked)
                {
                    katanaPanel.GetComponent<Image>().sprite = weapons[3].lockedPanel;
                }
                else
                {
                    katanaPanel.GetComponent<Image>().sprite = weapons[3].unselectedPanel;
                }

                if (weapons[4].locked)
                {
                    sniperPanel.GetComponent<Image>().sprite = weapons[4].lockedPanel;
                }
                else
                {
                    sniperPanel.GetComponent<Image>().sprite = weapons[4].unselectedPanel;
                }
                break;

            case Inventory.Knife:
                riflePanel.GetComponent<Image>().sprite = weapons[0].unselectedPanel;
                pistolPanel.GetComponent<Image>().sprite = weapons[1].unselectedPanel;
                knifePanel.GetComponent<Image>().sprite = weapons[2].selectedPanel;

                if (weapons[3].locked)
                {
                    katanaPanel.GetComponent<Image>().sprite = weapons[3].lockedPanel;
                }
                else
                {
                    katanaPanel.GetComponent<Image>().sprite = weapons[3].unselectedPanel;
                }

                if (weapons[4].locked)
                {
                    sniperPanel.GetComponent<Image>().sprite = weapons[4].lockedPanel;
                }
                else
                {
                    sniperPanel.GetComponent<Image>().sprite = weapons[4].unselectedPanel;
                }
                break;

            case Inventory.Katana:
                riflePanel.GetComponent<Image>().sprite = weapons[0].unselectedPanel;
                pistolPanel.GetComponent<Image>().sprite = weapons[1].unselectedPanel;
                knifePanel.GetComponent<Image>().sprite = weapons[2].unselectedPanel;
                katanaPanel.GetComponent<Image>().sprite = weapons[3].selectedPanel;
                sniperPanel.GetComponent<Image>().sprite = weapons[4].unselectedPanel;
                break;

            case Inventory.Sniper:
                riflePanel.GetComponent<Image>().sprite = weapons[0].unselectedPanel;
                pistolPanel.GetComponent<Image>().sprite = weapons[1].unselectedPanel;
                knifePanel.GetComponent<Image>().sprite = weapons[2].unselectedPanel;
                katanaPanel.GetComponent<Image>().sprite = weapons[3].unselectedPanel;
                sniperPanel.GetComponent<Image>().sprite = weapons[4].selectedPanel;
                break;
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
            if (hit.collider.tag == "Mutant")
            {
                hit.collider.gameObject.SendMessage("TakeDamage", weapons[currentWeaponIndex].damageValue);
            }
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
        print("die");
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

        reloadState = false;
        reloadText.SetActive(false);
    }

    public void SwitchWeapon(int target)
    {
        Debug.Log(target);
        if (target == 0)
        {
            currentSelected = Inventory.Rifle;
            currentWeaponIndex = 0;
            reloadState = false;
            timeStamp = Time.time;
        }
        else if (target == 1)
        {
            currentSelected = Inventory.Pistol;
            currentWeaponIndex = 1;
            reloadState = false;
            timeStamp = Time.time;
        }
        else if (target == 2)
        {
            currentSelected = Inventory.Knife;
            currentWeaponIndex = 2;
            reloadState = false;
            timeStamp = Time.time;
        }
        else if (target == 3 && !weapons[3].locked)
        {
            currentSelected = Inventory.Katana;
            currentWeaponIndex = 3;
            reloadState = false;
            timeStamp = Time.time;
        }
        else if (target == 4 && !weapons[4].locked)
        {
            currentSelected = Inventory.Sniper;
            currentWeaponIndex = 4;
            reloadState = false;
            timeStamp = Time.time;
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
