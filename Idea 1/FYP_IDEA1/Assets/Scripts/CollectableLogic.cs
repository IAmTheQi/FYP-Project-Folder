using System.Collections;
using System.Collections.Generic;
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

    public GameObject listObject;

    public CollectableItems[] itemsArray;

    public List<int> indexList;

    public int currentSelected;

    bool inspect;

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

        indexList = new List<int>();

        inspect = false;

        for (var i = 0; i < itemsArray.Length; i++)
        {
            if (!itemsArray[i].collected)
            {
                itemsArray[i].selectUI.GetComponent<Text>().color = uncollected;
            }
            else
            {
                indexList.Add(i);
                itemsArray[i].selectUI.GetComponent<Text>().color = unselected;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (!inspect)
        {
            if (indexList.Count > 1)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    ScrollItem("down");
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    ScrollItem("up");
                }
            }
            else if (indexList.Count == 1)
            {
                currentSelected = indexList[0];
                itemsArray[currentSelected].selectUI.GetComponent<Text>().color = selected;
            }
        }
	
	}

    void ScrollItem(string target)
    {
        Debug.Log(currentSelected);

        if (target == "down")
        {
            if (!itemsArray[indexList[ClampInRange(currentSelected, "down")]].collected)
            {
                ClampInRange(currentSelected, "down");
                ScrollItem("down");
                return;

            }
            else
            {
                itemsArray[indexList[currentSelected]].selectUI.GetComponent<Text>().color = unselected;
                currentSelected += 1;
                itemsArray[indexList[currentSelected]].selectUI.GetComponent<Text>().color = selected;
                return;
            }

        }
        else if (target == "up")
        {
            if (!itemsArray[indexList[ClampInRange(currentSelected, "up")]].collected)
            {
                ClampInRange(currentSelected, "up");
                ScrollItem("up");
                return;

            }
            else
            {
                itemsArray[indexList[currentSelected]].selectUI.GetComponent<Text>().color = unselected;
                currentSelected -= 1;
                itemsArray[indexList[currentSelected]].selectUI.GetComponent<Text>().color = selected;
                return;
            }

        }
    }

    public void CollectItem(GameObject target)
    {
        for (var i = 0; i < itemsArray.Length; i++)
        {
            if (target == itemsArray[i].itemObject)
            {
                itemsArray[i].collected = true;
                itemsArray[i].selectUI.GetComponent<Text>().color = unselected;
                indexList.Add(i);
                indexList.Sort();
                Destroy(target);
                break;
            }
        }
    }

    int ClampInRange(int target, string target2)
    {
        if (target2 == "down")
        {
            if (target + 1 > indexList.Count - 1)
            {
                target = 0;
            }
            else
            {
                target += 1;
            }
        }
        else if (target2 == "up")
        {
            if (target - 1 < 0)
            {
                target = indexList.Count - 1;
            }
            else
            {
                target -= 1;
            }
        }

        return target;
    }

    public void InspectItem()
    {
        inspect = !inspect;

        switch (inspect)
        {
            case true:
                itemsArray[currentSelected].inspectUI.SetActive(true);
                listObject.SetActive(false);
                break;

            case false:
                itemsArray[currentSelected].inspectUI.SetActive(false);
                listObject.SetActive(true);
                break;
        }
    }
}
