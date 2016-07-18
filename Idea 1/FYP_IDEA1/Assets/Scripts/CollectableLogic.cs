using UnityEngine;
using System.Collections;

public class CollectableLogic : MonoBehaviour {

    [System.Serializable]
    public struct CollectableItems
    {
        public string itemName;
        public GameObject itemObject;
        public bool collected;
        public GameObject selectUI;
        public GameObject inspectUI;
    }

    public CollectableItems[] itemsArray;

    public byte currentSelected;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CollectItem(GameObject target)
    {
        for (var i = 0; i < itemsArray.Length; i++)
        {
            if (target == itemsArray[i].itemObject)
            {
                itemsArray[i].collected = true;
                break;
            }
        }
    }
}
