using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public Transform lid;               // 箱子盖子
    public float openAngle = -90f;      // 打开角度
    public float openSpeed = 2f;        // 开/关速度
    private bool isOpen = false;        // 是否已经开启
    private Quaternion closedRotation;  // 箱子关闭时的角度
    private Quaternion openRotation;    // 箱子打开时的角度

    // 添加公共属性来访问 isOpen
    public bool IsOpen
    {
        get { return isOpen; }
        set { isOpen = value; }
    }

    void Start()
    {
        // 记录初始角度为"关闭"
        closedRotation = lid.localRotation;

        // 计算打开角度，在 X 轴上旋转
        openRotation = closedRotation * Quaternion.Euler(openAngle, 0, 0);
    }

    void Update()
    {
        // 平滑旋转到目标角度
        if (isOpen)
        {
            lid.localRotation = Quaternion.Slerp(lid.localRotation, openRotation, Time.deltaTime * openSpeed);
        }
        else
        {
            lid.localRotation = Quaternion.Slerp(lid.localRotation, closedRotation, Time.deltaTime * openSpeed);
        }
    }
}
