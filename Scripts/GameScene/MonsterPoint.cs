using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    //怪物波数
    public int maxWave;
    private int nowWave;
    //每波有多少怪物
    public int[] monsterNumPerWave;
    //怪物Id，再数组中随机生成怪物
    public List<int> monstersIDStart,monstersIDEnd;
    //当前波召唤怪物ID
    private int nowID;
    //单只怪物之间的创建间隔时间
    public float creatOffsetTime;
    //波与波之间的时间
    public float deltaTime;
    //第一波创建的时间
    public float firstDeltaTime;
    private bool isOver = false; 
    void Awake()
    {
        Invoke("CreateWave",firstDeltaTime);
        GameLeveLMgr.Instance.SetMonsterPoint(this);
    }

    
    private void CreateWave(){
        //当前波数增加
        nowWave ++;
        GameLeveLMgr.Instance.UpdateNowNum(nowWave);
        //创建怪物
        CreateMonster();
    }

    private void CreateMonster(){
        //创建怪物
        for(int j = 0; j < transform.childCount; ++j){
            //当前怪物ID
            nowID = Random.Range(monstersIDStart[nowWave - 1],monstersIDEnd[nowWave - 1]);
            //获取怪物数据
            MonsterInfo monsterInfo = DataManager.Instance.monsterInfos[nowID];
            Transform child = transform.GetChild(j);
            GameObject obj =  Instantiate(Resources.Load<GameObject>(monsterInfo.res),child.position,Quaternion.identity);  
            //GameLeveLMgr.Instance.ChangeMonsterNum(1);   
            //增加怪物数量
            MonsterObject monsterObject = obj.AddComponent<MonsterObject>();
            GameLeveLMgr.Instance.AddMonster(monsterObject);
            //初始化怪物数据
            monsterObject.init(monsterInfo);
        }
        
        //当前剩余刷怪数
        ref int nowNum = ref monsterNumPerWave[nowWave - 1];
        nowNum--;
        //检测是否生成怪物
        if(nowNum == 0){
            if(nowWave == maxWave){
                isOver = true;
                return ;
            }
            Invoke("CreateWave",deltaTime);
        }else {
            Invoke("CreateMonster",creatOffsetTime);
        }
    }

    public bool CheckOver(){
        return isOver;
    }

    public void setMaxWave(int num){
        maxWave = num;
    }
}
