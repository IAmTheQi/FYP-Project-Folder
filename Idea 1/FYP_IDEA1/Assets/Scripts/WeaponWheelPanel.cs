using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponWheelPanel : MonoBehaviour {

    GameObject playerObject;
    PlayerLogic playerScript;

    public byte index;

	// Use this for initialization
	void Start () {

        playerObject = GameObject.Find("PlayerController");
        playerScript = playerObject.GetComponent<PlayerLogic>();
	
	}
	
	// Update is called once per frame
	void Update () {

        if (index == 3)
        {
            if (playerScript.weapons[3].locked)
            {
                GetComponent<Button>().interactable = false;
            }
            else
            {
                GetComponent<Button>().interactable = true;
            }
        }

        if (index == 4)
        {
            if (playerScript.weapons[4].locked)
            {
                GetComponent<Button>().interactable = false;
            }
            else
            {
                GetComponent<Button>().interactable = true;
            }
        }

    }

    public void TriggerChange()
    {
        Debug.Log(index);
        if (index == 0)
        {
            playerScript.SwitchWeapon(0);
        }
        else if (index == 1)
        {
            playerScript.SwitchWeapon(1);
        }
        else if (index == 2)
        {
            playerScript.SwitchWeapon(2);
        }
        else if (index == 3 && !playerScript.weapons[3].locked)
        {
            playerScript.SwitchWeapon(3);
        }
        else if (index == 4 && !playerScript.weapons[4].locked)
        {
            playerScript.SwitchWeapon(4);
        }
    }
}
