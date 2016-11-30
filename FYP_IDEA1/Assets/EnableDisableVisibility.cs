using UnityEngine;
using System.Collections;

public class EnableDisableVisibility : MonoBehaviour {
	[SerializeField] string objectName;

	void OnBecameVisible() {
		enabled = true;
		Debug.Log(string.Format("{0} became visible", objectName));
	}

	void OnBecameInvisible() {
		enabled = false;
		Debug.Log(string.Format("{0} became invisible", objectName));
	}
}