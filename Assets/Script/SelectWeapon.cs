using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectWeapon : MonoBehaviour
{
    public Weapons weapon;
    public SelectWeapon[] weap;
    public DOTweenAnimation scalechange;

    // Start is called before the first frame update
    void Start()
    {

        if (DataManager.instance.CurWeapon == weapon)
        {
            OnSelect();
        }
        else
        {
            OnDeSelect();
        }
    }

    public void OnSelect()
    {
        scalechange.DOPlay();

        DataManager.instance.CurWeapon = weapon;

        for (int i = 0; i < weap.Length; i++)
        {
            if (weap[i] != this)
            {
                weap[i].OnDeSelect();
            }
        }
    }

    public void OnDeSelect()
    {
        scalechange.DORewind(); 
    }
}
