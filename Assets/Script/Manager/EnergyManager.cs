using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<EnergyManager>();
            }
            return m_instance;
        }
    }

    private static EnergyManager m_instance;

    public Slider energyBar;

    // Start is called before the first frame update
    void Start()
    {
        energyBar.maxValue = 100; // 슬라이더의 최대값을 100으로 설정 
        energyBar.minValue = 0; // 슬라이더의 최소값을 0으로 설정
        energyBar.value = 100; // 슬라이더 시작값을 100으로 설정
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 에너지 슬라이더 갱신
    public void UpdateEnergyBar(int newenergy)
    {
        energyBar.value -= newenergy;
    }
}
