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

    public bool anim2;
    public bool attacking;
    public bool aggro;

    bool dead;

    public MutantStates currentState;

    public float damage;
    float attackCounter;
    public float attackDelay;
    float timeStamp;

    GameObject modelObject;
    Animator mutantAnimator;
    protected NavMeshAgent mutantAgent;

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

        modelObject = transform.Find("MutantModel").gameObject;
        mutantAnimator = modelObject.GetComponent<Animator>();
        mutantAgent = GetComponent<NavMeshAgent>();

        idleEv = FMODUnity.RuntimeManager.CreateInstance(idleSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(idleEv, transform, GetComponent<Rigidbody>());
        //idleEv.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform, GetComponent<Rigidbody>()));
        idleEv.start();

        if (currentState == MutantStates.Idle)
        {
            transform.Rotate(new Vector3(0, Random.Range(0.0f, 360.0f), 0));
        }
    }

    // Update is called once per frame
    protected void Update()
    {

        if (playerObject.GetComponent<PlayerLogic>().pauseGame)
        {

        }
        else if (!dead && !playerObject.GetComponent<PlayerLogic>().pauseGame)
        {
            if (aggro)
            {
                if (currentState == MutantStates.Idle || currentState == MutantStates.Roam || currentState == MutantStates.Eat)
                {
                    PlayerEnter();
                    aggro = false;
                }
            }


            if (currentState == MutantStates.Chase)
            {
                mutantAgent.destination = playerObject.transform.position;
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
            else if (currentState == MutantStates.Roam)
            {
                mutantAnimator.SetBool("Moving", true);
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
                    KillPlayer();
                }
            }
        }

        if (dead)
        {
            mutantAgent.Stop();
        }
    }

    public void PlayerEnter()
    {
        if (currentState == MutantStates.Idle || currentState == MutantStates.Eat || currentState == MutantStates.Roam)
        {
            StartCoroutine(AlertMutant());
        }
    }

    public IEnumerator AlertMutant()
    {
        currentState = MutantStates.Alert;
        mutantAnimator.SetTrigger("Alert");
        FMODUnity.RuntimeManager.PlayOneShot(alertSound, transform.position);
        if (!anim2)
        {
            yield return new WaitForSeconds(2.1f);
        }
        else if (anim2)
        {
            yield return new WaitForSeconds(4f);
        }
        mutantAnimator.SetBool("Chase", true);
        currentState = MutantStates.Chase;
        StopCoroutine(AlertMutant());
    }

    void Ragdoll(bool value, HitData target)
    {
        if (currentState != MutantStates.Chase || currentState != MutantStates.Alert)
        {
            idleEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        dead = true;
        FMODUnity.RuntimeManager.PlayOneShot(dyingSound, transform.position);
        DeleteColliders();
        mutantAnimator.enabled = false;

        RagdollHelper ragdollHelper = GetComponent<RagdollHelper>();
        ragdollHelper.ragdolled = true;

        target.impactTarget.AddForce(target.ray.direction * 1000f);
        idleEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void TakeDamage(HitData value)
    {
        if (value.damageValue == 6969)
        {
            health = 0;
            Ragdoll(false, value);
        }

        if ((health - value.damageValue) > 0)
        {
            health -= value.damageValue;

            if (currentState != MutantStates.Chase)
            {
                StartCoroutine(AlertMutant());
            }

            FMODUnity.RuntimeManager.PlayOneShot(hitSound, transform.position);
        }
        else if ((health - value.damageValue) <= 0)
        {
            health = 0;
            Ragdoll(false, value);
        }
    }

    protected void Die()
    {
    }

    protected void DeleteColliders()
    {
        Destroy(this.GetComponent<CapsuleCollider>());
    }

    protected void AttackPlayer()
    {
        playerObject.GetComponent<PlayerLogic>().TakeDamage(damage);
        FMODUnity.RuntimeManager.PlayOneShot(attackSound, transform.position);
    }

    protected void KillPlayer()
    {
        attacking = false;
        mutantAnimator.SetBool("Attack", false);
        mutantAnimator.SetBool("Chase", false);
        mutantAnimator.SetTrigger("Kill");
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
