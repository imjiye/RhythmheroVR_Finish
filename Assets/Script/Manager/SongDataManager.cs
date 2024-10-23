using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Songs
{
    song1,
    song2,
    song3,
    song4,
    song5,
    song6,
    song7,
    song8,
    song9,
    song10,
    song11,
    song12,
    song13,
    song14,
    song15,
    song16,
    song17,
    song18
}

public class SongDataManager : MonoBehaviour
{
    public static SongDataManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) return;
    }

    public Songs CurSong;
}
