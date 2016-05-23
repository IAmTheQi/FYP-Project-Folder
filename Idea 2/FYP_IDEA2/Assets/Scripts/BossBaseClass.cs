using UnityEngine;
using System.Collections;

public class BossBaseClass: MonoBehaviour{

    public float health;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public float TakeDamage(float value)
    {
        if ((health - value) > 0)
        {
            health -= value;
        }
        else if ((health - value) < 0)
        {
            health = 0;
        }

        return health;
    }

    public virtual void Attack()
    {

    }
}
