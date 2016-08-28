using UnityEngine;
using System.Collections;

public class NoiseObject : MonoBehaviour{

    GameObject lastSeen;

	// Use this for initialization
	void Start () {
        lastSeen = GameObject.Find("NoiseLastSeen");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Activate()
    {
        lastSeen.SendMessage("Move", transform.position, SendMessageOptions.DontRequireReceiver);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 20);
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 20.0f);
    }
}
