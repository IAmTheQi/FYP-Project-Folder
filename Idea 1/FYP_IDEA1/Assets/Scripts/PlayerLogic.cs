using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(CollectableLogic))]
public class PlayerLogic : MonoBehaviour {

    GameObject gameController;
    SettingsConfig settingsScript;

    GameObject cam1;
    GameObject focusCam;
    GameObject scopeCam;
    GameObject scopeFocusCam;
    SmoothMouseLook lookScript;
    Transform camTransform;
    Vector3 startPosition;

    CollectableLogic collectScript;

    public ParticleSystem gunParticle;
    public ParticleSystem pistolParticle;

    public bool pauseGame;
    public bool weaponSelect;
    public bool itemView;
    public bool inspectView;
    public bool optionsView;

    public bool aimDownSight;

    GameObject ammoText;
    GameObject reloadText;
    GameObject weaponIcon;

    GameObject hitMarker;

    GameObject healthBar;

    GameObject bloodOverlay;
    Color bloodAlpha;
    GameObject shadowOverlay;
    Color shadowAlpha;

    GameObject weaponWheel;
    GameObject riflePanel;
    GameObject pistolPanel;
    GameObject knifePanel;
    GameObject katanaPanel;
    GameObject sniperPanel;

    GameObject itemMenu;
    GameObject pauseMenu;
    GameObject optionsMenu;

    public GameObject rifleObject;
    public GameObject rifleSight;
    public GameObject rifleHip;

    public GameObject pistolObject;
    public GameObject knifeObject;


    float lerpStart;
    float lerpTime;

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

    public float walkHeight;
    public float crouchHeight;
    public float proneHeight;

    bool isWalking;

    public bool focus;

    public byte currentWeaponIndex;

    public float playerHealth;
    public float playerSpeed;
    public float strafeSlow;
    public float jumpForce;
    public float gravity;

    public float speedModifier;
    public float runModifier;
    public float walkModifier;
    public float crouchModifier;
    public float proneModifier;

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

    float regenTimer;
    float regenDelay;
    bool regenHealth;

    [FMODUnity.EventRef]
    public string footsteps = "event:/PlayerFootstep";
    FMOD.Studio.EventInstance walkingEv;
    FMOD.Studio.ParameterInstance walkingParam;
    public FMOD.Studio.ParameterInstance surfaceParam;

    [FMODUnity.EventRef]
    public string panting = "event:/PlayerPant";
    FMOD.Studio.EventInstance pantingEv;
    FMOD.Studio.ParameterInstance pantingParam;

    [FMODUnity.EventRef]
    public string rifleSound = "event:/Rifle";

    [FMODUnity.EventRef]
    public string pistolSound = "event:/Pistol";

    [FMODUnity.EventRef]
    public string knifeSound = "event:/Knife";


    float param;

    // Use this for initialization
    void Start() {
        gameController = GameObject.Find("GameController");
        settingsScript = gameController.GetComponent<SettingsConfig>();

        cam1 = GameObject.Find("Main Camera");
        focusCam = GameObject.Find("FocusCamera");
        scopeCam = GameObject.Find("ScopeCamera");
        scopeFocusCam = GameObject.Find("ScopeFocusCamera");
        camTransform = cam1.transform;
        lookScript = cam1.GetComponent<SmoothMouseLook>();

        collectScript = GetComponent<CollectableLogic>();

        pauseGame = false;
        weaponSelect = false;
        itemView = false;
        inspectView = false;

        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        aimDownSight = false;

        hitMarker = GameObject.Find("HitMarker");
        hitMarker.SetActive(false);

        ammoText = GameObject.Find("AmmoText");
        reloadText = GameObject.Find("ReloadText");
        reloadText.SetActive(false);
        weaponIcon = GameObject.Find("Weapon Icon");

        healthBar = GameObject.Find("Health Bar");

        bloodOverlay = GameObject.Find("BloodOverlay");
        bloodAlpha = bloodOverlay.GetComponent<Image>().color;
        shadowOverlay = GameObject.Find("ShadowOverlay");
        shadowAlpha = shadowOverlay.GetComponent<Image>().color;

        weaponWheel = GameObject.Find("Weapon Wheel");
        riflePanel = GameObject.Find("Rifle Panel");
        pistolPanel = GameObject.Find("Pistol Panel");
        knifePanel = GameObject.Find("Knife Panel");
        katanaPanel = GameObject.Find("Katana Panel");
        sniperPanel = GameObject.Find("Sniper Panel");

        itemMenu = GameObject.Find("ItemSelect");
        itemMenu.SetActive(false);
        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);
        optionsMenu = GameObject.Find("OptionsMenu");
        optionsMenu.SetActive(false);

