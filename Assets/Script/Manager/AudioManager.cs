using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();

    public static AudioManager instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<AudioManager>();
            }
            return m_instance;
        }
    }

    private static AudioManager m_instance;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InitAudioClips();
        }
    }

    private void InitAudioClips()
    {
        foreach(var clip in audioClips)
        {
            audioDict.Add(clip.name, clip);
        }
    }

    // 음소거
    public void SoundAllMute()
    {
        audioSource.mute = true;
    }

    // 음소거 해제
    public void SoundAllPlay()
    {
        audioSource.mute = false;
    }

    // 재생
    public void PlaySound(string audioName)
    {
        if(audioDict.ContainsKey(audioName))
        {
            audioSource.clip = audioDict[audioName];
            audioSource.PlayOneShot(audioSource.clip);
        }
        else
        {
            Debug.LogWarning("no clip: " + audioName);
        }
    }

    // 멈추기
    public void StopSound()
    {
        audioSource.Stop();
    }
}
