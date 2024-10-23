using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private bool isMainScene = false; // Inspector���� ����

    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }

    private static UIManager m_instance;

    public GameObject Menu; // �޴� ��ư Ŭ���� Ȱ��ȭ�� �޴�â
    public GameObject Option; // �ɼ� ��ư Ŭ���� Ȱ��ȭ�� �ɼ�â
    public GameObject QuitPopUp; // ���� �����ư Ŭ���� Ȱ��ȭ�� ���â
    public GameObject HandMenu;

    public Image songImage; // �뷡�� �´� �̹����� ǥ��

    public TextMeshProUGUI scoreText; // ���� ǥ��
    public TextMeshProUGUI comboText; // ���� �޺� ǥ��
    public TextMeshProUGUI multiplierText; // ���� ���� ��� ǥ��

    public GameObject gameoverUI; // ���� ������ Ȱ��ȭ �� UI
    public GameObject gameclearUI; // ���� Ŭ����� Ȱ��ȭ �� UI 
    public GameObject ErrorPopUp; // ���� �˾�â UI
    public GameObject DateResetPopUp; // ������ ���� ��� â UI
    public GameObject ChatPopup; // ä��â UI

    public TextMeshProUGUI rank1NameText;
    public TextMeshProUGUI rank1ScoreText;
    public TextMeshProUGUI rank1RankText;
    public TextMeshProUGUI rank2NameText;
    public TextMeshProUGUI rank2ScoreText;
    public TextMeshProUGUI rank2RankText;
    public TextMeshProUGUI rank3NameText;
    public TextMeshProUGUI rank3ScoreText;
    public TextMeshProUGUI rank3RankText;
    public TextMeshProUGUI rank4NameText;
    public TextMeshProUGUI rank4ScoreText;
    public TextMeshProUGUI rank4RankText;
    public TextMeshProUGUI rank5NameText;
    public TextMeshProUGUI rank5ScoreText;
    public TextMeshProUGUI rank5RankText;

    // ����Ʈ ���ھ� ǥ��
    public TextMeshProUGUI bestScoreText;
    // ȹ���� ���� ǥ��
    public TextMeshProUGUI CurScoreText_C;
    public TextMeshProUGUI CurScoreText_O;
    // ȹ���� �޺� ǥ��
    public TextMeshProUGUI CurComboText_C;
    public TextMeshProUGUI CurComboText_O;
    // ��ũ ǥ��
    public TextMeshProUGUI RankText_C;
    public TextMeshProUGUI RankText_O;

    // DOTeween�� �̿��� ����Ʈ���ھ� ���� ��ȯ
    public GameObject doteweenColor;

    public AudioMixer masterMixer; // ����� ������ ���� �ͼ�
    public Slider BGMSlider; // BGM ���� �����̴�
    public Slider AudioSlider; // SFX ���� �����̴�

    private void Start()
    {
        if (isMainScene)
        {
            UpdateRankingDisplay();
        }
        
    }

    private void Update()
    {
        if (isMainScene)
        {
            UpdateRankingDisplay();
        }
    }

    // ���� �ؽ�Ʈ ����
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "" + newScore;
    }

    // �޺� �ؽ�Ʈ ����
    public void UpdateComboText(int newCombo)
    {
        comboText.text = "" + newCombo;
    }

    // ���� ��� ����
    public void UpdatemultiplierText(int newmultiplier)
    {
        multiplierText.text = "X " + newmultiplier;
    }


    // ���� Ŭ���� UI Ȱ��ȭ
    public void SetActiveGameClearUI(bool active)
    {
        gameclearUI.SetActive(active);
    }

    // ���� ���� UI Ȱ��ȭ
    public void SetActiveGameOverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    // ����Ʈ ���ھ� �ؽ�Ʈ ����
    public void UpdateBestScore(int cur_score)
    {
        bestScoreText.text = "" + cur_score;
        doteweenColor.GetComponent<DOTweenAnimation>().enabled = true;
    }

    public void UnUpdateBestScore(int score)
    {
        bestScoreText.text = "" + score;
        doteweenColor.GetComponent<DOTweenAnimation>().enabled = false;
    }

    // ���� ȹ���� ���� ǥ��
    public void Cur_Score()
    {
        int cscore = PlayerPrefs.GetInt("score");
        CurScoreText_C.text = "" + cscore;
        CurScoreText_O.text = "" + cscore;
    }
    // ���� ȹ���� �޺� ǥ��
    public void Cur_Combo()
    {
        int ccombo = PlayerPrefs.GetInt("MaxCombo");
        CurComboText_C.text = "" + ccombo;
        CurComboText_O.text = "" + ccombo;
    }

    // ��ũ ǥ��
    public void Cur_Rank(string newRank)
    {
        RankText_C.text = newRank;
        RankText_O.text = newRank;
    }

    // ���� �����
    public void GameRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ���� ����
    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // �� ��ȯ
    public void LoadScene(string sceneId)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneId);
    }

    // ������ �뷡�� �´� ���� �ε��ϱ�
    public void LoadSelectedSongScene()
    {
        switch (SongDataManager.instance.CurSong)
        {
            case Songs.song1:
                SceneManager.LoadScene("I don't Think so"); // Song1�� �ش��ϴ� �� �̸�
                break;
            case Songs.song2:
                SceneManager.LoadScene("Snap Shoot");
                break;
            case Songs.song3:
                SceneManager.LoadScene("Super");
                break;
            case Songs.song4:
                SceneManager.LoadScene("Maestro");
                break;
            case Songs.song5:
                SceneManager.LoadScene("Sticky");
                break;
            case Songs.song6:
                SceneManager.LoadScene("Drama");
                break;
            case Songs.song7:
                SceneManager.LoadScene("Armageddon");
                break;
            case Songs.song8:
                SceneManager.LoadScene("Supernova");
                break;
            case Songs.song9:
                SceneManager.LoadScene("Spot");
                break;
            case Songs.song10:
                SceneManager.LoadScene("Speechless");
                break;
            case Songs.song11:
                SceneManager.LoadScene("Reflection");
                break;
            case Songs.song12:
                SceneManager.LoadScene("Let It Go");
                break;
            case Songs.song13:
                SceneManager.LoadScene("Homura");
                break;
            case Songs.song14:
                SceneManager.LoadScene("Tanjiro no Uta");
                break;
            case Songs.song15:
                SceneManager.LoadScene("Betelgeuse");
                break;
            case Songs.song16:
                SceneManager.LoadScene("Beethoven Virus");
                break;
            case Songs.song17:
                SceneManager.LoadScene("He's a pirate");
                break;
            case Songs.song18:
                SceneManager.LoadScene("Yoon's Birthday");
                break;
            default:
                Debug.LogError("�뷡�� �´� ���� �����ϴ�.");
                break;
        }
    }

    // �޴�â Ȱ��ȭ
    public void OpenMenu()
    {
        Menu.SetActive(true);
        SoundManager.instance.SoundAllPause();
        Time.timeScale = 0f;
    }
    // �޴�â ��Ȱ��ȭ
    public void CloseMenu()
    {
        Menu.SetActive(false);
        SoundManager.instance.SoundAllUnPause();
        Time.timeScale = 1f;
    }

    public void OpneHandMenu()
    {
        HandMenu.SetActive(true);
    }

    public void CloseHandMenu()
    {
        HandMenu.SetActive(false);
    }


    // �ɼ�â Ȱ��ȭ
    public void OpenOption()
    {
        Option.SetActive(true);
    }

    // �ɼ�â ��Ȱ��ȭ
    public void CloseOption()
    {
        Option.SetActive(false);
    }

    // ����â Ȱ��ȭ
    public void OpenError()
    {
        ErrorPopUp.SetActive(true);
    }

    // ����â ��Ȱ��ȭ
    public void CloseError()
    {
        ErrorPopUp.SetActive(false);
    }

    // ���â Ȱ��ȭ
    public void OpenQuit()
    {
        QuitPopUp.SetActive(true);
    }

    // ���â ��Ȱ��ȭ
    public void CloseQuit()
    {
        QuitPopUp.SetActive(false);
    }

    // ������ ���� ��ư
    public void Delete_btn()
    {
        PlayerPrefs.DeleteAll();
    }

    // ������ ���� ���â Ȱ��ȭ
    public void DateResetPopUpOpen()
    {
        DateResetPopUp.SetActive(true);
    }

    // ������ ���� ���â ��Ȱ��ȭ
    public void DateResetPopUpClose()
    {
        DateResetPopUp.SetActive(false);
    }

    // ä��â Ȱ��ȭ
    public void ChatOpen()
    {
        ChatPopup.SetActive(true);
    }

    // ä��â ��Ȱ��ȭ
    public void ChatClose()
    {
        ChatPopup.SetActive(false);
    }

    public void UpdateRankingDisplay()
    {
        // ����� ���� 5���� ��ŷ ������ �ҷ��ͼ� UI�� ǥ��
        for (int i = 0; i < 5; i++)
        {
            string rankName = PlayerPrefs.GetString("rank_name" + i, "NoName");
            int rankScore = PlayerPrefs.GetInt("rank_score" + i, 0);
            string rankRank = PlayerPrefs.GetString("rank_rank" + i, "NoRank");

            switch (i)
            {
                case 0:
                    if (rank1NameText.text != rankName ||
                        rank1ScoreText.text != rankScore.ToString() ||
                        rank1RankText.text != rankRank)
                    {
                        rank1NameText.text = rankName;
                        rank1ScoreText.text = rankScore > 0 ? rankScore.ToString() : "---";
                        rank1RankText.text = rankRank;
                    }
                    break;
                case 1:
                    if (rank2NameText.text != rankName ||
                        rank2ScoreText.text != rankScore.ToString() ||
                        rank2RankText.text != rankRank)
                    {
                        rank2NameText.text = rankName;
                        rank2ScoreText.text = rankScore > 0 ? rankScore.ToString() : "---";
                        rank2RankText.text = rankRank;
                    }
                    break;
                case 2:
                    if (rank3NameText.text != rankName ||
                        rank3ScoreText.text != rankScore.ToString() ||
                        rank3RankText.text != rankRank)
                    {
                        rank3NameText.text = rankName;
                        rank3ScoreText.text = rankScore > 0 ? rankScore.ToString() : "---";
                        rank3RankText.text = rankRank;
                    }
                    break;
                case 3:
                    if (rank4NameText.text != rankName ||
                        rank4ScoreText.text != rankScore.ToString() ||
                        rank4RankText.text != rankRank)
                    {
                        rank4NameText.text = rankName;
                        rank4ScoreText.text = rankScore > 0 ? rankScore.ToString() : "---";
                        rank4RankText.text = rankRank;
                    }
                    break;
                case 4:
                    if (rank5NameText.text != rankName ||
                        rank5ScoreText.text != rankScore.ToString() ||
                        rank5RankText.text != rankRank)
                    {
                        rank5NameText.text = rankName;
                        rank5ScoreText.text = rankScore > 0 ? rankScore.ToString() : "---";
                        rank5RankText.text = rankRank;
                    }
                    break;
            }
        }
    }


    // �Ҹ� ��Ʈ�ϴ� ���
    public void MuteToggle(bool isOn)
    {
        if (isOn)
        {
            SoundManager.instance.SoundAllOn();
        }
        else
        {
            SoundManager.instance.SoundAllMute();
        }
    }

    public void AudioControl()
    {
        float sound1 = BGMSlider.value;
        float sound2 = AudioSlider.value;

        if (sound1 == -40f)
        {
            masterMixer.SetFloat("BGM", -80);
        }
        else
        {
            masterMixer.SetFloat("BGM", sound1);
        }

        if (sound2 == -40f)
        {
            masterMixer.SetFloat("Audio", -80);
        }
        else
        {
            masterMixer.SetFloat("Audio", sound2);
        }
    }
}
