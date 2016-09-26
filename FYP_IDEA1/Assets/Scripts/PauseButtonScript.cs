using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseButtonScript : MonoBehaviour {

    public Font normal;
    public Font over;

    Color overAlpha;
    Color normalAlpha;


    // Use this for initialization
    void Start () {

        overAlpha.a = 1;
        normalAlpha.a = 0.23f;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {
        GetComponent<Text>().font = over;
        GetComponent<Text>().color = overAlpha;
        Debug.Log("enter");
    }

    void OnMouseExit()
    {
        GetComponent<Text>().font = normal;
        GetComponent<Text>().color = normalAlpha;
    }
}
