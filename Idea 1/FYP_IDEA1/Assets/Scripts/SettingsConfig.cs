using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SettingsConfig : MonoBehaviour {

    public byte mouseSense;
    public byte mouseDrag;

    public float initialVelocity;
    public float currentVelocity;
    public float maxVelocity;
    public float forwardAccelerationRate;
    public float reverseAccelerationRate;
    public float deccelerationRate;

    public float playerHealth;
    public byte playerSpeed;
    public float strafeSlow;
    public float speedModifier;
    public float jumpForce;
    public float gravity;

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
