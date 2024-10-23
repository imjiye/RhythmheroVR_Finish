using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public GameObject[] weaponPrefabs_L;
    public GameObject[] weaponPrefabs_R;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < weaponPrefabs_L.Length; i++)
        {
            if((int)DataManager.instance.CurWeapon == i)
            {
                weaponPrefabs_L[i].SetActive(true);
            }
            else
            {
                weaponPrefabs_L[i].SetActive(false);
            }
        }

        for(int i = 0; i < weaponPrefabs_R.Length; i++)
        {
            if((int)DataManager.instance.CurWeapon == i)
            {
                weaponPrefabs_R[i].SetActive(true);
            }
            else
            {
                weaponPrefabs_R[i].SetActive(false);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < weaponPrefabs_L.Length; i++)
        {
            if ((int)DataManager.instance.CurWeapon == i)
            {
                weaponPrefabs_L[i].SetActive(true);
            }
            else
            {
                weaponPrefabs_L[i].SetActive(false);
            }
        }

        for (int i = 0; i < weaponPrefabs_R.Length; i++)
        {
            if ((int)DataManager.instance.CurWeapon == i)
            {
                weaponPrefabs_R[i].SetActive(true);
            }
            else
            {
                weaponPrefabs_R[i].SetActive(false);
            }
        }
    }
}
