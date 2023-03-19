using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnQuit;
    public Toggle togMusic;
    public Toggle tgoSound;
    public Slider sliderMusic;
    public Slider sliderSound;
    public override void Init()
    {
        //初始化开关
        togMusic.isOn = DataManager.Instance.musicData.musicOpen;
        tgoSound.isOn = DataManager.Instance.musicData.soundOpen;
        //初始化大小
        sliderMusic.value = DataManager.Instance.musicData.musicValue;
        sliderSound.value = DataManager.Instance.musicData.soundValue;

        btnQuit.onClick.AddListener(() => {
            DataManager.Instance.SaveMusicData();
            UIManager.Instance.HidePanel<SettingPanel>(true);
        });
        togMusic.onValueChanged.AddListener((v) => {
            //设置背景音乐开关
            BKMusic.Instance.SetIsOpen(v);
            //数据保存到manager里
            DataManager.Instance.musicData.musicOpen = v;
        });
        tgoSound.onValueChanged.AddListener((v) => {
            //数据保存到manager里
            DataManager.Instance.musicData.soundOpen = v;
        });
        
        sliderMusic.onValueChanged.AddListener((v) => {
            //设置音乐大小
            BKMusic.Instance.ChangeMusic(v);
            //数据存储到manager里
            DataManager.Instance.musicData.musicValue = v;
        });
        sliderSound.onValueChanged.AddListener((v) => {
            //数据存储到manager里
            DataManager.Instance.musicData.soundValue = v;
        });
    }
}
