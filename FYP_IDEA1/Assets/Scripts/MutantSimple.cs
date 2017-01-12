﻿using UnityEngine;
using System.Collections;

public class MutantSimple : MonoBehaviour
{
    public float health;
    public enum MutantStates
    {
        Roam,
        Idle,
        Eat,
        Alert,
        Chase
    }

    GameObject playerObject;

    public bool attacking;

    bool dead;

    public MutantStates currentState;

    public float damage;
    float attackCounter;
    public float attackDelay;
    float timeStamp;

    public GameObject focusRing;

    GameObject modelObject;
    Animator mutantAnimator;
    protected NavMeshAgent mutantAgent;

    public GameObject backColliderObject;
    public GameObject headColliderObject;

    [FMODUnity.EventRef]
    public string idleSound = "event:/Mutant/Idle";
    FMOD.Studio.EventInstance idleEv;

    [FMODUnity.EventRef]
    public string alertSound = "event:/Mutant/Alert";

    [FMODUnity.EventRef]
    public string attackSound = "event:/Mutant/Attack";

    [FMODUnity.EventRef]
    public string dyingSound = "event:/Mutant/Dying";

    [FMODUnity.EventRef]
    public string hitSound = "event:/Mutant/BulletPunch";

    // Use this for initialization
    protected void Start()
    {
        dead = false;

        playerObject = GameObject.Find("PlayerController");

        attackCounter = 0.0f;
        timeStamp = Time.time;

        attacking = false;

        focusRing.SetActive(false);

        modelObject = transform.Find("MutantModel").gameObject;
        mutantAnimator = modelObject.GetComponent<Animator>();
        mutantAgent = GetComponent<NavMeshAgent>();

        idleEv = FMODUnity.RuntimeManager.CreateInstance(idleSound);
        idleEv.start();
    }

    // Update is called once per frame
    protected void Update()
    {

        if (playerObject.GetComponent<PlayerLogic>().pauseGame)
        {

        }
        else if (!dead && !playerObject.GetComponent<PlayerLogic>().pauseGame)
        {
            if (currentState == MutantStates.Chase)
            {
                mutantAgent.destination = playerObject.transform.position;

                //Debug.LogFormat("name:{0}       distance:{1}", gameObject.name, Vector3.Distance(transform.position, playerObject.transform.position));
                //Debug.Log(mutantAgent.speed);
                if (Vector3.Distance(transform.position, playerObject.transform.position) <= 2f)
                {
                    attacking = true;
                }
                else if (mutantAgent.velocity.magnitude > 0f)
                {
                    attacking = false;
                }
            }
            else if (currentState == MutantStates.Idle)
            {
                if (mutantAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    if (Random.Range(0f, 1f) < 0.05f)
                    {
                        mutantAnimator.SetTrigger("IdleBreaker");
                    }
                }
            }

            if (!attacking)
            {
                mutantAnimator.SetBool("Attack", false);
            }
            else if (attacking)
            {
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
        }

        if (dead)
        {
            mutantAgent.Stop();
        }
    }
    public void PlayerEnter()
    {
        StartCoroutine(AlertMutant());
    }

    public IEnumerator AlertMutant()
    {
        idleEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        currentState = MutantStates.Alert;	
        mutantAnimator.SetTrigger("Alert");
        FMODUnity.RuntimeManager.PlayOneShot(alertSound);
        yield return new WaitForSeconds(2.1f);
        mutantAnimator.SetBool("Chase", true);
        currentState = MutantStates.Chase;
        StopCoroutine(AlertMutant());
    }

    public void TakeDamage(float value)
    {
        if (value == 6969)
        {
            StartCoroutine(Death());
        }

        if ((health - value) > 0)
        {
            health -= value;

            if (currentState != MutantStates.Chase)
            {
                StartCoroutine(AlertMutant());
            }

            FMODUnity.RuntimeManager.PlayOneShot(hitSound);
        }
        else if ((health - value) <= 0)
        {
            health = 0;
            StartCoroutine(Die());
        }
    }

    protected IEnumerator Death()
    {
        if (currentState != MutantStates.Chase || currentState != MutantStates.Alert)
        {
            idleEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        dead = true;
        mutantAnimator.SetTrigger("Death");
        FMODUnity.RuntimeManager.PlayOneShot(dyingSound);
        yield return new WaitForSeconds(2.5f);
        DeleteColliders();
        StopCoroutine(Death());
    }

    protected IEnumerator Die()
    {
        if (currentState != MutantStates.Chase || currentState != MutantStates.Alert)
        {
            idleEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        dead = true;
        mutantAnimator.SetTrigger("Die");
        FMODUnity.RuntimeManager.PlayOneShot(dyingSound);
        yield return new WaitForSeconds(2.5f);
        DeleteColliders();
        StopCoroutine(Die());
    }

    protected void DeleteColliders()
    {
        Destroy(this.GetComponent<CapsuleCollider>());
        Destroy(headColliderObject);
        Destroy(backColliderObject);
    }

    protected void AttackPlayer()
    {
        playerObject.GetComponent<PlayerLogic>().TakeDamage(damage);
        FMODUnity.RuntimeManager.PlayOneShot(attackSound);
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
