using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource BGMsource;

    public static SoundManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SoundManager>();
            }
            return m_instance;
        }
    }

    private static SoundManager m_instance;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SoundAllMute()
    {
        BGMsource.mute = true;
    }

    public void SoundAllOn()
    {
        BGMsource.mute = false;
    }

    public void SoundAllPause()
    {
        BGMsource.Pause();
    }

    public void SoundAllUnPause()
    {
        BGMsource.UnPause();
    }

    // 노래의 길이 측정
    public float GetBGMLength()
    {
        if (BGMsource.clip != null)
        {
            return BGMsource.clip.length;
        }
        return 0; // 노래가 없으면 0 반환하기
    }
}
