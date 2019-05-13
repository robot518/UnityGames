using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    float angle;
    float rotateSpeed = 20;
    public Transform target;

    void Start()
    {
    }
    private void Update()
    {
        //缩放
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            GetComponent<Camera>().fieldOfView--;
            if (GetComponent<Camera>().fieldOfView < 6)
                GetComponent<Camera>().fieldOfView = 6;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            GetComponent<Camera>().fieldOfView++;
            if (GetComponent<Camera>().fieldOfView > 150)
                GetComponent<Camera>().fieldOfView = 150;
        }

        //旋转
        if (Input.GetMouseButton(0))
        {
            float _mouseX = Input.GetAxis("Mouse X");
            float _mouseY = Input.GetAxis("Mouse Y");
            //控制相机绕中心点(centerPoint)水平旋转
            transform.RotateAround(target.localPosition, Vector3.up, _mouseX * rotateSpeed);

            //记录相机绕中心点垂直旋转的总角度
            angle += _mouseY * rotateSpeed;

            //控制相机绕中心点垂直旋转(！注意此处的旋转轴时相机自身的x轴正方向！)
            transform.transform.RotateAround(target.localPosition, transform.right, _mouseY * rotateSpeed);
        }
    }
}