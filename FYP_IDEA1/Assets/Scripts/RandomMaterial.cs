using UnityEngine;
using System.Collections;

public class RandomMaterial : MonoBehaviour {

    public Texture[] textureArray;

    int index;

	// Use this for initialization
	void Start () {

	}

    void OnEnable()
    {
        index = Random.Range(0, (textureArray.Length - 1));

        GetComponent<Renderer>().material.mainTexture = textureArray[index];
    }
}