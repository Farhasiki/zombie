using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerObject : MonoBehaviour
{
    private int currentHP;
    private int maxHP;

    public bool isDead;
    private static MainTowerObject instance;
    public static MainTowerObject Instance => instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start() {
        //init();
    }
    public void init(int maxHP){
        this.maxHP = maxHP;
        UpdateHP(maxHP);
    }

    private void UpdateHP(int HP){
        currentHP = HP;
        UIManager.Instance.GetPanel<GamePanel> ().UpdateTowerHP(currentHP,maxHP);
    }
    //可以加血可以减血
    public void ChangeHP(int dmg){
        if(isDead) return  ;
        currentHP = Mathf.Clamp(currentHP - dmg, 0, maxHP);
        if(currentHP == 0){
            isDead = true;
            
           GameLeveLMgr.Instance.Gameover(false);
        }
        //更新面板
        UpdateHP(currentHP);
    }


    private void OnDestroy() {
        instance = null;
    }
}
