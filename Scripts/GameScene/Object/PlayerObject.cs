using System.Security;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    private int money = 0;
    private int atk;
    public float rotateSpeed = 30f;
    private Animator animator;
    //持枪对象开火点
    public Transform firePoint;
    private void Start() {
        animator = GetComponent<Animator>();
    }

    //初始化玩家信息
    public void  init(int money,int atk){
        this.atk = atk;
        this.money = money;
        UpdateMoney();
    }

    // Update is called once per frame
    private void Update() {
        //更新动作信息
        //人物应用的时动画位移
        animator.SetFloat("HSpeed",Input.GetAxis("Horizontal"));
        animator.SetFloat("VSpeed",Input.GetAxis("Vertical"));
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            animator.SetTrigger("Roll");
        }
        //旋转玩家视角
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
        //控制角色下蹲
        animator.SetLayerWeight(2,Mathf.Lerp(animator.GetLayerWeight(2),Input.GetKey(KeyCode.LeftShift) ? 1:0,.1f));
        //控制玩家开火
        if(Input.GetMouseButton(0)){
            animator.SetTrigger("Fire");
        }
    }
    public void KnifeEvent(){
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward + transform.up,1,1 << LayerMask.NameToLayer("Monster"));
        DataManager.Instance.PlaySound("Music/Knife");
        foreach (Collider collider in colliders)
        {
            TakeDamage(collider);
        }
    }
    public void ShootEvent(){
        RaycastHit[] hits;
        animator.ResetTrigger("Fire");
        hits = Physics.RaycastAll(firePoint.position,firePoint.forward,10000,1 << LayerMask.NameToLayer("Monster"));

        DataManager.Instance.PlaySound("Music/Gun");
        
        foreach(RaycastHit hit in hits){
            //触发特效
            GameObject effObj = Instantiate(Resources.Load<GameObject>(DataManager.Instance.currentRole.hitEff));
            effObj.transform.position = hit.point;
            Destroy(effObj,1);

            TakeDamage(hit.collider);
            break;
        }
    }
    //更新玩家局内金币数量
    private void UpdateMoney(){
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }

    public void changeMoney(int money){
        this.money += money;
        UpdateMoney();
    }
    public int GetMoney(){
        return money;
    }

    private void TakeDamage(Collider collider){
        MonsterObject obj =  collider.GetComponent<MonsterObject>();
        if(obj == null || obj.isDead)return ;
        obj.Wound(atk);
    }

}
