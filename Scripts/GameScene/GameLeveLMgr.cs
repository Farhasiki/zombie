using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLeveLMgr
{
    private static GameLeveLMgr instance = new GameLeveLMgr();
    public static GameLeveLMgr Instance => instance;
    public PlayerObject player;
    //所有出拐点
    private MonsterPoint point;
    //当前波数；
    private int nowWaveNum = 0;
    private int maxWaveNum;
    //一共多少个怪物
    // public int nowMonsterNum = 0;
    private List<MonsterObject> monsterObjects = new List<MonsterObject>();
    private GameLeveLMgr(){
          
    }

    public void InitInfo(SceneInfo info){
        //显示gamePanel面板
        UIManager.Instance.ShowPanel<GamePanel>();
        //获取玩家数据
        //获取选择的角色信息
        RoleInfo roleInfo = DataManager.Instance.currentRole;
        //获取玩家出生点
        Transform bornPos = GameObject.Find("HeroBornPos").transform;
        //生成玩家
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res),bornPos.position,bornPos.rotation);

        //让摄像机看向选中角色
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);
        //设置场景的波数
        point.GetComponent<MonsterPoint>().setMaxWave(info.wave);
        //对角色对象进行初始化
        player = heroObj.GetComponent<PlayerObject>();
        //初始化角色信息
        player.init(info.money,roleInfo.atk);

        //初始化中央塔血量
        MainTowerObject.Instance.init(info.towerHP);

        //获取最大波数
        maxWaveNum = info.wave;
        UpdateMaxNum(maxWaveNum);
        //锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UpdateMaxNum(int num){
        maxWaveNum = num;
        //更新波数面板
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum,maxWaveNum);
    }

    public void UpdateNowNum(int num){
        nowWaveNum = num;
        //更新波数面板
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum,maxWaveNum);
    }

    public bool CheckOver(){
        return point.CheckOver() && monsterObjects.Count == 0;
    }

    public void AddMonster(MonsterObject obj){
        monsterObjects.Add(obj);
    }
    public void RemoveMonster(MonsterObject obj){
        monsterObjects.Remove(obj);
    }

    public void SetMonsterPoint(MonsterPoint point){
        this.point = point;
    }
    /// <summary>
    /// 怪物寻找攻击点
    /// </summary>
    /// <param name="pos">攻击点位置</param>
    /// <param name="range">攻击点范围</param>
    /// <returns>怪物</returns>
    public MonsterObject FindMonster(Vector3 pos, int range){
        for(int j = 0; j < monsterObjects.Count; ++j){
            MonsterObject obj = monsterObjects[j];
            if(obj.isDead || Vector3.Distance(obj.transform.position,pos) >= range)continue;
            return monsterObjects[j];
        }
        return null;
    }
    /// <summary>
    /// 寻找满足条件的所有怪物
    /// </summary>
    /// <param name="pos">攻击点位置</param>
    /// <param name="range">攻击点范围</param>
    /// <returns>怪物</returns>
    public List<MonsterObject> FindMonsters(Vector3 pos, int range){
        List<MonsterObject> list = new List<MonsterObject>();
        for(int j = 0; j < monsterObjects.Count; ++j){
            MonsterObject obj = monsterObjects[j];
            if(obj.isDead || Vector3.Distance(obj.transform.position,pos) >= range)continue;
            list.Add(monsterObjects[j]);
        }
        return list;
    }

    //重置关卡数据
    public void Reset(){
        point = null;
        // nowMonsterNum = 0;
        monsterObjects.Clear();
        player= null;
    }

    public void Gameover(bool isWin){
        Time.timeScale = 0.1f;
        GameoverPanel over = UIManager.Instance.ShowPanel<GameoverPanel>();
        over.UpdateData((int)(GameLeveLMgr.Instance.player.GetMoney() * (isWin ? 1 : 0)),isWin);
        //锁定鼠标
        Cursor.lockState = CursorLockMode.None;
    }
}
