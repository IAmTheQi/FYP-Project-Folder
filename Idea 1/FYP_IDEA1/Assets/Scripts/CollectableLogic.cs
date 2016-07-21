using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    public int currentSelected;

    Color selected;
    Color unselected;
    Color uncollected;

    // Use this for initialization
    void Start()
    {

        currentSelected = 0;

        selected = itemsArray[0].selectUI.GetComponent<Text>().color;
        selected.a = 1f;
        unselected = itemsArray[0].selectUI.GetComponent<Text>().color;
        unselected.a = 0.3f;
        uncollected = itemsArray[0].selectUI.GetComponent<Text>().color;
        uncollected.a = 0.15f;

        for (var i = 0; i < itemsArray.Length; i++)
        {
            if (!itemsArray[i].collected)
            {
                itemsArray[i].selectUI.GetComponent<Text>().color = unselected;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ScrollItem("down");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ScrollItem("up");
        }
	
	}

    void ScrollItem(string target)
    {
        Debug.Log(currentSelected);

        if (target == "down")
        {
            if (!itemsArray[currentSelected + 1].collected)
            {
                currentSelected += 1;
                ScrollItem("down");
                return;
            }
            else
            {
                itemsArray[currentSelected].selectUI.GetComponent<Text>().color = unselected;
                currentSelected += 1;
                itemsArray[currentSelected].selectUI.GetComponent<Text>().color = selected;
                return;
            }
        }
        else if (target == "up")
        {
            if (!itemsArray[currentSelected - 1].collected)
            {
                currentSelected -= 1;
                ScrollItem("up");
                return;
            }
            else
            {
                itemsArray[currentSelected].selectUI.GetComponent<Text>().color = unselected;
                currentSelected -= 1;
                itemsArray[currentSelected].selectUI.GetComponent<Text>().color = selected;
                return;
            }
        }
        /*
        for (var i = 0; i < itemsArray.Length; i++)
        {
            if (itemsArray[i].collected)
            {
                if (i == currentSelected)
                {
                    itemsArray[i].selectUI.GetComponent<Text>().color = selected;
                }
                else
                {
                    itemsArray[i].selectUI.GetComponent<Text>().color = unselected;
                }
            }
            else if (!itemsArray[i].collected)
            {
                if (target == "up")
                {
                    i -= 1;
                    currentSelected = i;
                }
                else if (target == "down")
                {
                    i += 1;
                    currentSelected = i;
                }
            }
        }*/
    }

    public void CollectItem(GameObject target)
    {
        for (var i = 0; i < itemsArray.Length; i++)
        {
            if (target == itemsArray[i].itemObject)
            {
                itemsArray[i].collected = true;
                Destroy(target);
                break;
            }
        }
    }
}
