using UnityEngine;
using System.Collections;

public class GunShotFeedbackHandler : MonoBehaviour {

    public GameObject bulletPrefab;
    public ParticleSystem[] bloodPrefabArray;

    GameObject[] bulletMarkArray;
    int bulletMarkIndex;

    ParticleSystem[] bloodSplatArray;
    int bloodSplatIndex;

	// Use this for initialization
	void Start () {

        bulletMarkArray = new GameObject[20];
        bulletMarkIndex = 0;
	
        for (int i = 0; i < bulletMarkArray.Length; i++)
        {
            bulletMarkArray[i] = Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            bulletMarkArray[i].SetActive(false);
        }

        bloodSplatArray = new ParticleSystem[20];
        bloodSplatIndex = 0;

        for (int j = 0; j < bloodSplatArray.Length; j++)
        {
            int index = Random.Range(0, (bloodPrefabArray.Length - 1));
            bloodSplatArray[j] = Instantiate(bloodPrefabArray[index], new Vector3(0, 0, 0), Quaternion.identity) as ParticleSystem;
            bloodSplatArray[j].gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateMark(bool mutantHit, RaycastHit hit)
    {
        if (!mutantHit)
        {
            bulletMarkArray[bulletMarkIndex].SetActive(false);
            bulletMarkArray[bulletMarkIndex].transform.position = hit.point + (hit.normal * 0.01f);
            bulletMarkArray[bulletMarkIndex].transform.rotation = Quaternion.LookRotation(-hit.normal);
            bulletMarkArray[bulletMarkIndex].SetActive(true);

            if (bulletMarkIndex < bulletMarkArray.Length - 1)
            {
                bulletMarkIndex += 1;
            }
            else
            {
                bulletMarkIndex = 0;
            }
        }
        else if (mutantHit)
        {
            bloodSplatArray[bloodSplatIndex].gameObject.SetActive(true);
            bloodSplatArray[bloodSplatIndex].transform.position = hit.point;
            bloodSplatArray[bloodSplatIndex].transform.rotation = Quaternion.LookRotation(-hit.normal);
            bloodSplatArray[bloodSplatIndex].Emit(1);

            if (bloodSplatIndex < bloodSplatArray.Length - 1)
            {
                bloodSplatIndex += 1;
            }
            else
            {
                bloodSplatIndex = 0;
            }
        }
    }
}
