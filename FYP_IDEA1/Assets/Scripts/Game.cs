using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Game{

    public static Game current;

    public static string timeStamp;
    
    public struct MutantData
    {
        public GameObject mutantObject;
        public Transform lastTransform;
        public string currentState;
        public float health;
    }

    public static List<MutantData> mutantList = new List<MutantData>();

    public struct WeaponsData
    {
        public string weaponName;
        public bool locked;
        public int currentAmmo;
        public int remainingAmmo;
    }

    public struct PlayerData
    {
        public GameObject playerObject;
        public Transform playerTransform;
        public float playerHealth;
        public WeaponsData[] weaponsArray;
    }

    public static PlayerData playerData = new PlayerData();

    public static PlayerLogic playerScript;

    public Game()
    {
        timeStamp = (System.DateTime.Now).ToString();
        Debug.Log(timeStamp);

        GameObject[] mutants;
        mutants = GameObject.FindGameObjectsWithTag("Mutant");

        foreach (GameObject mutantObject in mutants)
        {
            MutantData currentMutant = new MutantData();
            currentMutant.mutantObject = mutantObject;
            currentMutant.lastTransform = mutantObject.transform;
            currentMutant.currentState = mutantObject.GetComponent<MutantSimple>().ReturnState();
            currentMutant.health = mutantObject.GetComponent<MutantSimple>().health;

            Debug.Log(currentMutant.mutantObject + "," + currentMutant.lastTransform + "," + currentMutant.currentState + "," + currentMutant.health);

            mutantList.Add(currentMutant);
        }

        playerScript = GameObject.Find("PlayerController").GetComponent<PlayerLogic>();
        playerData.playerObject = GameObject.Find("PlayerController");
        playerData.playerTransform = playerData.playerObject.transform;
        playerData.playerHealth = playerScript.playerHealth;
        playerData.weaponsArray = new WeaponsData[5];
        for (var i = 0; i < 5; i++)
        {
            playerData.weaponsArray[i].weaponName = playerScript.weapons[i].weaponName;
            playerData.weaponsArray[i].currentAmmo = playerScript.weapons[i].currentAmmo;
            playerData.weaponsArray[i].remainingAmmo = playerScript.weapons[i].remainingAmmo;
        }
    }
}
