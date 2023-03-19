using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GamePanel : BasePanel
{
    public Image imgHP;
    public Text txtHP;

    public Text txtWave;
    public Text txtMoney;

    //初始HP的宽度，在外侧控制
    public float hpW = 500;
    public Button btnQuit;

    //屏幕底部炮塔控件
    public Transform botTrans;
    //三个炮塔控件
    public List<TowerBtn> towerBtns = new List<TowerBtn>();

    //当前选中造塔点
    private TowerPoint nowTowerPoint = null;

    //控制输入
    private bool checkInput = false;


    public override void Init()
    {
        //监听返回按钮
        btnQuit.onClick.AddListener(() => {
            //关闭gamePanel
            UIManager.Instance.HidePanel<GamePanel> ();
            //切换到暂停界面

            //切换到游戏开始界面
            UnityEngine.SceneManagement.SceneManager.LoadScene("beginScene");
        });

        //一开始隐藏底部炮塔UI
        botTrans.gameObject.SetActive(false);
    }

    /// <summary>
    /// 更新安全区血量
    /// </summary>
    /// <param name="hp">当前血量</param>
    /// <param name="maxHP">总血量</param>
    public void UpdateTowerHP(int hp,int maxHP){
        txtHP.text = hp + "/" + maxHP;
        //控制血量图片
        imgHP.rectTransform.sizeDelta = new Vector2(1f * hp / maxHP * hpW,40);
    }
    /// <summary>
    /// 更新剩余波数
    /// </summary>
    /// <param name="nowNum">当前波束</param>
    /// <param name="maxNum">最大波数</param>
    public void UpdateWaveNum(int nowNum,int maxNum){
        txtWave.text = nowNum + "/" + maxNum;
    }
    /// <summary>
    /// 更新金币
    /// </summary>
    /// <param name="money">当前金币数</param>
    public void UpdateMoney(int money){
        txtMoney.text = "" + money;
    }
    /// <summary>
    /// 更新造塔点信息
    /// </summary>
    /// <param name="point">造塔点</param>
    public void UpdateSelTower(TowerPoint point){
        //记录当前塔点
        nowTowerPoint = point;
        if(point == null){//如果出塔了
            checkInput = false;
            botTrans.gameObject.SetActive(false);
        }else{//造塔
            checkInput = true;
            botTrans.gameObject.SetActive(true);
            if(nowTowerPoint.info == null){
            for (int i = 0; i < towerBtns.Count; i++){
                towerBtns[i].gameObject.SetActive(true);
                towerBtns[i].InitInfo(nowTowerPoint.chooseIDs[i],"数字键" + (i + 1));
            }
            }else{
                for (int i = 0; i < towerBtns.Count; i++){
                    towerBtns[i].gameObject.SetActive(false);
                }
                towerBtns[1].gameObject.SetActive(true);
                towerBtns[1].InitInfo(nowTowerPoint.info.next,"空格键");
            }
        }
        
    }

    public override void Update()
    {
        base.Update();
        if(!checkInput)return ; 
        if(nowTowerPoint.info == null){
            if(Input.GetKeyDown(KeyCode.Alpha1)){
                nowTowerPoint.CreateTower(nowTowerPoint.chooseIDs[0]);
            }else if(Input.GetKeyDown(KeyCode.Alpha2)){
                nowTowerPoint.CreateTower(nowTowerPoint.chooseIDs[1]);
            }else if(Input.GetKeyDown(KeyCode.Alpha3)){
                nowTowerPoint.CreateTower(nowTowerPoint.chooseIDs[2]);
            }
        }else{
            if(Input.GetKeyDown(KeyCode.Space)){
                nowTowerPoint.CreateTower(nowTowerPoint.info.next);
            }
        }
    }
}
