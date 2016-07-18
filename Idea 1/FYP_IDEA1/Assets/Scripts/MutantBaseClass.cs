using UnityEngine;
using System.Collections;

[RequireComponent (typeof(LineRenderer))]
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

    bool dead;

    Vector3 moveDirection;

    public MutantStates currentState;

    CharacterController controller;

    public float damage;
    public float attackCounter;
    public float attackDelay;
    public float timeStamp;

    public GameObject playerLastPositionPrefab;
    protected GameObject playerLastPosition;
    protected GameObject noiseLastPosition;

    public GameObject startPositionPrefab;
    protected GameObject startPosition;

    public GameObject detectionObject;

    protected LineRenderer lineRenderer;

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
        attackDelay = 2.0f;
        timeStamp = Time.time;

        playerLastPosition = (GameObject)Instantiate(playerLastPositionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        noiseLastPosition = GameObject.Find("NoiseLastSeen");

        startPosition = (GameObject)Instantiate(startPositionPrefab, transform.position, Quaternion.identity);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }
	
	// Update is called once per frame
	protected void Update () {
        if (playerObject.GetComponent<PlayerLogic>().pauseGame || playerObject.GetComponent<PlayerLogic>().weaponSelect || playerObject.GetComponent<PlayerLogic>().itemView)
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
                if (Vector3.Distance(transform.position, playerObject.transform.position) < 3.0f)
                {
                    if (Time.time > (timeStamp + attackDelay))
                    {
                        AttackPlayer();
                        timeStamp = Time.time;
                    }
                }
            }

            //Heartbeat sensing
            if (Input.GetKey(KeyCode.F))
            {
                playerObject.GetComponent<PlayerLogic>().focus = true;
                if (Vector3.Distance(transform.position, playerObject.transform.position) < 20)
                {
                    lineRenderer.enabled = true;
                }
                else
                {
                    lineRenderer.enabled = false;
                }
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {
                playerObject.GetComponent<PlayerLogic>().focus = false;
                lineRenderer.enabled = false;
            }

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, playerObject.transform.position);
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

    public virtual void CalmMutant()
    {
        currentState = MutantStates.Return;
        playerLastPosition.SendMessage("Reset");
        noiseLastPosition.SendMessage("Reset");
    }

    protected void Die()
    {
        gameObject.SetActive(false);
    }

    protected void Distract()
    {
        currentState = MutantStates.Distracted;
    }

    protected void AttackPlayer()
    {
        playerObject.GetComponent<PlayerLogic>().TakeDamage(damage);
    }

    public void RecordLastSeen(Transform target)
    {
        playerLastPosition.transform.position = target.position;
    }
}
