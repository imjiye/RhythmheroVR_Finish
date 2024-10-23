using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils.Datums;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    private static GameManager m_instance;

    private int score = 0;
    private int combo = 0;
    private int maxcombo = 0;
    private int energy = 100;
    private int damage = 0;

    public InputField InputTextName; // 플레이어 닉네임 입력 UI
    public GameObject RedcubeSpawn;
    public GameObject BluecubeSpawn;

    public bool isGameover { get; private set; }

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(EndGameAfterBGM());
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetInt("combo", combo);
    }

    // Update is called once per frame
    void Update()
    {
        if(energy < 0)
        {
            GameOver();
        }
    }

    //// EndGame을 노래가 끝나고 1초 뒤에 호출하게 하는 코루틴
    //private IEnumerator EndGameAfterBGM()
    //{
    //    float bgmLength = SoundManager.instance.GetBGMLength();

    //    yield return new WaitForSeconds(bgmLength);

    //    yield return new WaitForSeconds(1f);

    //    GameClear();
    //}

    // 콤보에 따른 배수 계산 함수
    public int GetComboMultiplier()
    {
        if(combo >= 35)
        {
            return 5; // 35 콤보 이상은 5배
        }
        else if(combo >= 25)
        {
            return 4; // 25 ~ 34 콤보는 4배
        }
        else if(combo >= 15)
        {
            return 3; // 15 ~ 24 콤보는 3배
        }
        else if(combo >= 6)
        {
            return 2; // 6 ~ 14 콤보는 2배
        }
        return 1; // 콤보가 5 이하인 경우 배수 없음
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수에 콤보 배수 적용하기
            int multiplier = GetComboMultiplier();
            UIManager.instance.UpdatemultiplierText(multiplier);
            int finalScore = Mathf.RoundToInt(newScore * multiplier);

            // 점수 추가
            score += finalScore;

            // 점수 정보 저장
            PlayerPrefs.SetInt("score", score);
            // 점수 UI 텍스트 갱신
            UIManager.instance.UpdateScoreText(score);
        }
    }

    // 콤보를 추가하고 UI 갱신
    public void AddCombo(int newCombo)
    {
        if (!isGameover)
        {
            // 콤보 추가
            combo += newCombo;

            // 최고 콤보 갱신
            if(combo > maxcombo)
            {
                maxcombo = combo;

                PlayerPrefs.SetInt("MaxCombo", maxcombo);
            }

            // 콤보 정보 저장
            PlayerPrefs.SetInt("combo", combo);
            // 콤보 UI 텍스트 갱신
            UIManager.instance.UpdateComboText(combo);
        }
    }

    // 콤보 리셋 & 에너지 감소
    public void ResetCombo()
    {
        combo = 0;
        // 콤보 UI 텍스트 갱신
        UIManager.instance.UpdateComboText(combo);

        energy -= damage;
        EnergyManager.instance.UpdateEnergyBar(damage);
    }

    // 게임 클리어 처리
    public void GameClear()
    {
        isGameover = true;

        // 남아있는 모든 큐브 삭제하기
        CubeNote.DestroyAllCube();

        RedcubeSpawn.SetActive(false);
        BluecubeSpawn.SetActive(false);

        // 기존 획득 점수
        int finalScore = PlayerPrefs.GetInt("score");

        // 최종 점수를 최고 점수로 저장
        Set_BestScore(finalScore);

        Set_MyCombo(combo);
                
        string scoreRank = ScoreRank();

        // 닉네임을 입력받아 저장하기
        //SaveUserName();
        string username = PlayerPrefs.GetString("username", "NoName");

        Debug.Log($"Game Clear - Score: {finalScore}, Username: {username}, Rank: {scoreRank}");

        UIManager.instance.SetActiveGameClearUI(true);
        UIManager.instance.Cur_Score();
        UIManager.instance.Cur_Combo();
        UIManager.instance.Cur_Rank(scoreRank);

        // 랭킹 업데이트 함수 호출
        UpdateRanking(finalScore, username, scoreRank);

        // PlayerPrefs 변경사항 즉시 저장
        PlayerPrefs.Save();
    }

    // 게임 오버 처리
    public void GameOver()
    {
        isGameover = true;

        // 남아있는 모든 큐브 삭제하기
        CubeNote.DestroyAllCube();

        RedcubeSpawn.SetActive(false);
        BluecubeSpawn.SetActive(false);

        string scoreRank = ScoreRank();

        UIManager.instance.SetActiveGameOverUI(true);
        UIManager.instance.Cur_Score();
        UIManager.instance.Cur_Combo();
        UIManager.instance.Cur_Rank(scoreRank);

        SoundManager.instance.SoundAllPause();
    }


    public string ScoreRank()
    {
        if (score >= 25000)
        {
            return "A"; 
        }
        else if (score >= 20000)
        {
            return "B"; 
        }
        else if (score >= 10000)
        {
            return "C"; 
        }
        else if (score >= 8000)
        {
            return "D"; 
        }
        else if(score >= 5000)
        {
            return "E";
        }
        return "F";
    }

    public void UpdateRanking(int score, string username, string rank)
    {
        Debug.Log($"Updating Ranking - New Score: {score}, Username: {username}, Rank: {rank}");

        // 현재 저장된 상위 5명의 점수와 닉네임을 불러옴
        int[] topScores = new int[5];
        string[] topNames = new string[5];
        string[] topRank = new string[5];

        for (int i = 0; i < 5; i++)
        {
            topScores[i] = PlayerPrefs.GetInt("rank_score" + i, 0);
            topNames[i] = PlayerPrefs.GetString("rank_name" + i, "None");
            topRank[i] = PlayerPrefs.GetString("rank_rank" + i, "None");

            Debug.Log($"Loaded Rank {i}: Score={topScores[i]}, Name={topNames[i]}, Rank={topRank[i]}");
        }

        // 현재 점수가 상위 5위 안에 들어갈 수 있는지 체크
        bool rankUpdated = false;
        for (int i = 0; i < 5; i++)
        {
            if (score > topScores[i])
            {
                // 새로 들어온 점수가 순위에 들면 기존 순위를 한 칸씩 내림
                for (int j = 2; j > i; j--)
                {
                    topScores[j] = topScores[j - 1];
                    topNames[j] = topNames[j - 1];
                    topRank[j] = topRank[j - 1];
                }
                // 새로운 점수를 해당 순위에 삽입
                topScores[i] = score;
                topNames[i] = username;
                topRank[i] = rank;
                rankUpdated = true;
                Debug.Log($"New ranking inserted at position {i}");
                break;
            }
        }

        // PlayerPrefs에 업데이트된 랭킹 저장
        if (rankUpdated)
        {
            for (int i = 0; i < 5; i++)
            {
                PlayerPrefs.SetInt("rank_score" + i, topScores[i]);
                PlayerPrefs.SetString("rank_name" + i, topNames[i]);
                PlayerPrefs.SetString("rank_rank" + i, topRank[i]);
                // 디버그 로그 추가
                Debug.Log($"Saved Rank {i}: Score={topScores[i]}, Name={topNames[i]}, Rank={topRank[i]}");
            }
            PlayerPrefs.Save();  // 변경사항 즉시 저장
        }
    }

    // 최고 점수 저장하기
    public int Get_BestScore()
    {
        int BS = PlayerPrefs.GetInt("BestScore");
        return BS;
    }

    public void Set_BestScore(int cur_score)
    {
        if (cur_score > Get_BestScore())
        {
            PlayerPrefs.SetInt("BestScore", cur_score);
            UIManager.instance.UpdateBestScore(cur_score);
        }
        else
        {
            UIManager.instance.UnUpdateBestScore(Get_BestScore());
        }
    }

    // 콤보 누적하기
    public int Get_MyCombo()
    {
        int myc = PlayerPrefs.GetInt("MyCombo");
        return myc;
    }

    public void Set_MyCombo(int Haning_Combo)
    {
        PlayerPrefs.SetInt("MyCombo", Haning_Combo + Get_MyCombo());
    }

    // 닉네임 저장하기
    public void SaveUserName()
    {
        string username = InputTextName.text;

        if (string.IsNullOrEmpty(username))
        {
            username = "NoName";
        }

        PlayerPrefs.SetString("username", username);

        // 이미 게임이 끝났다면 랭킹 업데이트
        if (isGameover)
        {
            int finalScore = PlayerPrefs.GetInt("score");
            string rank = ScoreRank();
            UpdateRanking(finalScore, username, rank);
            PlayerPrefs.Save();
        }
    }
}

