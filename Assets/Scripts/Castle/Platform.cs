using UnityEngine;

public class Platform : MonoBehaviour
{
    public string requiredItemName = "SmallStatue";       // 要求手上的物体名称
    public GameObject lionOnPlatform;              // 平台上的狮子（默认隐藏）

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 右键点击
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    TryPlaceLion();
                }
            }
        }
    }

    void TryPlaceLion()
    {
        // 检查玩家是否持有物品，且是指定的狮子
        if (ItemPickup.instance.isHoldingItem &&
            ItemPickup.instance.currentItemName == requiredItemName)
        {
            // 1. 销毁玩家手上的狮子
            Destroy(ItemPickup.instance.currentItem);

            // 2. 清空手持状态
            ItemPickup.instance.currentItem = null;
            ItemPickup.instance.currentItemName = "";
            ItemPickup.instance.isHoldingItem = false;

            // 3. 激活平台上的狮子
            lionOnPlatform.SetActive(true);

            Debug.Log("狮子已放置到平台上！");
        }
        else
        {
            Debug.Log("你手上没有狮子或不是正确的物品！");
        }
    }
}
