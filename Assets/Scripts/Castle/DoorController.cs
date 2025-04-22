using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 门的控制脚本
public class DoorController : MonoBehaviour
{
    public ItemPickup playerPickup; // 拖拽玩家的 ItemPickup 脚本
    public float interactionRange = 3f;
    private bool isOpen = false;
    private Camera playerCamera;


    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        // 按下 E 键时尝试开门
        if (Input.GetMouseButtonDown(1) && !isOpen)
        {
            TryOpenDoor();
        }
    }

    void TryOpenDoor()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.gameObject == this.gameObject) // 是这扇门
            {
                if (playerPickup.isHoldingItem && playerPickup.currentItem != null)
                {
                    string itemName = playerPickup.currentItem.name;

                    if (itemName.Contains("CrowBar")) // 如果是撬棍
                    {
                        OpenDoor();
                    }
                    else
                    {
                        Debug.Log("需要撬棍才能打开这扇门！");
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

        // 旋转门 90 度（你也可以用协程慢慢转）
        transform.Rotate(Vector3.up, 90f);

        Debug.Log("门已打开！");
    }
}
