using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChooseHeroPanel : BasePanel
{
    //选择角色按钮
    public Button btnLeft;
    public Button btnRight;

    //返回开始按钮
    public Button btnStart;
    public Button btnBack;
    public Text txtMoney;//玩家金币
    public Text txtName;//角色名字

    //购买英雄
    public Button btnUnlock;
    public Text txtUnlock;
    private Transform posPoint;//英雄起始位置

    private GameObject heroObj;
    private RoleInfo nowRoleInfo;
    private int nowIndx = 0;

    public override void Init()
    {
        posPoint = GameObject.Find("posPoint").transform;

        //更新玩家拥有金币数
        txtMoney.text = DataManager.Instance.playerData.currentMoney.ToString();

        int cnt = DataManager.Instance.roleInfos.Count;//角色数目

        btnBack.onClick.AddListener(() => {
            //隐藏当前面板
            UIManager.Instance.HidePanel<ChooseHeroPanel>(true);

            Camera.main.GetComponent<CameraAnimator>().TurnRight(()=>{
                //旋转结束后显示开始界面
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });
        btnStart.onClick.AddListener(() => {
            //选定当前角色
            DataManager.Instance.currentRole = nowRoleInfo;

            //隐藏当前面板,切换到选景面板
            UIManager.Instance.HidePanel<ChooseHeroPanel>(true);
            UIManager.Instance.ShowPanel<ChooseScenePanel>();

        });
        btnLeft.onClick.AddListener(() => {
            nowIndx = (nowIndx + cnt - 1) % cnt;
            //放置角色
            ChangeRole();

        });
        btnRight.onClick.AddListener(() => {
            nowIndx = (nowIndx + 1) % cnt;
            //放置角色
            ChangeRole();
        });

        btnUnlock.onClick.AddListener(() => {
            PlayerData data = DataManager.Instance.playerData;

            if(data.currentMoney >= nowRoleInfo.lockMoney){
                data.currentMoney -= nowRoleInfo.lockMoney;

                txtMoney.text = data.currentMoney.ToString();//更改交易后金钱
                data.buyHero |= (1 << nowIndx);//存储购买的英雄
                DataManager.Instance.SavePlayerData();

                UpdateLockBtn();

                UIManager.Instance.ShowPanel<TipPanel>().ChanegeInfo("购买成功");
            }else{
                UIManager.Instance.ShowPanel<TipPanel>().ChanegeInfo("金币不足");
            }
        });
        ChangeRole();
    }

    private void ChangeRole(){//左右按钮选择角色
        if(heroObj != null)Destroy(heroObj);
        nowRoleInfo = DataManager.Instance.roleInfos[nowIndx];
        txtName.text = nowRoleInfo.tips;

        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleInfo.res),posPoint.position,posPoint.rotation);
        //在选景时不需要控制角色
        Destroy(heroObj.GetComponent<PlayerObject>());
        
        //选取不同角色时更新选角界面
        UpdateLockBtn();
    }

    private void UpdateLockBtn(){
        if((DataManager.Instance.playerData.buyHero >> nowIndx & 1) == 1){//玩家拥有这名角色
            btnUnlock.gameObject.SetActive(false);//隐藏购买按钮
            btnStart.gameObject.SetActive(true);//开启开始按钮
        }else{
            btnStart.gameObject.SetActive(false);//关闭开始按钮
            btnUnlock.gameObject.SetActive(true);//开启购买按钮
            
            txtUnlock.text = "￥" + nowRoleInfo.lockMoney;
        }   
    }

    public override void HideMe(UnityAction callBack)
    {
        if(heroObj != null){
            DestroyImmediate(heroObj);
            heroObj = null;
        }
        base.HideMe(callBack);
    }
}
