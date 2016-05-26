using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

    GameObject parentObject;
    PlayerLogic parentScript;

	// Use this for initialization
	void Start () {

        parentObject = transform.parent.gameObject;
        parentScript = parentObject.GetComponent<PlayerLogic>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Attack(int attack)
    {
        parentScript.ShootRay(attack);
    }
}
