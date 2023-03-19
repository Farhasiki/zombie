using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverPanel : BasePanel
{
    public Text txtMoney;
    public Text txtOver;
    public Button btnSure;
    public override void Init()
    {
        btnSure.onClick.AddListener(() => {
            //隐藏面板
            UIManager.Instance.HidePanel<GameoverPanel>(true);
            UIManager.Instance.HidePanel<GamePanel>(true);
            //切换场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("BeginScene");
            //清空关卡数据
            GameLeveLMgr.Instance.Reset();
            //恢复时间
            Time.timeScale = 1;
        });
    }

    public void UpdateData(int money,bool isWin){
        txtMoney.text = "￥" + money;
        txtOver.text = isWin ? "胜利" : "失败";

        //改变玩家数据
        DataManager.Instance.playerData.currentMoney += money;
        DataManager.Instance.SavePlayerData();
    }
}