        lerpStart = 0f;
        lerpTime = 0.2f;

        playerHealth = settingsScript.playerHealth;
        playerSpeed = settingsScript.playerSpeed;
        strafeSlow = settingsScript.strafeSlow;
        jumpForce = settingsScript.jumpForce;
        gravity = settingsScript.gravity;

        speedModifier = settingsScript.walkModifier;
        runModifier = settingsScript.runModifier;
        walkModifier = settingsScript.walkModifier;
        crouchModifier = settingsScript.crouchModifier;
        proneModifier = settingsScript.proneModifier;

        initialVelocity = settingsScript.initialVelocity;
        currentVelocity = settingsScript.currentVelocity;
        maxVelocity = settingsScript.maxVelocity;
        forwardAccelerationRate = settingsScript.forwardAccelerationRate;
        reverseAccelerationRate = settingsScript.reverseAccelerationRate;
        deccelerationRate = settingsScript.deccelerationRate;

        walkHeight = settingsScript.walkHeight;
        crouchHeight = settingsScript.crouchHeight;
        proneHeight = settingsScript.proneHeight;

        isWalking = false;

        focus = false;

        controller = GetComponent<CharacterController>();

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        currentSelected = Inventory.Rifle;
        currentState = PlayerStates.Idle;
        currentWeaponIndex = 0;

        timeStamp = Time.time;
        reloadState = false;

        regenTimer = 0.0f;
        regenDelay = 5.0f;
        regenHealth = false;

        walkingEv = FMODUnity.RuntimeManager.CreateInstance(footsteps);
        walkingEv.getParameter("Speed", out walkingParam);
        walkingEv.getParameter("Surface", out surfaceParam);
        walkingEv.start();

        pantingEv = FMODUnity.RuntimeManager.CreateInstance(panting);
        pantingEv.getParameter("Panting", out pantingParam);
        pantingEv.start();
        pantingParam.setValue(0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseGame)
        {
            pauseMenu.SetActive(true);

            //Pause Key
            if (Input.GetKeyDown(KeyCode.Escape) && !itemView)
            {
                pauseGame = false;
            }

            if (itemView)
            {
                itemMenu.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F) && !inspectView)
                {
                    ButtonTrigger("inspect");
                }

