using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnBegin;
    public Button btnSetting;
    public Button btnAbout;
    public Button btnQuit;
    public override void Init() 
    {
        btnBegin.onClick.AddListener(() => {
            UIManager.Instance.HidePanel<BeginPanel>(true);
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() => {
                //旋转摄像机后,显示选角面板
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();
            });

        });
        btnSetting.onClick.AddListener(() => {
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
        btnAbout.onClick.AddListener(() => {
            //转跳到github浏览界面
        });
        btnQuit.onClick.AddListener(() => {//退出游戏
            Application.Quit();
        });

        
    }
}
