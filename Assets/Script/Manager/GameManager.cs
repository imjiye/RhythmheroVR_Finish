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

    public InputField InputTextName; // �÷��̾� �г��� �Է� UI
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

    //// EndGame�� �뷡�� ������ 1�� �ڿ� ȣ���ϰ� �ϴ� �ڷ�ƾ
    //private IEnumerator EndGameAfterBGM()
    //{
    //    float bgmLength = SoundManager.instance.GetBGMLength();

    //    yield return new WaitForSeconds(bgmLength);

    //    yield return new WaitForSeconds(1f);

    //    GameClear();
    //}

    // �޺��� ���� ��� ��� �Լ�
    public int GetComboMultiplier()
    {
        if(combo >= 35)
        {
            return 5; // 35 �޺� �̻��� 5��
        }
        else if(combo >= 25)
        {
            return 4; // 25 ~ 34 �޺��� 4��
        }
        else if(combo >= 15)
        {
            return 3; // 15 ~ 24 �޺��� 3��
        }
        else if(combo >= 6)
        {
            return 2; // 6 ~ 14 �޺��� 2��
        }
        return 1; // �޺��� 5 ������ ��� ��� ����
    }

    // ������ �߰��ϰ� UI ����
    public void AddScore(int newScore)
    {
        // ���� ������ �ƴ� ���¿����� ���� ���� ����
        if (!isGameover)
        {
            // ������ �޺� ��� �����ϱ�
            int multiplier = GetComboMultiplier();
            UIManager.instance.UpdatemultiplierText(multiplier);
            int finalScore = Mathf.RoundToInt(newScore * multiplier);

            // ���� �߰�
            score += finalScore;

            // ���� ���� ����
            PlayerPrefs.SetInt("score", score);
            // ���� UI �ؽ�Ʈ ����
            UIManager.instance.UpdateScoreText(score);
        }
    }

    // �޺��� �߰��ϰ� UI ����
    public void AddCombo(int newCombo)
    {
        if (!isGameover)
        {
            // �޺� �߰�
            combo += newCombo;

            // �ְ� �޺� ����
            if(combo > maxcombo)
            {
                maxcombo = combo;

                PlayerPrefs.SetInt("MaxCombo", maxcombo);
            }

            // �޺� ���� ����
            PlayerPrefs.SetInt("combo", combo);
            // �޺� UI �ؽ�Ʈ ����
            UIManager.instance.UpdateComboText(combo);
        }
    }

    // �޺� ���� & ������ ����
    public void ResetCombo()
    {
        combo = 0;
        // �޺� UI �ؽ�Ʈ ����
        UIManager.instance.UpdateComboText(combo);

        energy -= damage;
        EnergyManager.instance.UpdateEnergyBar(damage);
    }

    // ���� Ŭ���� ó��
    public void GameClear()
    {
        isGameover = true;

        // �����ִ� ��� ť�� �����ϱ�
        CubeNote.DestroyAllCube();

        RedcubeSpawn.SetActive(false);
        BluecubeSpawn.SetActive(false);

        // ���� ȹ�� ����
        int finalScore = PlayerPrefs.GetInt("score");

        // ���� ������ �ְ� ������ ����
        Set_BestScore(finalScore);

        Set_MyCombo(combo);
                
        string scoreRank = ScoreRank();

        // �г����� �Է¹޾� �����ϱ�
        //SaveUserName();
        string username = PlayerPrefs.GetString("username", "NoName");

        Debug.Log($"Game Clear - Score: {finalScore}, Username: {username}, Rank: {scoreRank}");

        UIManager.instance.SetActiveGameClearUI(true);
        UIManager.instance.Cur_Score();
        UIManager.instance.Cur_Combo();
        UIManager.instance.Cur_Rank(scoreRank);

        // ��ŷ ������Ʈ �Լ� ȣ��
        UpdateRanking(finalScore, username, scoreRank);

        // PlayerPrefs ������� ��� ����
        PlayerPrefs.Save();
    }

    // ���� ���� ó��
    public void GameOver()
    {
        isGameover = true;

        // �����ִ� ��� ť�� �����ϱ�
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

        // ���� ����� ���� 5���� ������ �г����� �ҷ���
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

        // ���� ������ ���� 5�� �ȿ� �� �� �ִ��� üũ
        bool rankUpdated = false;
        for (int i = 0; i < 5; i++)
        {
            if (score > topScores[i])
            {
                // ���� ���� ������ ������ ��� ���� ������ �� ĭ�� ����
                for (int j = 2; j > i; j--)
                {
                    topScores[j] = topScores[j - 1];
                    topNames[j] = topNames[j - 1];
                    topRank[j] = topRank[j - 1];
                }
                // ���ο� ������ �ش� ������ ����
                topScores[i] = score;
                topNames[i] = username;
                topRank[i] = rank;
                rankUpdated = true;
                Debug.Log($"New ranking inserted at position {i}");
                break;
            }
        }

        // PlayerPrefs�� ������Ʈ�� ��ŷ ����
        if (rankUpdated)
        {
            for (int i = 0; i < 5; i++)
            {
                PlayerPrefs.SetInt("rank_score" + i, topScores[i]);
                PlayerPrefs.SetString("rank_name" + i, topNames[i]);
                PlayerPrefs.SetString("rank_rank" + i, topRank[i]);
                // ����� �α� �߰�
                Debug.Log($"Saved Rank {i}: Score={topScores[i]}, Name={topNames[i]}, Rank={topRank[i]}");
            }
            PlayerPrefs.Save();  // ������� ��� ����
        }
    }

    // �ְ� ���� �����ϱ�
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

    // �޺� �����ϱ�
    public int Get_MyCombo()
    {
        int myc = PlayerPrefs.GetInt("MyCombo");
        return myc;
    }

    public void Set_MyCombo(int Haning_Combo)
    {
        PlayerPrefs.SetInt("MyCombo", Haning_Combo + Get_MyCombo());
    }

    // �г��� �����ϱ�
    public void SaveUserName()
    {
        string username = InputTextName.text;

        if (string.IsNullOrEmpty(username))
        {
            username = "NoName";
        }

        PlayerPrefs.SetString("username", username);

        // �̹� ������ �����ٸ� ��ŷ ������Ʈ
        if (isGameover)
        {
            int finalScore = PlayerPrefs.GetInt("score");
            string rank = ScoreRank();
            UpdateRanking(finalScore, username, rank);
            PlayerPrefs.Save();
        }
    }
}

