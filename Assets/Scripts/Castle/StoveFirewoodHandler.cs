using UnityEngine;

public class StoveFirewoodHandler : MonoBehaviour
{
    public GameObject[] firewoodSlots; // 炉子内部的4个柴火
    public GameObject flameObject;     // 火焰效果对象（默认未勾选）
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // 初始状态隐藏所有火
        foreach (GameObject slot in firewoodSlots)
        {
            if (slot != null)
                slot.SetActive(false);
        }

        if (flameObject != null)
            flameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 右键点击
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    HandleRightClick();
                }
            }
        }
    }

    void HandleRightClick()
    {
        ItemPickup pickup = FindObjectOfType<ItemPickup>();
        if (pickup == null || !pickup.isHoldingItem || pickup.currentItem == null)
        {
            Debug.Log("你没有拿着任何物品");
            return;
        }

        string itemName = pickup.currentItem.name;

        if (itemName.Contains("Firewood"))
        {
            TryAddFirewood(pickup);
        }
        else if (itemName.Contains("FireBall"))
        {
            TryIgnite();
        }
    }

    /*void TryAddFirewood(ItemPickup pickup)
    {
        foreach (GameObject slot in firewoodSlots)
        {
            if (!slot.activeSelf)
            {
                slot.SetActive(true);
                Destroy(pickup.currentItem);
                pickup.currentItem = null;
                pickup.isHoldingItem = false;
                Debug.Log("添加了一块柴火！");
                return;
            }
        }

        Debug.Log("炉子已经满了！");
    }*/

    void TryAddFirewood(ItemPickup pickup)
    {
        bool hasAnyInactive = false;

        foreach (GameObject slot in firewoodSlots)
        {
            if (!slot.activeSelf)
            {
                hasAnyInactive = true;
                break;
            }
        }

        if (!hasAnyInactive)
        {
            Debug.Log("炉子已经满了！");
            return;
        }

        // 一次性点亮全部柴火
        foreach (GameObject slot in firewoodSlots)
        {
            slot.SetActive(true);
        }

        Destroy(pickup.currentItem);
        pickup.currentItem = null;
        pickup.isHoldingItem = false;

        Debug.Log("添加了一块柴火，全部点亮！");
    }


    void TryIgnite()
    {
        int activeFirewoodCount = 0;

        foreach (GameObject slot in firewoodSlots)
        {
            if (slot.activeSelf)
            {
                activeFirewoodCount++;
            }
        }

        if (activeFirewoodCount < 4)
        {
            Debug.Log("必须放满 4 块柴火才能点燃！");
            return;
        }

        if (flameObject != null && !flameObject.activeSelf)
        {
            flameObject.SetActive(true);
            Debug.Log("炉子已点燃！");
        }
    }

}
