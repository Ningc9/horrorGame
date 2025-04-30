using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWoodRemove : MonoBehaviour
{
    public GameObject wood;
    private bool isHiden = true;

    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    // 获取玩家当前拿的物品
                    ItemPickup pickup = FindObjectOfType<ItemPickup>();
                    if (pickup != null && pickup.isHoldingItem && pickup.currentItem != null)
                    {
                        // 判断是否是撬棍（名字里包含Crowbar）
                        if (pickup.currentItem.name.Contains("CrowBar"))
                        {
                            isHiden = false;
                            wood.SetActive(isHiden);
                            Debug.Log("木板已被撬开！");
                        }
                        else
                        {
                            Debug.Log("你需要撬棍才能撬开木板！");
                        }
                    }
                    else
                    {
                        Debug.Log("你没有拿着任何工具！");
                    }
                }
            }
        }
    }
}