                if (inspectView)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        collectScript.InspectItem();
                        inspectView = false;
                    }
                }
                else if (!inspectView)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        itemView = false;
                    }
                }
            }
            else if (!itemView)
            {
                itemMenu.SetActive(false);
            }
        }
        else if (weaponSelect)
        {
            weaponWheel.SetActive(true);

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                weaponSelect = false;
            }
        }
        else
        {
            weaponWheel.SetActive(false);
            pauseMenu.SetActive(false);
            itemMenu.SetActive(false);

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
                    moveDirection = new Vector3(0, 0, 0);
                    moveDirection = transform.TransformDirection(moveDirection);

                    currentState = PlayerStates.Idle;
                    currentVelocity -= deccelerationRate * Time.deltaTime;
                }

                //Crouch key (Left Control)
                if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.Z))
                {
                    currentState = PlayerStates.Crouch;
                }
                else if (Input.GetKeyUp(KeyCode.LeftControl))
                {

                }

                //Prone key ("Z" key)
                if (Input.GetKey(KeyCode.Z) && !Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.LeftShift))
                {
                    currentState = PlayerStates.Prone;
                }
                else if (Input.GetKeyUp(KeyCode.Z))
                {

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

                    if (currentWeaponIndex == 0)
                    {
                        FMODUnity.RuntimeManager.PlayOneShot(rifleSound);
                        gunParticle.Emit(1);
                    }
                    else if (currentWeaponIndex == 1)
                    {
                        FMODUnity.RuntimeManager.PlayOneShot(pistolSound);
                        pistolParticle.Emit(1);
                    }
                    else if (currentWeaponIndex == 2)
                    {
                        FMODUnity.RuntimeManager.PlayOneShot(knifeSound);
                    }
                    timeStamp = Time.time;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                lerpStart = 0f;
                aimDownSight = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                lerpStart = 0f;
                aimDownSight = false;
            }

            //Weapon select key
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                weaponSelect = true;
            }

            //Collectable item menu
            if (Input.GetKeyDown(KeyCode.I))
            {
                itemView = true;
            }

            //Pause Key
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseGame = true;
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

            //Health Handler
            if (playerHealth < 100.0f)
            {
                if (regenTimer < regenDelay)
                {
                    regenTimer += Time.deltaTime;
                }
                else
                {
                    regenHealth = true;
                }
            }

            if (playerHealth < 50.0f)
            {
                bloodAlpha = bloodOverlay.GetComponent<Image>().color;
                bloodAlpha.a += 0.05f;
            }
            else if (playerHealth > 50.0f)
            {
                bloodAlpha = bloodOverlay.GetComponent<Image>().color;
                bloodAlpha.a -= 0.05f;
            }

            if (regenHealth)
            {
                playerHealth += 1.0f;
                if (playerHealth > 100)
                {
                    playerHealth = 100.0f;
                    regenTimer = 0.0f;
                    regenHealth = false;
                }
            }

            //Focus
            if (focus)
            {
                shadowAlpha = shadowOverlay.GetComponent<Image>().color;
                shadowAlpha.a += 0.05f;
                focusCam.SetActive(true);
            }
            else
            {
                shadowAlpha.a -= 0.05f;
                focusCam.SetActive(false);
            }

            //Check current movement state to adjust speed multiplier accordingly
            switch (currentState)
            {
                case PlayerStates.Walk:
                    controller.height = walkHeight;
                    lookScript.minimumY = -80f;
                    lookScript.maximumY = 80f;
                    speedModifier = settingsScript.walkModifier;
                    pantingParam.setValue(0.0f);
                    break;
                case PlayerStates.Run:
                    controller.height = walkHeight;
                    lookScript.minimumY = -80f;
                    lookScript.maximumY = 80f;
                    speedModifier = settingsScript.runModifier;
                    pantingParam.setValue(1.0f);
                    break;
                case PlayerStates.Crouch:
                    controller.height = crouchHeight;
                    lookScript.minimumY = -40f;
                    lookScript.maximumY = 80f;
                    speedModifier = settingsScript.crouchModifier;
                    pantingParam.setValue(0.0f);
                    break;
                case PlayerStates.Prone:
                    controller.height = proneHeight;
                    lookScript.minimumY = 0f;
                    lookScript.maximumY = 40f;
                    speedModifier = settingsScript.proneModifier;
                    pantingParam.setValue(0.0f);
                    break;
                case PlayerStates.Idle:
                    controller.height = walkHeight;
                    lookScript.minimumY = -80f;
                    lookScript.maximumY = 80f;
                    speedModifier = settingsScript.walkModifier;
                    pantingParam.setValue(0.0f);
                    break;
                case PlayerStates.Jump:
                    controller.height = walkHeight;
                    lookScript.minimumY = -80f;
                    lookScript.maximumY = 80f;
                    pantingParam.setValue(0.0f);
                    break;
            }

            //Item Interaction & Collection
            

            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray ray = new Ray(camTransform.position, camTransform.forward);
                if (Physics.Raycast(ray, out interactHit, 5))
                {
                    Debug.Log(interactHit.collider.name);
                    if (interactHit.collider.tag == "Interactable")
                    {
                        interactHit.collider.gameObject.SendMessage("Activate");
                    }
               
                    if (interactHit.collider.tag == "Collectable")
                    {
                        collectScript.CollectItem(interactHit.collider.gameObject);
                    }
                }
            }
        }
        


        //UI Elements
        //Ammo Text
        ammoText.GetComponent<Text>().text = weapons[currentWeaponIndex].currentAmmo + "|" + weapons[currentWeaponIndex].remainingAmmo;

        //HUD Weapon Icon
        weaponIcon.GetComponent<Image>().sprite = weapons[currentWeaponIndex].weaponSprite;

        //Health Bar
        healthBar.GetComponent<Image>().fillAmount = (playerHealth / 100) * 0.61f;
        healthBar.GetComponent<Image>().fillAmount = Mathf.Clamp(healthBar.GetComponent<Image>().fillAmount, 0f, 0.66f) + 0.05f;

        //Shadow and Blood Overlay
        bloodAlpha.a = Mathf.Clamp(bloodAlpha.a, 0.0f, 1.0f);
        bloodOverlay.GetComponent<Image>().color = bloodAlpha;
        shadowAlpha.a = Mathf.Clamp(shadowAlpha.a, 0.0f, 0.8f);
        shadowOverlay.GetComponent<Image>().color = shadowAlpha;

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

                if (aimDownSight)
                {
                    lerpStart += Time.deltaTime;
                    if (lerpStart >= lerpTime)
                    {
                        lerpStart = lerpTime;
                    }
                    rifleObject.transform.position = Vector3.Lerp(rifleHip.transform.position, rifleSight.transform.position, lerpStart / lerpTime);

                    if (focus)
                    {
                        focusCam.SetActive(false);
                    }
                }

                if (!aimDownSight)
                {
                    lerpStart += Time.deltaTime;
                    if (lerpStart >= lerpTime)
                    {
                        lerpStart = lerpTime;
                    }
                    rifleObject.transform.position = Vector3.Lerp(rifleSight.transform.position, rifleHip.transform.position, lerpStart / lerpTime);

                    if (!focus)
                    {
                        focusCam.SetActive(true);
                    }
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
            Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
            if (hit.collider.tag == "Mutant")
            {
                hit.collider.gameObject.SendMessage("TakeDamage", weapons[currentWeaponIndex].damageValue);
                StartCoroutine(Blink());
            }
            else if (hit.collider.tag == "MutantHead")
            {
                hit.collider.transform.parent.gameObject.SendMessage("TakeDamage", weapons[currentWeaponIndex].damageValue * 100);
                StartCoroutine(Blink());
            }
            else if (hit.collider.tag == "MutantBack" && currentWeaponIndex == 3)
            {
                hit.collider.transform.parent.gameObject.SendMessage("TakeDamage", weapons[currentWeaponIndex].damageValue * 100);
            }
            Debug.Log(hit.collider.name);
        }
        GunNoise();
    }

    void GunNoise()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 20);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.GetComponent<MutantOne>() != null)
            {
                hitColliders[i].SendMessage("RecordLastSeen", transform);
                hitColliders[i].SendMessage("LosePlayer");
            }
            i++;
        }
    }

    IEnumerator Blink()
    {
        Debug.Log("hold");
        hitMarker.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitMarker.SetActive(false);
    }

    //Enemy detection
    void OnControllerColliderHit(ControllerColliderHit collider)
    {
        /*if (collider.gameObject.tag == "Mutant")
        {
            KillPlayer();
        }*/
    }

    public void TakeDamage(float value)
    {
        playerHealth -= value;
        regenTimer = 0.0f;
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

            rifleObject.SetActive(true);
            pistolObject.SetActive(false);
            knifeObject.SetActive(false);

            timeStamp = Time.time;
        }
        else if (target == 1)
        {
            currentSelected = Inventory.Pistol;
            currentWeaponIndex = 1;
            reloadState = false;

            rifleObject.SetActive(false);
            pistolObject.SetActive(true);
            knifeObject.SetActive(false);

            timeStamp = Time.time;
        }
        else if (target == 2)
        {
            currentSelected = Inventory.Knife;
            currentWeaponIndex = 2;
            reloadState = false;

            rifleObject.SetActive(false);
            pistolObject.SetActive(false);
            knifeObject.SetActive(true);

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
    
    public void ChangeSurface(string target)
    {
        if (target == "Concrete")
        {
            surfaceParam.setValue(1.0f);
        }
        else if (target == "Dirt")
        {
            surfaceParam.setValue(2.0f);
        }
        else if (target == "Wood")
        {
            surfaceParam.setValue(3.0f);
        }
    }

    public void ButtonTrigger(string target)
    {
        switch (target)
        {
            case "artifacts":
                itemView = !itemView;
                break;

            case "pause":
                pauseGame = !pauseGame;
                break;

            case "inspect":
                if (!collectScript.Empty())
                {
                    inspectView = !inspectView;
                    collectScript.InspectItem();
                }
                break;
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
