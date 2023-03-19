using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;
    //存储面板子类,key=>面板名字,value=>面板预设体
    Dictionary<string,BasePanel> panelDic = new Dictionary<string, BasePanel>();
    private Transform canvasTranform;
    //在构造函数中创建面板父类,保证用一个面板父类  
    private UIManager(){
        //在预设体中存放canvas 放入场景中
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTranform = canvas.transform;
        //切换场景时不删除canvas,保证只用一个canvas
        GameObject.DontDestroyOnLoad(canvas);
    }

    //显示面板:
    public T ShowPanel<T>()where T:BasePanel{
        //保证满版预设体的名字和类型名相同，在查找时可以很快的查找
        string panelName = typeof(T).Name;
        //查找是否显示过该面板,如果显示过直接存贮在字典中，直接获取即可
        if(panelDic.ContainsKey(panelName)){
            return panelDic[panelName] as T;
        }
        //如果字典中未储存，则在资源中直接获取，在字典中直接创建面板,并实例化出来
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        //将对象放到canvas上
        panelObj.transform.SetParent(canvasTranform,false);
        //获取脚本组件
        T panel = panelObj.GetComponent<T>();
        //存入字典中
        panelDic.Add(panelName,panel);
        panel.ShowMe();
        return panel;
    }
    /// </summary>
    ///隐藏面板
    /// isFade 是否淡出面板，false表示直接删除面板，true表示淡出面板后删除面板
    public void HidePanel<T>(bool isFade = false) where T:BasePanel{
        // 获取面板名字
        string panelName = typeof(T).Name;
        //判断当前面板中是否显示该面板
        if(panelDic.ContainsKey(panelName)){
            T panel = panelDic[panelName] as T;
            if(isFade){
                panel.HideMe(()=>{
                    GameObject.Destroy(panel.gameObject);
                    panelDic.Remove(panelName);
                });
            }else {
                GameObject.Destroy(panel.gameObject);
                panelDic.Remove(panelName);
            }
        }
    }

    //得到面板
    public T GetPanel<T>()where T:BasePanel{
        string panelName = typeof(T).Name;
        if(panelDic.ContainsKey(panelName)){
            return panelDic[panelName] as T;
        }
        //当前未显示该面板
        return null;
    }


}
