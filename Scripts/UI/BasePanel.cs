using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float alphaSpeed = 10f;
    public bool isShow = false;
    private UnityAction hideCallBack = null;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        if(canvasGroup == null){
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        }
    }

    public virtual void ShowMe(){
        canvasGroup.alpha = 0;
        isShow = true;
    }
    // 面板隐藏后传入委托，把想要做的事传入
    public virtual void HideMe(UnityAction callBack){
        canvasGroup.alpha = 1;
        isShow = false;
        hideCallBack = callBack;
    }

    protected virtual void Start()
    {
        Init();
    }

    /*
        所有子面板都实现注册功能，写成抽象类让子类实现注册功能
    */
    public abstract void Init();

    virtual public void Update()
    {
        if(isShow && canvasGroup.alpha != 1){
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha > 1)canvasGroup.alpha = 1;
        }else if(!isShow && canvasGroup.alpha != 0){
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha < 0) {
                canvasGroup.alpha = 0;
            } 
            hideCallBack?.Invoke();//当窗口被隐藏时调用，且callBack不为空
        }
    }
}
