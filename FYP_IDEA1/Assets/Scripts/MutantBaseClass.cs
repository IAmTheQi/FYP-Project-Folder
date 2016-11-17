using UnityEngine;
using System.Collections;

public class MutantBaseClass : MonoBehaviour {

    public float health;

    public enum MutantStates
    {
        Idle,
        Return,
        Wander,
        Alerted,
        Chase,
        Attacked,
        Lost,
        Distracted
    }

    GameObject playerObject;

    float mutantSpeed;
    float gravity;

    public bool attacking;

    bool dead;

    Vector3 moveDirection;

    public MutantStates currentState;

    CharacterController controller;

    public float damage;
    public float attackCounter;
    public float attackDelay;
    public float timeStamp;

    GameObject focusRing;

    public GameObject playerLastPositionPrefab;
    protected GameObject playerLastPosition;
    protected GameObject noiseLastPosition;

    public GameObject startPositionPrefab;
    protected GameObject startPosition;

    public GameObject detectionObject;

    protected LineRenderer lineRenderer;

    GameObject modelObject;
    Animator mutantAnimator;
    

    // Use this for initialization
    protected void Start () {
        dead = false;

        playerObject = GameObject.Find("PlayerController");

        mutantSpeed = 0.0f;
        gravity = 10.0f;

        moveDirection = new Vector3(0, 0, 0);

        currentState = MutantStates.Idle;

        controller = GetComponent<CharacterController>();

        damage = 10.0f;
        attackCounter = 0.0f;
        attackDelay = 3.0f;
        timeStamp = Time.time;

        attacking = false;

        focusRing = transform.Find("FocusRing").gameObject;
        focusRing.SetActive(false);

        playerLastPosition = (GameObject)Instantiate(playerLastPositionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        noiseLastPosition = GameObject.Find("NoiseLastSeen");

        startPosition = (GameObject)Instantiate(startPositionPrefab, transform.position, Quaternion.identity);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        modelObject = transform.Find("MutantModel").gameObject;
        mutantAnimator = modelObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	protected void Update () {

        if (playerObject.GetComponent<PlayerLogic>().pauseGame || playerObject.GetComponent<PlayerLogic>().itemView)
        {

        }
        else
        {
            //Movement Handler for when mutant is on and off the ground
            if (controller.isGrounded)
            {
                if (currentState == MutantStates.Lost || currentState == MutantStates.Chase || currentState == MutantStates.Distracted || currentState == MutantStates.Wander || currentState == MutantStates.Return)
                {
                    moveDirection = new Vector3(0, 0, 1);
                }
                else if (currentState == MutantStates.Idle)
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

            if (currentState == MutantStates.Chase || currentState == MutantStates.Lost || currentState == MutantStates.Distracted || currentState == MutantStates.Wander || currentState == MutantStates.Idle)
            {
                mutantSpeed = 2.0f;
            }

            //Mutant movement modifier
            if (!attacking && !dead)
            {
                controller.Move(moveDirection * Time.deltaTime);
            }

            if ((currentState == MutantStates.Alerted || currentState == MutantStates.Chase) && currentState != MutantStates.Lost)
            {
                transform.LookAt(new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z));
            }
            else if (currentState == MutantStates.Lost)
            {
                transform.LookAt(new Vector3(playerLastPosition.transform.position.x, transform.position.y, playerLastPosition.transform.position.z));
            }
            else if (currentState == MutantStates.Distracted)
            {
                transform.LookAt(new Vector3(noiseLastPosition.transform.position.x, transform.position.y, noiseLastPosition.transform.position.z));
            }
            else if (currentState == MutantStates.Return)
            {
                transform.LookAt(new Vector3(startPosition.transform.position.x, transform.position.y, startPosition.transform.position.z));
            }

            //Attack Player Counter
            if (currentState == MutantStates.Chase)
            {
                if (attacking)
                {
                    //attacking = true;
                    mutantAnimator.SetBool("Attack", true);

                    if (!playerObject.GetComponent<PlayerLogic>().IsDead())
                    {
                        if (Time.time > (timeStamp + attackDelay))
                        {
                            AttackPlayer();
                            timeStamp = Time.time;
                        }
                    }
                    else
                    {
                        mutantAnimator.SetTrigger("Kill");
                    }
                }
                else
                {
                    mutantAnimator.SetBool("Attack", false);
                }
            }

            //Heartbeat sensing
            if (Input.GetKeyDown(KeyCode.C))
            {
                playerObject.GetComponent<PlayerLogic>().focus = true;
                if (Vector3.Distance(transform.position, playerObject.transform.position) < 500)
                {
                    //lineRenderer.enabled = true;
                    focusRing.SetActive(true);
                }
                else
                {
                    //lineRenderer.enabled = false;
                    focusRing.SetActive(false);
                }
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                playerObject.GetComponent<PlayerLogic>().focus = false;
                //lineRenderer.enabled = false;
                focusRing.SetActive(false);
            }

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, playerObject.transform.position);
        }
    }

    public void AlertMutant()
    {
        StartCoroutine(StateChange("Chase"));
    }

    public void PlayerTouch()
    {
        if (currentState != MutantStates.Chase)
        {
            StartCoroutine(StateChange("Chase"));
        }
        else if (currentState == MutantStates.Chase)
        {
            attacking = true;
        }
    }

    public void PlayerLeave()
    {
        if (attacking)
        {
            attacking = false;
        }
    }

    public IEnumerator StateChange(string target)
    {
        mutantAnimator.SetTrigger("Alert");
        yield return new WaitForSeconds(2.0f);

        switch (target)
        {
            case "Distract":
                currentState = MutantStates.Distracted;
                break;
            case "Chase":
                currentState = MutantStates.Chase;
                break;
            case "LosePlayer":
                currentState = MutantStates.Lost;
                break;
        }
        mutantAnimator.SetBool("Chase", true);
        StopCoroutine(StateChange(""));
    }

    public void LosePlayer()
    {
        StartCoroutine(StateChange("LosePlayer"));
    }

    public void TakeDamage(float value)
    {
        Debug.Log("Ouch");
        if ((health - value) > 0)
        {
            health -= value;
        }
        else if ((health - value) <= 0)
        {
            health = 0;
            StartCoroutine(Die());
        }
    }

    public virtual void CalmMutant()
    {
        currentState = MutantStates.Return;
        playerLastPosition.SendMessage("Reset");
        noiseLastPosition.SendMessage("Reset");
    }

    protected IEnumerator Die()
    {
        dead = true;
        mutantAnimator.SetTrigger("Death");
        yield return new WaitForSeconds(2.5f);
        Destroy(this);
        StopCoroutine(Die());
    }

    protected void Distract()
    {
        StartCoroutine(StateChange("Distract"));
    }

    protected void AttackPlayer()
    {
        playerObject.GetComponent<PlayerLogic>().TakeDamage(damage);
    }

    public void RecordLastSeen(Transform target)
    {
        playerLastPosition.SendMessage("Move", target.position);
    }

    public string ReturnState()
    {
        return currentState.ToString();
    }

    public bool IsDead()
    {
        return dead;
    }
}
