using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Events; 

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;
    private UnityAction playOverAction;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    //左转
    public void TurnLeft(UnityAction action){
        animator.SetTrigger("Left");
        playOverAction = action;
    }
    //右转
    public void TurnRight(UnityAction action){
        animator.SetTrigger("Right");
        playOverAction = action;
    }

    //动画结束时运行
    public void PlayOver(){
        playOverAction?.Invoke();
        playOverAction = null;
    }
}
