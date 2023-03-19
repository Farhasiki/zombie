using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager{
    private static DataManager instance = new DataManager();
    public static DataManager Instance => instance;
    //音乐相关数据
    public MusicData musicData;
    //角色信息
    public List<RoleInfo> roleInfos;
    //玩家相关数据
    public PlayerData playerData;
    //场景数据
    public List<SceneInfo> sceneInfos;
    //怪物数据
    public List<MonsterInfo> monsterInfos;
    //当前选中玩家
    public RoleInfo currentRole;
    //存储塔的数据
    public List<TowerInfo> towerInfos;
    
    private DataManager(){
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        roleInfos = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        sceneInfos = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        monsterInfos = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        towerInfos = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");
    }
    //存储音乐数据
    public void SaveMusicData(){
        JsonMgr.Instance.SaveData(musicData,"MusicData");
    }
    //存储玩家数据
    public void SavePlayerData(){
        JsonMgr.Instance.SaveData(playerData,"PlayerData");
    }
    public void PlaySound(string resName){
        GameObject musicObj = new GameObject();
        AudioSource a = musicObj.AddComponent<AudioSource>();
        a.clip = Resources.Load<AudioClip>(resName);
        a.volume = musicData.musicValue;
        a.mute = !musicData.soundOpen;
        a.Play();

        GameObject.Destroy(musicObj,1);
    }
}
