using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    public Image imgPic;
    public Text txtNum;
    public Text txtMoney;

    public void InitInfo(int id,string intputStr){
        TowerInfo info = DataManager.Instance.towerInfos[id - 1];
        imgPic.sprite = Resources.Load<Sprite>(info.imgRes);
        txtMoney.text = "￥" + info.money;
        txtNum.text = intputStr;
        //如果钱不够
        if(info.money > GameLeveLMgr.Instance.player.GetMoney()){
            txtMoney.text = "金币不足";
        }
    }
}


