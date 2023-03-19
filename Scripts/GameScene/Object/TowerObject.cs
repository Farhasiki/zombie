using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObject : MonoBehaviour
{
    public Transform head;//头部
    public List<Transform> firePoints;//开火点
    private TowerInfo towerInfo;//炮台关联数据
    private MonsterObject targetObj;//当前目标
    private List<MonsterObject> targetObjs;//当前目标
    private float roundSpeed = 20;//炮塔旋转速度
    private float lastAtk = 0;
    private Vector3 monsterPos;

    public void initInfo(TowerInfo info){
        this.towerInfo = info;
    }

    // Update is called once per frame
    void Update()
    {
        if(towerInfo.type == 1){//可抽象
            if(targetObj == null || targetObj.isDead || 
                Vector3.Distance(targetObj.transform.position,transform.position) > towerInfo.atkRange){
                print(towerInfo.atkRange);
                targetObj = GameLeveLMgr.Instance.FindMonster(transform.position,towerInfo.atkRange);
                print(targetObj);
            }
            if(targetObj == null)return ;
            //获取目标点
            monsterPos = targetObj.transform.position;
            monsterPos.y = head.position.y;
            head.rotation = Quaternion.Lerp(head.rotation,Quaternion.LookRotation(monsterPos - head.position),roundSpeed * Time.deltaTime);

            if(Vector3.Angle(head.forward,monsterPos - head.position) < 5 && 
                Time.time - lastAtk >= towerInfo.offsetTime){

                targetObj.Wound(towerInfo.atk * firePoints.Count);
                //播放音效
                DataManager.Instance.PlaySound("Music/Bomb");
                //加载特效
                CreateEff();
                //记录攻击事件
                lastAtk = Time.time;
            }

        }else if(towerInfo.type == 2){
            targetObjs = GameLeveLMgr.Instance.FindMonsters(head.transform.position,towerInfo.atkRange);
            if(Time.time - lastAtk >= towerInfo.offsetTime){
                //创建开火特效
                GameObject obj = Instantiate(Resources.Load<GameObject>(towerInfo.eff),head.position,head.rotation);   
                GameObject.Destroy(obj,.3f);
                for (int i = 0; i < targetObjs.Count; i++){
                    targetObjs[i].Wound(towerInfo.atk);
                }

            }
        }
    }
    /// <summary>
    /// 创建特效
    /// </summary>
    private void CreateEff(){
        foreach(Transform pos in firePoints){
            GameObject obj = Instantiate(Resources.Load<GameObject>(towerInfo.eff),pos.position,pos.rotation);
            GameObject.Destroy(obj,.3f);
        }
    }
}
