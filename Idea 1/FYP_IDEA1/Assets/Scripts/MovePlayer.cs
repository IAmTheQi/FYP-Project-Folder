using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

    GameObject cam1;
    SmoothMouseLook mouseLookScript;

    public byte playerSpeed;
    public float strafeSlow;
    public float sprintMultiplier;

	// Use this for initialization
	void Start () {
        cam1 = GameObject.Find("Main Camera");
        //mouseLookScript = cam1.GetComponent<SmoothMouseLook>();

        playerSpeed = 2;
        strafeSlow = 0.5f;
        sprintMultiplier = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * sprintMultiplier * playerSpeed * Time.deltaTime);
        }

        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * playerSpeed * strafeSlow * Time.deltaTime);
        }

        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * playerSpeed * Time.deltaTime);
        }

        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * playerSpeed * strafeSlow * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKey("a") && !Input.GetKey("d"))
        {
            sprintMultiplier = 2.0f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprintMultiplier = 1.0f;
        }
	
	}
}
