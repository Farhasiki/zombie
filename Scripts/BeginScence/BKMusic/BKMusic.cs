using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;

    public static BKMusic Instance => instance;
    private AudioSource bkMusic;

    private void Awake() {
        instance = this;
        bkMusic = GetComponent<AudioSource> ();

        MusicData data = DataManager.Instance.musicData;
        SetIsOpen(data.musicOpen);
        ChangeMusic(data.musicValue);
    }
    //设置音乐的开关
    public void SetIsOpen(bool isOpen){
        bkMusic.mute = !isOpen;
    }
    //设置音乐的大小
    public void ChangeMusic(float musicValue){
        bkMusic.volume = musicValue;
    }
}
