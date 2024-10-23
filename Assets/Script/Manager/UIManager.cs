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
    [SerializeField] private bool isMainScene = false; // Inspector에서 설정

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

    public GameObject Menu; // 메뉴 버튼 클릭시 활성화될 메뉴창
    public GameObject Option; // 옵션 버튼 클릭시 활성화될 옵션창
    public GameObject QuitPopUp; // 게임 종료버튼 클릭시 활성화될 경고창
    public GameObject HandMenu;

    public Image songImage; // 노래에 맞는 이미지를 표시

    public TextMeshProUGUI scoreText; // 점수 표시
    public TextMeshProUGUI comboText; // 누적 콤보 표시
    public TextMeshProUGUI multiplierText; // 현재 점수 배수 표시

    public GameObject gameoverUI; // 게임 오버시 활성화 할 UI
    public GameObject gameclearUI; // 게임 클리어시 활성화 할 UI 
    public GameObject ErrorPopUp; // 에러 팝업창 UI
    public GameObject DateResetPopUp; // 데이터 삭제 경고 창 UI
    public GameObject ChatPopup; // 채팅창 UI

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

    // 베스트 스코어 표시
    public TextMeshProUGUI bestScoreText;
    // 획득한 점수 표시
    public TextMeshProUGUI CurScoreText_C;
    public TextMeshProUGUI CurScoreText_O;
    // 획득한 콤보 표시
    public TextMeshProUGUI CurComboText_C;
    public TextMeshProUGUI CurComboText_O;
    // 랭크 표시
    public TextMeshProUGUI RankText_C;
    public TextMeshProUGUI RankText_O;

    // DOTeween을 이용한 베스트스코어 색상 변환
    public GameObject doteweenColor;

    public AudioMixer masterMixer; // 오디오 조절을 위한 믹서
    public Slider BGMSlider; // BGM 조절 슬라이더
    public Slider AudioSlider; // SFX 조절 슬라이더

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

    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "" + newScore;
    }

    // 콤보 텍스트 갱신
    public void UpdateComboText(int newCombo)
    {
        comboText.text = "" + newCombo;
    }

    // 점수 배수 갱신
    public void UpdatemultiplierText(int newmultiplier)
    {
        multiplierText.text = "X " + newmultiplier;
    }


    // 게임 클리어 UI 활성화
    public void SetActiveGameClearUI(bool active)
    {
        gameclearUI.SetActive(active);
    }

    // 게임 오버 UI 활성화
    public void SetActiveGameOverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    // 베스트 스코어 텍스트 변경
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

    // 현재 획득한 점수 표시
    public void Cur_Score()
    {
        int cscore = PlayerPrefs.GetInt("score");
        CurScoreText_C.text = "" + cscore;
        CurScoreText_O.text = "" + cscore;
    }
    // 현재 획득한 콤보 표시
    public void Cur_Combo()
    {
        int ccombo = PlayerPrefs.GetInt("MaxCombo");
        CurComboText_C.text = "" + ccombo;
        CurComboText_O.text = "" + ccombo;
    }

    // 랭크 표시
    public void Cur_Rank(string newRank)
    {
        RankText_C.text = newRank;
        RankText_O.text = newRank;
    }

    // 게임 재시작
    public void GameRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 게임 종료
    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 씬 전환
    public void LoadScene(string sceneId)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneId);
    }

    // 선택한 노래에 맞는 씬을 로드하기
    public void LoadSelectedSongScene()
    {
        switch (SongDataManager.instance.CurSong)
        {
            case Songs.song1:
                SceneManager.LoadScene("I don't Think so"); // Song1에 해당하는 씬 이름
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
                Debug.LogError("노래에 맞는 씬이 없습니다.");
                break;
        }
    }

    // 메뉴창 활성화
    public void OpenMenu()
    {
        Menu.SetActive(true);
        SoundManager.instance.SoundAllPause();
        Time.timeScale = 0f;
    }
    // 메뉴창 비활성화
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


    // 옵션창 활성화
    public void OpenOption()
    {
        Option.SetActive(true);
    }

    // 옵션창 비활성화
    public void CloseOption()
    {
        Option.SetActive(false);
    }

    // 에러창 활성화
    public void OpenError()
    {
        ErrorPopUp.SetActive(true);
    }

    // 에러창 비활성화
    public void CloseError()
    {
        ErrorPopUp.SetActive(false);
    }

    // 경고창 활성화
    public void OpenQuit()
    {
        QuitPopUp.SetActive(true);
    }

    // 경고창 비활성화
    public void CloseQuit()
    {
        QuitPopUp.SetActive(false);
    }

    // 데이터 삭제 버튼
    public void Delete_btn()
    {
        PlayerPrefs.DeleteAll();
    }

    // 데이터 삭제 경고창 활성화
    public void DateResetPopUpOpen()
    {
        DateResetPopUp.SetActive(true);
    }

    // 데이터 삭제 경고창 비활성화
    public void DateResetPopUpClose()
    {
        DateResetPopUp.SetActive(false);
    }

    // 채팅창 활성화
    public void ChatOpen()
    {
        ChatPopup.SetActive(true);
    }

    // 채팅창 비활성화
    public void ChatClose()
    {
        ChatPopup.SetActive(false);
    }

    public void UpdateRankingDisplay()
    {
        // 저장된 상위 5명의 랭킹 정보를 불러와서 UI에 표시
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


    // 소리 뮤트하는 토글
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
