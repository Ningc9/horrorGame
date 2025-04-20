using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public Transform lid;               // 盖子物体
    public float openAngle = -90f;      // 打开角度
    public float openSpeed = 2f;        // 打开/关闭速度
    private bool isOpen = false;        // 是否已经打开
    private Quaternion closedRotation;  // 盖子关闭时的角度
    private Quaternion openRotation;    // 盖子打开时的角度

    void Start()
    {
        // 记录初始角度为“关闭”
        closedRotation = lid.localRotation;

        // 计算打开角度（绕 X 轴旋转）
        openRotation = closedRotation * Quaternion.Euler(openAngle, 0, 0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isOpen = !isOpen; // 切换开关状态
        }

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
