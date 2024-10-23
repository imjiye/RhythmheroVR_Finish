using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapons
{
    Saber1,
    Saber2,
    Sword1,
    Sword2,
    Sword3,
    Sword4,
    Sword5,
    Watermelon
}
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) return;
        DontDestroyOnLoad(gameObject);
    }
    public Weapons CurWeapon;
}
