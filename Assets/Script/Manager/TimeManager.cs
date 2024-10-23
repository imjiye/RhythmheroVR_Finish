using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public TextMeshProUGUI timerText; // �ð� ǥ�� �ؽ�Ʈ
    public Slider timerSlider; // �ð� �����̴�
    public float playTime = 120f; // ��ü �÷��� �ð�(��)
    private float currentTime; // ���� ���� �ð�

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        currentTime = playTime; // ���� �� ���� �ð��� ��ü �÷��� �ð����� ����
        timerSlider.maxValue = 100; // �����̴��� �ִ밪�� 100���� ����
        timerSlider.value = 0; // �����̴� ���۰��� 0���� ����
        UpdateTimerUI(); // UI ������Ʈ
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // �ð� ����
            float timePercentage = (1 - (currentTime / playTime)) * 100; // ���� �ð��� 0���� 100������ ��ȯ
            timerSlider.value = timePercentage; // �����̴� �� ����
            UpdateTimerUI(); // UI ������Ʈ
        }
        if(currentTime <= 0)
        {
            GameOver();
            timerText.text = "00:00";
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f); // ���� �ð�(��)
        int seconds = Mathf.FloorToInt(currentTime % 60); // ���� �ð�(��)
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // 00:00 �������� �ؽ�Ʈ ������Ʈ
    }

    private void GameOver()
    {
        GameManager.instance.GameClear();
    }
}
