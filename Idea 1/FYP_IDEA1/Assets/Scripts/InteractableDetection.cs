using UnityEngine;
using System.Collections;

public class InteractableDetection : MonoBehaviour {

    GameObject camera1;

    Transform camTransform;


    RaycastHit hit;

    // Use this for initialization
    void Start () {

        camera1 = GameObject.Find("Main Camera");
        camTransform = camera1.transform;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Interactable")
        {
            Ray ray = new Ray(camTransform.position, camTransform.forward);
            if (Physics.Raycast(ray, out hit, 10))
            {
                if (hit.collider.tag == "Interactable")
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        hit.collider.gameObject.SendMessage("Activate");
                    }
                }
            }

        }
    }
}
