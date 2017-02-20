using UnityEngine;
using System.Collections;


public class Custom{

    
}

public class HitData
{
    public float damageValue;
    public RaycastHit hitPoint;
    public Ray ray;
    public Rigidbody impactTarget;
}

public class GameHandler
{
    static int previousLevel;
    static int rifleMag;
    static int rifleAmmo;
    static int pistolMag;
    static int pistolAmmo;

    static public void Save(GameObject playerObject, int target)
    {
        previousLevel = target;
        rifleMag = playerObject.GetComponent<PlayerLogic>().weapons[0].currentAmmo;
        rifleAmmo = playerObject.GetComponent<PlayerLogic>().weapons[0].remainingAmmo;
        pistolMag = playerObject.GetComponent<PlayerLogic>().weapons[1].currentAmmo;
        pistolAmmo = playerObject.GetComponent<PlayerLogic>().weapons[1].remainingAmmo;

        PlayerPrefs.SetInt("PreviousLevel", previousLevel);
        PlayerPrefs.SetInt("RifleMag", rifleMag);
        PlayerPrefs.SetInt("RifleAmmo", rifleAmmo);
        PlayerPrefs.SetInt("PistolMag", pistolMag);
        PlayerPrefs.SetInt("PistolAmmo", pistolAmmo);

        PlayerPrefs.Save();
    }

    static public void Load(GameObject playerObject)
    {
        if (PlayerPrefs.HasKey("RifleMag") && PlayerPrefs.HasKey("RifleAmmo") && PlayerPrefs.HasKey("PistolMag") && PlayerPrefs.HasKey("PistolAmmo"))
        {
            playerObject.GetComponent<PlayerLogic>().weapons[0].currentAmmo = PlayerPrefs.GetInt("RifleMag");
            playerObject.GetComponent<PlayerLogic>().weapons[0].remainingAmmo = PlayerPrefs.GetInt("RifleAmmo");
            playerObject.GetComponent<PlayerLogic>().weapons[1].currentAmmo = PlayerPrefs.GetInt("PistolMag");
            playerObject.GetComponent<PlayerLogic>().weapons[1].remainingAmmo = PlayerPrefs.GetInt("PistolAmmo");
        }
    }
}
