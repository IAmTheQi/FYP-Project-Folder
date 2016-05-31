using UnityEngine;
using System.Collections;

[RequireComponent (typeof(LineRenderer))]
public class MutantBaseClass : MonoBehaviour {

    public float health;

    enum MutantStates
    {
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

    MutantStates currentState;

    CharacterController controller;

    protected GameObject playerLastPosition;
    protected GameObject noiseLastPosition;

    protected LineRenderer lineRenderer;

    // Use this for initialization
    protected void Start () {
        dead = false;

        playerObject = GameObject.Find("PlayerController");

        mutantSpeed = 0.0f;
        gravity = 10.0f;

        moveDirection = new Vector3(0, 0, 0);

        currentState = MutantStates.Wander;

        controller = GetComponent<CharacterController>();

        playerLastPosition = GameObject.Find("PlayerLastSeen");
        noiseLastPosition = GameObject.Find("NoiseLastSeen");

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }
	
	// Update is called once per frame
	protected void Update () {

        //Movement Handler for when mutant is on and off the ground
        if (controller.isGrounded)
        {
            if (currentState == MutantStates.Lost || currentState == MutantStates.Chase || currentState == MutantStates.Distracted)
            {
                moveDirection = new Vector3(0, 0, 1);
            }
            else if (currentState == MutantStates.Wander)
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

        if (currentState == MutantStates.Chase || currentState == MutantStates.Lost || currentState == MutantStates.Distracted)
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

        if (Input.GetKey(KeyCode.F))
        {
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
            lineRenderer.enabled = false;
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, playerObject.transform.position);
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

    protected void CalmMutant()
    {
        currentState = MutantStates.Wander;
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
}
