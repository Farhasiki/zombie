using UnityEngine;
using UnityEngine.AI;

public class MonsterObject : MonoBehaviour
{
    //动画部分
    private Animator animator;
    private NavMeshAgent agent;
    // 一些不变的基础数据
    private MonsterInfo monsterInfo;
    //当前状态(可更改)
    private int HP;
    public bool isDead = false;
    // Start is called before the first frame update

    private float lastAtk = 0;
    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();    
    }

    public void init(MonsterInfo info){
        monsterInfo = info;
        //动画状态机
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);
        //数值初始化
        agent.stoppingDistance = info.atkDistance;
        agent.speed = agent.acceleration = info.moveSpeed;
        agent.angularSpeed = info.roundSpeed;
        HP = info.HP;
    }

    // Update is called once per frame
    void Update()
    {
        //检测怪物攻坤
        if(isDead)return ;
        //根据怪物的移动速度播放动画
        animator.SetBool("Run",agent.velocity != Vector3.zero);

        //怪物到达一定的距离后攻击
        if(Vector3.Distance(transform.position,MainTowerObject.Instance.transform.position) <= monsterInfo.atkDistance 
            && Time.time - lastAtk >= monsterInfo.atkOffSet){
            
            //记录上次攻击的时间
            lastAtk = Time.time;
            animator.SetTrigger("Atk");
        }

    }


    public void Wound(int dmg){
        if(isDead)return ;
        //减少血量
        HP = Mathf.Clamp(HP - dmg,0,monsterInfo.HP);
        if(HP == 0){
            Dead();
            DataManager.Instance.PlaySound("Music/Dead");
        }else{
            DataManager.Instance.PlaySound("Music/Wound");
            animator.SetTrigger("Wound");
        }
    }
    public void Dead(){
        isDead = true;
        //停止移动
        //agent.isStopped = true;
        agent.enabled = false;
        //播放死亡动画
        animator.SetBool("Dead",true); 
        //加钱
        GameLeveLMgr.Instance.player.changeMoney(monsterInfo.money);
    }

    public void OnWoundEnter(){
        agent.speed = 0;  
    }

    public void OnWoundExit(){
        agent.speed = monsterInfo.moveSpeed;
    }
    //死亡动画完毕后，移除尸体
    public void OnDeadExit(){
        //死亡后减少怪物数量
        GameLeveLMgr.Instance.RemoveMonster(this);
        //去除碰撞检测
        GetComponent<CapsuleCollider>().enabled = false;
        //一秒后尸体消失
        Destroy(this.gameObject,1);

        //每次怪物死亡时检测
        if(GameLeveLMgr.Instance.CheckOver()){
            GameLeveLMgr.Instance.Gameover(true);
        }
    }   

    public void OnBornExit(){  
        if(agent == null)return ;
        //给予怪物目标
        agent.SetDestination(MainTowerObject.Instance.transform.position);
        //让怪物移动
        animator.SetBool("Run",true);
    }

    public void AtkEvent(){
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward + transform.up,1, 1 << LayerMask.NameToLayer("MainTower"));
        DataManager.Instance.PlaySound("Music/Eat");
        foreach(Collider hit in hits){
            if(MainTowerObject.Instance.gameObject == hit.gameObject){
                //怪物攻击中心塔
                MainTowerObject.Instance.ChangeHP(monsterInfo.atk);
            }
        }
    }
}
