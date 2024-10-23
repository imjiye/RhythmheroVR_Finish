using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public TextMeshProUGUI timerText; // 시간 표시 텍스트
    public Slider timerSlider; // 시간 슬라이더
    public float playTime = 120f; // 전체 플레이 시간(초)
    private float currentTime; // 현재 남은 시간

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
        currentTime = playTime; // 시작 시 남은 시간을 전체 플레이 시간으로 설정
        timerSlider.maxValue = 100; // 슬라이더의 최대값을 100으로 설정
        timerSlider.value = 0; // 슬라이더 시작값을 0으로 설정
        UpdateTimerUI(); // UI 업데이트
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // 시간 감소
            float timePercentage = (1 - (currentTime / playTime)) * 100; // 남은 시간을 0에서 100까지로 변환
            timerSlider.value = timePercentage; // 슬라이더 값 설정
            UpdateTimerUI(); // UI 업데이트
        }
        if(currentTime <= 0)
        {
            GameOver();
            timerText.text = "00:00";
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f); // 남은 시간(분)
        int seconds = Mathf.FloorToInt(currentTime % 60); // 남은 시간(초)
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // 00:00 형식으로 텍스트 업데이트
    }

    private void GameOver()
    {
        GameManager.instance.GameClear();
    }
}
