using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoorController : MonoBehaviour
{
    public ItemPickup playerPickup;           // 拖拽玩家的 ItemPickup 脚本
    public float interactionRange = 3f;       // 交互距离
    private Camera playerCamera;
    private bool isOpen = false;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isOpen)
        {
            TryOpenDoor();
        }
    }

    void TryOpenDoor()
    {
        // 发射射线，判断玩家是否对着当前这扇门
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.gameObject == this.gameObject) // 是这扇门
            {
                if (playerPickup.isHoldingItem && playerPickup.currentItem != null)
                {
                    string itemName = playerPickup.currentItem.name;

                    if (itemName.Contains("GreenKey"))
                    {
                        OpenDoor();
                    }
                    else
                    {
                        Debug.Log("需要绿色钥匙才能打开这扇门！");
                    }
                }
                else
                {
                    Debug.Log("你手上什么都没有！");
                }
            }
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        transform.Rotate(Vector3.up, 90f);  // 简单旋转90度
        Debug.Log("门已打开！");
    }
}