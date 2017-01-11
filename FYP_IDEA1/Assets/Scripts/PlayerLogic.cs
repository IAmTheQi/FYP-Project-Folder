using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent (typeof(CollectableLogic))]
public class PlayerLogic : MonoBehaviour {

    GameObject gameController;
    SettingsConfig settingsScript;
    GunShotFeedbackHandler gunShotScript;

    GameObject cam1;
    GameObject focusCam;
    GameObject leanPivot;
    public Texture scopeTexture;
    SmoothMouseLook lookScript;
    Transform camTransform;
    Vector3 startPosition;

    GameObject flashlightObject;

    GameObject crosshairLeft;
    GameObject crosshairRight;
    GameObject crosshairTop;
    GameObject crosshairBottom;
    float crosshairScale;
    float spreadFactor;

    CollectableLogic collectScript;

    public ParticleSystem gunParticle;
    public ParticleSystem pistolParticle;

    public GameObject rifleMuzzle;
    public GameObject pistolMuzzle;

    bool tutorialPrompt;

    public bool pauseGame;
    public bool itemView;
    public bool inspectView;
    public bool optionsView;
    public bool quickInspect;

    public bool aimDownSight;

    GameObject ammoText;
    GameObject reloadText;
    GameObject weaponIcon;

    GameObject hitMarker;

    GameObject healthBar;
    GameObject[] rifleBullets;
    GameObject[] pistolBullets;

    GameObject bloodOverlay;
    Color bloodAlpha;
    GameObject shadowOverlay;
    Color shadowAlpha;

    GameObject itemMenu;
    GameObject pauseMenu;
    GameObject optionsMenu;

    public GameObject rifleObject;
    public GameObject rifleSight;
    public GameObject rifleHip;

    public GameObject pistolObject;
    public GameObject pistolSight;
    public GameObject pistolHip;

    public GameObject knifeObject;

    Animator rifleAnimator;
    Animator knifeAnimator;
    Animator pistolAnimator;

    float lerpStart;
    float lerpTime;

    public float alertRange;

    enum PlayerStates
    {
        Run,
        Walk,
        Idle,
        Jump
    }
    
    [System.Serializable]
    public struct Weapons
    {
        public string weaponName;
        public GameObject weaponUI;
        public Sprite fullBullet;
        public Sprite emptyBullet;
        public bool locked;
        public int currentAmmo;
        public int magazineSize;
        public int remainingAmmo;
        public int totalAmmo;
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
    bool playerCrouch;

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
    RaycastHit aimHit;

    RaycastHit surfaceHit;
    
    PlayerStates currentState;
    public Weapons[] weapons;
    public float timeStamp;
    public bool reloadState;

    float regenTimer;
    float regenDelay;
    bool regenHealth;
    bool dead;

    [FMODUnity.EventRef]
    public string footsteps = "event:/PlayerFootstep";
    FMOD.Studio.EventInstance walkingEv;
    FMOD.Studio.ParameterInstance walkingParam;
    public FMOD.Studio.ParameterInstance surfaceParam;

    [FMODUnity.EventRef]
    public string rifleSound = "event:/Rifle";

    [FMODUnity.EventRef]
    public string rifleReloadSound = "event:/RifleReload";

    [FMODUnity.EventRef]
    public string pistolSound = "event:/Pistol";

    [FMODUnity.EventRef]
    public string knifeSound = "event:/Knife";


    float param;

    // Use this for initialization
    void Start() {
        gameController = GameObject.Find("GameController");
        settingsScript = gameController.GetComponent<SettingsConfig>();
        gunShotScript = gameController.GetComponent<GunShotFeedbackHandler>();

        cam1 = GameObject.Find("Main Camera");
        focusCam = GameObject.Find("FocusCamera");
        leanPivot = GameObject.Find("LeanPivot");
        camTransform = cam1.transform;
        lookScript = cam1.GetComponent<SmoothMouseLook>();

        crosshairLeft = GameObject.Find("CrosshairLeft");
        crosshairRight = GameObject.Find("CrosshairRight");
        crosshairTop = GameObject.Find("CrosshairTop");
        crosshairBottom = GameObject.Find("CrosshairBottom");
        crosshairScale = 0f;
        spreadFactor = 0f;

        flashlightObject = GameObject.Find("Flashlight");

        collectScript = GetComponent<CollectableLogic>();

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            tutorialPrompt = true;
        }

