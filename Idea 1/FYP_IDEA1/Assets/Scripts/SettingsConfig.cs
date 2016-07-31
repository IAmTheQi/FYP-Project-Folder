using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System.Xml;

public class SettingsConfig : MonoBehaviour {

    public TextAsset playerValues;

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
    void Start () {
        string textData = playerValues.text;
        ParseValuesXML(textData);
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

    void ParseValuesXML(string xmlData)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xmlData));

        string healthPath = "//player/health";
        XmlNode healthNode = xmlDoc.SelectSingleNode(healthPath);
        playerHealth = float.Parse(healthNode.InnerXml);

        string walkHeightPath = "//player/heights/walk";
        XmlNode walkHeightNode = xmlDoc.SelectSingleNode(walkHeightPath);
        walkHeight = float.Parse(walkHeightNode.InnerXml);

        string crouchHeightPath = "//player/heights/crouch";
        XmlNode crouchHeightNode = xmlDoc.SelectSingleNode(crouchHeightPath);
        crouchHeight = float.Parse(crouchHeightNode.InnerXml);

        string proneHeightPath = "//player/heights/prone";
        XmlNode proneHeightNode = xmlDoc.SelectSingleNode(proneHeightPath);
        proneHeight = float.Parse(proneHeightNode.InnerXml);

        string runModPath = "//player/modifier/run";
        XmlNode runModNode = xmlDoc.SelectSingleNode(runModPath);
        runModifier = float.Parse(runModNode.InnerXml);

        string walkModPath = "//player/modifier/walk";
        XmlNode walkModNode = xmlDoc.SelectSingleNode(walkModPath);
        walkModifier = float.Parse(walkModNode.InnerXml);

        string crouchModPath = "//player/modifier/crouch";
        XmlNode crouchModNode = xmlDoc.SelectSingleNode(crouchModPath);
        crouchModifier = float.Parse(crouchModNode.InnerXml);

        string proneModPath = "//player/modifier/prone";
        XmlNode proneModNode = xmlDoc.SelectSingleNode(proneModPath);
        proneModifier = float.Parse(proneModNode.InnerXml);

        string maxVeloPath = "//player/movement/max_velocity";
        XmlNode maxVeloNode = xmlDoc.SelectSingleNode(maxVeloPath);
        maxVelocity = float.Parse(maxVeloNode.InnerXml);

        string forwAccelPath = "//player/movement/forward_acceleration";
        XmlNode forwAccelNode = xmlDoc.SelectSingleNode(forwAccelPath);
        forwardAccelerationRate = float.Parse(forwAccelNode.InnerXml);

        string revAccelPath = "//player/movement/reverse_acceleration";
        XmlNode revAccelNode = xmlDoc.SelectSingleNode(revAccelPath);
        reverseAccelerationRate = float.Parse(revAccelNode.InnerXml);

        string decelPath = "//player/movement/decceleration_rate";
        XmlNode decelNode = xmlDoc.SelectSingleNode(decelPath);
        deccelerationRate = float.Parse(decelNode.InnerXml);

        string gravityPath = "//player/movement/gravity";
        XmlNode gravityNode = xmlDoc.SelectSingleNode(gravityPath);
        gravity = float.Parse(gravityNode.InnerXml);

        string jumpPath = "//player/movement/jump_force";
        XmlNode jumpNode = xmlDoc.SelectSingleNode(jumpPath);
        jumpForce = float.Parse(jumpNode.InnerXml);

        string strafePath = "//player/movement/strafe_slow";
        XmlNode strafeNode = xmlDoc.SelectSingleNode(strafePath);
        strafeSlow = float.Parse(strafeNode.InnerXml);
    }
}
