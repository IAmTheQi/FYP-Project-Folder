using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SettingsConfig : MonoBehaviour {

    public byte mouseSense;
    public byte mouseDrag;

    public float walkHeight;
    public float crouchHeight;
    public float proneHeight;

    public float playerHealth;
    public float playerSpeed;
    public float strafeSlow;
    public float jumpForce;
    public float gravity;

    public float speedModifier;
    public float runModifier;
    public float walkModifier;
    public float crouchModifier;
    public float proneModifier;

    public float initialVelocity;
    public float currentVelocity;
    public float maxVelocity;
    public float forwardAccelerationRate;
    public float reverseAccelerationRate;
    public float deccelerationRate;

    // Use this for initialization
    void Awake () {

    }
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetKeyDown(KeyCode.P))
        {
            ResetScene();
        }
	}

    public void ResetScene()
    {
        SceneManager.LoadScene("Level1");
    }
}
