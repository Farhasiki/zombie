using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseScenePanel : BasePanel
{
    public Button btnLeft;
    public Button btnRight;
    public Button btnStart;
    public Button btnBack;
    public Image imgScene;
    public Text txtInfo;


    //当前场景索引
    private int nowIndx = 0;
    //当前场景信息
    private SceneInfo nowSceneInfo;
    public override void Init()
    {
        btnBack.onClick.AddListener(() => {
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            //显示角色
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
        });
        btnStart.onClick.AddListener(() => {
            //隐藏面板
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            //开始游戏,切换场景
            //因为可能出现场景未加载出来就获取场景信息，所以需要异步加载场景，
            AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);
            //进行关卡初始化,在场景加载完之后进行初始化
            ao.completed += (obj) =>{
                GameLeveLMgr.Instance.InitInfo(nowSceneInfo); 
            };
        });
        int cnt = DataManager.Instance.sceneInfos.Count;


        btnLeft.onClick.AddListener(() => {
            nowIndx = (nowIndx + cnt - 1) % cnt;
            ChangeScene();
        });
        btnRight.onClick.AddListener(() => {
            nowIndx = (nowIndx + 1) % cnt;
            ChangeScene();
        });
        ChangeScene();
    }
    /// <summary>
    /// 切换场景信息
    /// </summary>
    public void ChangeScene(){
        nowSceneInfo = DataManager.Instance.sceneInfos[nowIndx];
        imgScene.sprite = Resources.Load<Sprite>(nowSceneInfo.imgRes);
        txtInfo.text = "名字：" + nowSceneInfo.name + "\n\n"+ "介绍：" + nowSceneInfo.tips;
;    }
}
