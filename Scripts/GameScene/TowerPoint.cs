using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    private GameObject towerObj = null;//塔对象
    public TowerInfo info = null;//塔信息
    public List<int> chooseIDs;//选择塔的信息
    
    public void CreateTower(int id){
        //显示面板
        // UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        //获取炮塔信息
        TowerInfo temInfo = DataManager.Instance.towerInfos[id - 1];
        if(temInfo.money > GameLeveLMgr.Instance.player.GetMoney())return ;
        //扣钱
        GameLeveLMgr.Instance.player.changeMoney(-temInfo.money);

        //判断是否有塔 如果有就删除
        if(towerObj != null){
            Destroy(towerObj);
            towerObj = null;
        }
        //创建炮塔
        towerObj = Instantiate(Resources.Load<GameObject>(temInfo.res),this.transform.position,Quaternion.identity);
        //初始化信息
        towerObj.GetComponent<TowerObject>().initInfo(temInfo);
        //造完塔后更新数据
        if(temInfo.next == 0){
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
        }else{
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        }
        //记录当前数据
        this.info = temInfo;
    }

    private void OnTriggerEnter(Collider other) {
        if(info != null && info.next == 0)return ;
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
    }

    private void OnTriggerExit(Collider other) {
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
    }
}
