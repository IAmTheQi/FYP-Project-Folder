using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossOne : BossBaseClass {

    Animation anim;

    public Image healthBar;

	// Use this for initialization
	void Start () {

        health = 200.0f;

        anim = GetComponent<Animation>();

        base.Start();
	}

    void Update()
    {
        base.Update();

        healthBar.fillAmount = health / 200;
        Debug.Log(health);
    }

    public override void Die()
    {
        base.Die();
        anim.Play();
    }
}
