using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //控制摄像机位置
    public Transform target;
    public Vector3 offset;

    //控制摄像机朝向/移动
    public float bodyHight;
    public float moveSpeed;
    public float rotateSpeed;


    // Update is called once per frame
    void Update()
    {
        if(target == null)return ;
        //z轴偏移
        Vector3 targetTrans = target.forward * offset.z;
        //y轴偏移
        targetTrans += Vector3.up * offset.y;
        //x轴偏移
        targetTrans += transform.right * offset.x;
        //移动摄像机
        transform.position = Vector3.Lerp(transform.position, target.position + targetTrans,moveSpeed * Time.deltaTime);
        //计算计算机旋转角度
        Quaternion quaternion = Quaternion.LookRotation(target.position + Vector3.up * bodyHight - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation,quaternion,rotateSpeed * Time.deltaTime);
    }


    public void SetTarget(Transform player){
        target = player;
    }
}
