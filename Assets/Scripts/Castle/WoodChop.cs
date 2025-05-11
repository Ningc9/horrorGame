using UnityEngine;

public class WoodChop : MonoBehaviour
{
    public int maxHits = 4;                     // 可砍次数
    public GameObject firewoodPrefab;          // 柴火预制体
    public Transform dropPosition;             // 柴火掉落位置

    private int currentHits = 0;
    private bool isDestroyed = false;
    private bool hasDroppedWood = false;       // 是否已掉落过柴火

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
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
                    TryChop();
                }
            }
        }
    }

    void TryChop()
    {
        if (isDestroyed) return;

        // 检查是否拿着斧头
        GameObject currentItem = FindObjectOfType<ItemPickup>().currentItem;
        if (currentItem == null || !currentItem.name.Contains("Axe"))
        {
            Debug.Log("你没有拿着斧头！");
            return;
        }

        currentHits++;
        Debug.Log("你砍了木头！当前砍了：" + currentHits + " 次");

        // 第一次砍时掉落一块木头
        if (!hasDroppedWood && firewoodPrefab != null && dropPosition != null)
        {
            Instantiate(firewoodPrefab, dropPosition.position, Quaternion.identity);
            hasDroppedWood = true;
        }

        if (currentHits >= 1)
        {
            DestroyWood();
        }
    }

    void DestroyWood()
    {
        isDestroyed = true;
        Debug.Log("木头被完全砍碎！");
        gameObject.SetActive(false);
    }
}
