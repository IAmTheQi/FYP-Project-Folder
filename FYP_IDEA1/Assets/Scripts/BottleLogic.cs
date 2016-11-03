using UnityEngine;
using System.Collections;

public class BottleLogic : MonoBehaviour {

    GameObject lastSeen;

    // Use this for initialization
    void Start () {
        lastSeen = GameObject.Find("NoiseLastSeen");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "PlayerController")
        {
            StartCoroutine(Break());
            Debug.Log(collision.gameObject.name);
        }
        else
        {

        }
    }

    IEnumerator Break()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
        Activate();
        StopCoroutine(Break());
    }

    void Activate()
    {
        lastSeen.SendMessage("Move", transform.position, SendMessageOptions.DontRequireReceiver);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 30);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.GetComponent<MutantOne>() != null)
            {
                hitColliders[i].SendMessage("Distract");
            }
            i++;
        }
    }
}