        pauseGame = false;
        itemView = false;
        inspectView = false;
        quickInspect = false;

        rifleAnimator = rifleObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        knifeAnimator = knifeObject.GetComponent<Animator>();
        pistolAnimator = pistolObject.GetComponent<Animator>();

        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        aimDownSight = false;

        hitMarker = GameObject.Find("HitMarker");
        hitMarker.SetActive(false);

        ammoText = GameObject.Find("AmmoText");
        reloadText = GameObject.Find("ReloadText");
        reloadText.SetActive(false);
        weaponIcon = GameObject.Find("Weapon Icon");

        healthBar = GameObject.Find("Health Bar");
        rifleBullets = new GameObject[30];
        for (int i = 0; i < rifleBullets.Length; i++)
        {
            rifleBullets[i] = GameObject.Find("Bullet " + i);
            rifleBullets[i].GetComponent<Image>().sprite = weapons[0].fullBullet;
        }

        pistolBullets = new GameObject[12];
        for (int j = 0; j < pistolBullets.Length; j++)
        {
            pistolBullets[j] = GameObject.Find("PBullet " + j);
            pistolBullets[j].GetComponent<Image>().sprite = weapons[1].fullBullet;
        }

        weapons[1].weaponUI.SetActive(false);

        bloodOverlay = GameObject.Find("BloodOverlay");
        bloodAlpha = bloodOverlay.GetComponent<Image>().color;
        shadowOverlay = GameObject.Find("ShadowOverlay");
        shadowAlpha = shadowOverlay.GetComponent<Image>().color;

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
        playerCrouch = false;

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
        
        currentState = PlayerStates.Idle;
        currentWeaponIndex = 0;

        timeStamp = Time.time;
        reloadState = false;

        regenTimer = 0.0f;
        regenDelay = 5.0f;
        regenHealth = false;
        dead = false;

        walkingEv = FMODUnity.RuntimeManager.CreateInstance(footsteps);
        walkingEv.getParameter("Speed", out walkingParam);
        walkingEv.getParameter("Surface", out surfaceParam);
        walkingEv.start();
        walkingParam.setValue(0.0f);
        surfaceParam.setValue(0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SaveLoad.Save();
        }

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

