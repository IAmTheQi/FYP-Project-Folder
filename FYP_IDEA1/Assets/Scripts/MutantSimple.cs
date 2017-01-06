using UnityEngine;
using System.Collections;

public class MutantSimple : MonoBehaviour
{
    public float health;
    public enum MutantStates
    {
        Roam,
        Idle,
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
    }

    // Update is called once per frame
    protected void Update()
    {

        if (playerObject.GetComponent<PlayerLogic>().pauseGame || playerObject.GetComponent<PlayerLogic>().itemView)
        {

        }
        else if (!dead)
        {
            if (currentState == MutantStates.Chase)
            {
                mutantAgent.destination = playerObject.transform.position;

                //Debug.LogFormat("name:{0}       distance:{1}", gameObject.name, Vector3.Distance(transform.position, playerObject.transform.position));
                //Debug.Log(mutantAgent.speed);
                if (Vector3.Distance(transform.position, playerObject.transform.position) <= 2.1f)
                {
                    attacking = true;
                }
                else if (mutantAgent.velocity.magnitude > 0f)
                {
                    attacking = false;
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
        currentState = MutantStates.Alert;	
        mutantAnimator.SetTrigger("Alert");
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
        }
        else if ((health - value) <= 0)
        {
            health = 0;
            StartCoroutine(Die());
        }
    }

    protected IEnumerator Death()
    {
        dead = true;
        mutantAnimator.SetTrigger("Death");
        yield return new WaitForSeconds(2.5f);
        DeleteColliders();
        StopCoroutine(Death());
    }

    protected IEnumerator Die()
    {
        dead = true;
        mutantAnimator.SetTrigger("Die");
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
        Debug.Log("attack");
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
