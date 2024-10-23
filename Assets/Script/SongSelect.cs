using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SongSelect : MonoBehaviour
{
    public Songs song;
    public SongSelect[] S;
    public Sprite songSprite;

    void Start()
    {
        if (SongDataManager.instance.CurSong == song)
        {
            OnSelect();
        }
        else
        {
            OnDeSelect();
        }
    }

    public void OnSelect()
    {
        SongDataManager.instance.CurSong = song;

        UIManager.instance.songImage.sprite = songSprite;

        for (int i = 0; i < S.Length; i++)
        {
            if (S[i] != this)
            {
                S[i].OnDeSelect();
            }
        }
    }

    public void OnDeSelect()
    {
        
    }
}