                if (inspectView && !quickInspect)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        collectScript.InspectItem();
                        inspectView = false;
                    }
                }
                else if (inspectView && quickInspect)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        collectScript.InspectItem();
                        inspectView = false;
                        itemView = false;
                        pauseGame = false;
                        quickInspect = false;
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
        else
        {
            pauseMenu.SetActive(false);
            itemMenu.SetActive(false);
            //Movement Handler for when player is on and off the ground
            if (controller.isGrounded)
            {
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    currentVelocity += forwardAccelerationRate * Time.deltaTime;
                    moveDirection = transform.TransformDirection(moveDirection);

                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        currentState = PlayerStates.Walk;
                    }


                    //Sprinting key (Left Shift)
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        currentState = PlayerStates.Run;
                    }

                    Ray surfaceRay = new Ray(transform.position, -transform.up);

                    if (Physics.Raycast(surfaceRay, out surfaceHit, controller.height))
                    {
                        //Debug.LogFormat("name:{0}       tag:{1}",surfaceHit.collider.name, surfaceHit.collider.tag);
                        if (surfaceHit.collider.tag == "Concrete")
                        {
                            ChangeSurface("Concrete");
                        }
                        else if (surfaceHit.collider.tag == "Wood")
                        {
                            ChangeSurface("Wood");
                        }
                        else if (surfaceHit.collider.tag == "Dirt")
                        {
                            ChangeSurface("Dirt");
                        }
                        else
                        {

                        }

                        if (surfaceHit.collider.name != "Area1Foundation")
                        {

                        }
                    }
                }
                else if ((Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) && !Input.GetKey(KeyCode.LeftShift))
                {
                    moveDirection = new Vector3(0, 0, 0);
                    moveDirection = transform.TransformDirection(moveDirection);

                    currentState = PlayerStates.Idle;
                    currentVelocity -= deccelerationRate * Time.deltaTime;
                }

                //Jumping (Space)
                if (Input.GetKeyDown(KeyCode.Space))
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

            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0)
            {
                walkingParam.setValue(speedModifier * currentVelocity);
                //Debug.Log(speedModifier * currentVelocity);
            }
            else
            {
                walkingParam.setValue(0);
            }
            //Weapon change key handler
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //If current weapon is not alredy the target weapon
                if (currentWeaponIndex != 0)
                {
                    if (currentWeaponIndex == 1)
                    {
                        pistolAnimator.SetTrigger("Swapping");
                        StartCoroutine(SwitchWeapon(0));
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //If current weapon is not alredy the target weapon
                if (currentWeaponIndex != 1)
                {
                    if (currentWeaponIndex == 0)
                    {
                        rifleAnimator.SetTrigger("Swapping");
                        StartCoroutine(SwitchWeapon(1));
                    }
                }
            }

            //Flashlight Toggle
            if (Input.GetKeyDown(KeyCode.T))
            {
                flashlightObject.SetActive(!flashlightObject.activeInHierarchy);
            }

            //Reload key ("R" key)
            if (Input.GetKeyDown(KeyCode.R) && !reloadState && weapons[currentWeaponIndex].remainingAmmo > 0)
            {
                reloadState = true;
                timeStamp = Time.time;

                if (currentWeaponIndex == 0)
                {
                    rifleAnimator.SetBool("Firing", false);
                    rifleAnimator.SetTrigger("Reload");
                    FMODUnity.RuntimeManager.PlayOneShot(rifleReloadSound);
                }
                else if (currentWeaponIndex == 1)
                {
                    pistolAnimator.SetBool("Firing", false);
                    pistolAnimator.SetTrigger("Reload");
                }
            }

            //Left Mouse Button Shoot
            if (Input.GetMouseButton(0) && currentState != PlayerStates.Run)
            {
                if (Time.time > (timeStamp + weapons[currentWeaponIndex].shootDelay) && !reloadState)
                {
                    if (currentWeaponIndex != 2 && weapons[currentWeaponIndex].currentAmmo > 0)
                    {
                        ShootRay();
                        crosshairScale += 25f;
                        spreadFactor += 0.1f;
                        if (currentWeaponIndex == 0)
                        {
                            rifleAnimator.SetBool("Firing", true);
                            FMODUnity.RuntimeManager.PlayOneShot(rifleSound);
                            gunParticle.Emit(1);
                            StartCoroutine(MuzzleFlash(rifleMuzzle));
                            rifleBullets[weapons[0].currentAmmo - 1].GetComponent<Image>().sprite = weapons[0].emptyBullet;
                        }
                        else if (currentWeaponIndex == 1)
                        {
                            pistolAnimator.SetBool("Firing", true);
                            FMODUnity.RuntimeManager.PlayOneShot(pistolSound);
                            pistolParticle.Emit(1);
                            StartCoroutine(MuzzleFlash(pistolMuzzle));
                            pistolBullets[weapons[1].currentAmmo - 1].GetComponent<Image>().sprite = weapons[1].emptyBullet;
                        }
                        weapons[currentWeaponIndex].currentAmmo -= 1;

                    }

                    timeStamp = Time.time;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (currentWeaponIndex == 0)
                {
                    rifleAnimator.SetBool("Firing", false);
                }
                else if (currentWeaponIndex == 1)
                {
                    pistolAnimator.SetBool("Firing", false);
                }
            }


            if (Input.GetMouseButton(1) && currentState != PlayerStates.Run)
            {
                if (!aimDownSight && !reloadState)
                {
                    lerpStart = 0f;
                    aimDownSight = true;

                    crosshairLeft.SetActive(false);
                    crosshairRight.SetActive(false);
                    crosshairTop.SetActive(false);
                    crosshairBottom.SetActive(false);
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                if (aimDownSight)
                {
                    lerpStart = 0f;
                    aimDownSight = false;

                    crosshairLeft.SetActive(true);
                    crosshairRight.SetActive(true);
                    crosshairTop.SetActive(true);
                    crosshairBottom.SetActive(true);
                }
            }


            //Scoped variables
            if (aimDownSight)
            {
                lerpStart += Time.deltaTime;
                if (lerpStart >= lerpTime)
                {
                    lerpStart = lerpTime;
                }

                spreadFactor = Mathf.Clamp(spreadFactor, 0f, 1f);
            }

            if (!aimDownSight)
            {
                lerpStart += Time.deltaTime;
                if (lerpStart >= lerpTime)
                {
                    lerpStart = lerpTime;
                }

                spreadFactor = Mathf.Clamp(spreadFactor, 0f, 0.2f);
            }

            //Collectable item menu
            if (Input.GetKeyDown(KeyCode.I))
            {
                itemView = true;
            }

            //Pause Key
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (collectScript.IsPrompting())
                {
                    collectScript.SetSelected();
                }
                else if (!collectScript.IsPrompting() && !tutorialPrompt)
                {
                    pauseGame = true;
                }
            }

            //Check for Reload need
            if (weapons[currentWeaponIndex].currentAmmo <= 0 && !reloadState && weapons[currentWeaponIndex].remainingAmmo > 0)
            {
                reloadState = true;
                timeStamp = Time.time;

                if (currentWeaponIndex == 0)
                {
                    rifleAnimator.SetBool("Firing", false);
                    rifleAnimator.SetTrigger("Reload");
                    FMODUnity.RuntimeManager.PlayOneShot(rifleReloadSound);
                }
                else if (currentWeaponIndex == 1)
                {
                    pistolAnimator.SetBool("Firing", false);
                    pistolAnimator.SetTrigger("Reload");
                }
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
                if (playerHealth <= 0)
                {
                    playerHealth = 0;
                    StartCoroutine(KillPlayer());
                }
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
                    break;
                case PlayerStates.Run:
                    controller.height = walkHeight;
                    lookScript.minimumY = -80f;
                    lookScript.maximumY = 80f;
                    speedModifier = settingsScript.runModifier;
                    break;
                case PlayerStates.Idle:
                    controller.height = walkHeight;
                    lookScript.minimumY = -80f;
                    lookScript.maximumY = 80f;
                    speedModifier = settingsScript.walkModifier;
                    break;
                case PlayerStates.Jump:
                    controller.height = crouchHeight;
                    lookScript.minimumY = -80f;
                    lookScript.maximumY = 80f;

                    if (currentWeaponIndex == 0)
                    {
                        rifleAnimator.SetTrigger("Jump");
                    }
                    else if (currentWeaponIndex == 1)
                    {
                        pistolAnimator.SetTrigger("Jump");
                    }
                    break;
            }

            //Item Interaction & Collection
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (collectScript.IsPrompting())
                {
                    pauseGame = true;
                    itemView = true;
                    inspectView = true;
                    collectScript.SetSelected();
                    collectScript.InspectItem();
                    quickInspect = true;
                    Debug.Log(pauseGame + "," + itemView + "," + inspectView);
                }
                else
                {
                    Ray ray = new Ray(camTransform.position, camTransform.forward);
                    if (Physics.Raycast(ray, out interactHit, 7))
                    {
                        Debug.Log(interactHit.collider.gameObject.name);
                        if (interactHit.collider.tag == "Interactable")
                        {
                            interactHit.collider.gameObject.SendMessage("Activate");
                        }

                        if (interactHit.collider.tag == "Collectable")
                        {
                            collectScript.CollectItem(interactHit.collider.gameObject);
                        }

                        if (interactHit.collider.tag == "PistolBox")
                        {
                            weapons[1].remainingAmmo += 12;
                            Destroy(interactHit.collider.gameObject);
                        }
                        else if (interactHit.collider.tag == "PistolAmmo")
                        {
                            weapons[1].remainingAmmo += 4;
                            Destroy(interactHit.collider.gameObject);
                        }

                        if (interactHit.collider.tag == "RifleBox")
                        {
                            weapons[0].remainingAmmo += 30;
                            Destroy(interactHit.collider.gameObject);
                        }
                        else if (interactHit.collider.tag == "RifleAmmo")
                        {
                            weapons[0].remainingAmmo += 7;
                            Destroy(interactHit.collider.gameObject);
                        }
                    }
                }
            }
        }

        
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        //UI Elements
        //Ammo Text
        ammoText.GetComponent<Text>().text = weapons[currentWeaponIndex].remainingAmmo.ToString();

        //Health Bar
        healthBar.GetComponent<Image>().fillAmount = playerHealth / 100;

        //Shadow and Blood Overlay
        bloodAlpha.a = Mathf.Clamp(bloodAlpha.a, 0.0f, 1.0f);
        bloodOverlay.GetComponent<Image>().color = bloodAlpha;
        shadowAlpha.a = Mathf.Clamp(shadowAlpha.a, 0.0f, 0.8f);
        shadowOverlay.GetComponent<Image>().color = shadowAlpha;

        //Crosshair Clamp
        crosshairScale -= 1f;
        crosshairScale = Mathf.Clamp(crosshairScale, 0f, 70f);

        //Shoot Spread
        spreadFactor -= 0.001f;

        if (!aimDownSight)
        {
            crosshairLeft.transform.localPosition = new Vector3(-crosshairScale, 0, 0);
            crosshairRight.transform.localPosition = new Vector3(crosshairScale, 0, 0);
            crosshairTop.transform.localPosition = new Vector3(0, crosshairScale, 0);
            crosshairBottom.transform.localPosition = new Vector3(0, -crosshairScale, 0);
        }

        switch (currentWeaponIndex)
        {
            case 0:
                if (currentState == PlayerStates.Run)
                {
                    rifleAnimator.SetBool("Sprinting", true);
                }
                else
                {
                    rifleAnimator.SetBool("Sprinting", false);
                }

                if (aimDownSight)
                {
                    rifleObject.transform.position = Vector3.Lerp(rifleHip.transform.position, rifleSight.transform.position, lerpStart / lerpTime);
                    rifleAnimator.SetBool("Scoped", true);
                }

                if (!aimDownSight)
                {
                    rifleObject.transform.position = Vector3.Lerp(rifleSight.transform.position, rifleHip.transform.position, lerpStart / lerpTime);
                    rifleAnimator.SetBool("Scoped", false);
                }
                break;

            case 1:
                if (currentState == PlayerStates.Run)
                {
                    pistolAnimator.SetBool("Sprinting", true);
                }
                else
                {
                    pistolAnimator.SetBool("Sprinting", false);
                }

                if (aimDownSight)
                {
                    pistolObject.transform.position = Vector3.Lerp(pistolHip.transform.position, pistolSight.transform.position, lerpStart / lerpTime);
                }

                if (!aimDownSight)
                {
                    pistolObject.transform.position = Vector3.Lerp(pistolSight.transform.position, pistolHip.transform.position, lerpStart / lerpTime);
                }
                break;
        }
    }

    //Shooting raycast check
    void ShootRay()
    {
        //Fires ray from camera, centre of screen into 3D space
        Vector3 shootDirection = camTransform.forward;
        shootDirection.z += (Random.Range(-spreadFactor, spreadFactor))/10;
        shootDirection.y += (Random.Range(-spreadFactor, spreadFactor))/10;

        Ray ray = new Ray(camTransform.position, shootDirection);
        

        if (Physics.Raycast(ray, out hit, weapons[currentWeaponIndex].range))
        {
            if (hit.collider.tag == "Mutant" || hit.collider.tag == "MutantBack")
            {
                hit.collider.gameObject.SendMessage("TakeDamage", weapons[currentWeaponIndex].damageValue);
                StartCoroutine(Blink());
                gunShotScript.CreateMark(true, hit);
            }
            else if (hit.collider.tag == "MutantHead")
            {
                hit.collider.transform.root.gameObject.SendMessage("TakeDamage", 6969);
                StartCoroutine(Blink());
                gunShotScript.CreateMark(true, hit);
            }
            else if (hit.collider.tag != "Mutant" && hit.collider.tag != "MutantBack" && hit.collider.tag != "MutantHead")
            {
                gunShotScript.CreateMark(false, hit);
            }
            //Debug.Log(hit.collider.name);
        }
        GunNoise();
    }

    IEnumerator Stab()
    {
        yield return new WaitForSeconds(2f);
    }

    void GunNoise()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, alertRange);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.GetComponent<MutantSimple>() != null)
            {
                hitColliders[i].SendMessage("PlayerEnter");
            }
            i++;
        }
    }

    IEnumerator Blink()
    {
        hitMarker.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitMarker.SetActive(false);
        StopCoroutine(Blink());
    }

    IEnumerator MuzzleFlash(GameObject targetFlash)
    {
        targetFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        targetFlash.SetActive(false);
        StopCoroutine(MuzzleFlash(null));
    }

    //Enemy detection
    void OnControllerColliderHit(ControllerColliderHit collider)
    {
        /*if (collider.gameObject.tag == "Mutant")
        {
            `();
        }*/
    }

    public void TakeDamage(float value)
    {
        playerHealth -= value;
        regenTimer = 0.0f;
    }

    //Die function
    IEnumerator KillPlayer()
    {
        dead = true;
        yield return new WaitForSeconds(3.0f);
        transform.position = startPosition;
    }

    public bool IsDead()
    {
        return dead;
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

            if (currentWeaponIndex == 0)
            {
                for (int i = 0; i < rifleBullets.Length; i++)
                {
                    if (i < weapons[currentWeaponIndex].remainingAmmo)
                    {
                        rifleBullets[i].GetComponent<Image>().sprite = weapons[0].fullBullet;
                    }
                    else
                    {
                        rifleBullets[i].GetComponent<Image>().sprite = weapons[0].emptyBullet;
                    }
                }
            }
            else if (currentWeaponIndex == 1)
            {
                for (int j = 0; j < pistolBullets.Length; j++)
                {
                    if (j < weapons[currentWeaponIndex].remainingAmmo)
                    {
                        pistolBullets[j].GetComponent<Image>().sprite = weapons[1].fullBullet;
                    }
                    else
                    {
                        pistolBullets[j].GetComponent<Image>().sprite = weapons[1].emptyBullet;
                    }
                }
            }

            weapons[currentWeaponIndex].remainingAmmo = 0;
        }
        else //Normal reload
        {
            int fillUp = weapons[currentWeaponIndex].magazineSize - weapons[currentWeaponIndex].currentAmmo;
            weapons[currentWeaponIndex].currentAmmo = weapons[currentWeaponIndex].magazineSize;
            weapons[currentWeaponIndex].remainingAmmo -= fillUp;

            if (currentWeaponIndex == 0)
            {
                for(int i = 0; i < rifleBullets.Length; i++)
                {
                    rifleBullets[i].GetComponent<Image>().sprite = weapons[0].fullBullet;
                }
            }
            else if (currentWeaponIndex == 1)
            {
                for (int j = 0; j < pistolBullets.Length; j++)
                {
                    pistolBullets[j].GetComponent<Image>().sprite = weapons[1].fullBullet;
                }
            }
        }
        
        reloadState = false;
        reloadText.SetActive(false);
    }

    public IEnumerator SwitchWeapon(int target)
    {
        yield return new WaitForSeconds(0.5f);
        if (target == 0)
        {
            currentWeaponIndex = 0;
            reloadState = false;

            rifleObject.SetActive(true);
            weapons[0].weaponUI.SetActive(true);
            pistolObject.SetActive(false);
            weapons[1].weaponUI.SetActive(false);

            timeStamp = Time.time;
        }
        else if (target == 1)
        {
            currentWeaponIndex = 1;
            reloadState = false;

            rifleObject.SetActive(false);
            weapons[0].weaponUI.SetActive(false);
            pistolObject.SetActive(true);
            weapons[1].weaponUI.SetActive(true);

            timeStamp = Time.time;
        }
    }

    public IEnumerator PlayerJump()
    {
        yield return new WaitForSeconds(0.25f);

        
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

            case "options":
                optionsView = !optionsView;
                break;

            case "quit":
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }

    public void StopSoundInstances()
    {
        walkingEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void PromptTutorial()
    {
        tutorialPrompt = !tutorialPrompt;
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
