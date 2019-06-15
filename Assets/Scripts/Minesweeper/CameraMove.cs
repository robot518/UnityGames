using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    float angle, newdis = 0, olddis = 0;
    float rotateSpeed = -20;
    bool bDown = false;
    public Transform target;

    void Start()
    {
    }
    void Update()
    {
        //缩放
        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(0).phase==TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved){
                var s1 = Input.GetTouch(0).position;         //第一个手指屏幕坐标
                var s2 = Input.GetTouch(1).position;         //第二个手指屏幕坐标
                newdis = Vector2.Distance(s1, s2);
                if (newdis > olddis)             //手势外拉
                {
                    GetComponent<Camera>().fieldOfView++;
                    if (GetComponent<Camera>().fieldOfView > 150)
                        GetComponent<Camera>().fieldOfView = 150;
                }
                if (newdis < olddis)
                {
                    GetComponent<Camera>().fieldOfView--;
                    if (GetComponent<Camera>().fieldOfView < 6)
                        GetComponent<Camera>().fieldOfView = 6;
                }
                olddis = newdis;
            }
        }

        //旋转
        if (bDown)
        {
            float _mouseX = Input.GetAxis("Mouse X");
            float _mouseY = Input.GetAxis("Mouse Y");
            //控制相机绕中心点(centerPoint)水平旋转
            transform.RotateAround(target.localPosition, Vector3.down, _mouseX * rotateSpeed);

            //记录相机绕中心点垂直旋转的总角度
            angle += _mouseY * rotateSpeed;

            //控制相机绕中心点垂直旋转(！注意此处的旋转轴时相机自身的x轴正方向！)
            transform.transform.RotateAround(target.localPosition, -transform.right, _mouseY * rotateSpeed);
        }
        if (Input.GetMouseButton(0))
        {
            bDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            bDown = false;
        }
    }
}