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
        energyBar.maxValue = 100; // �����̴��� �ִ밪�� 100���� ���� 
        energyBar.minValue = 0; // �����̴��� �ּҰ��� 0���� ����
        energyBar.value = 100; // �����̴� ���۰��� 100���� ����
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ������ �����̴� ����
    public void UpdateEnergyBar(int newenergy)
    {
        energyBar.value -= newenergy;
    }
}
