using UnityEngine;

public class WoodChop : MonoBehaviour
{
    public int maxHits = 4;
    public GameObject firewoodPrefab;      // 柴火预制体
    public Transform[] dropPositions;      // 掉落位置（最多 4 个）

    private int currentHits = 0;
    private bool isDestroyed = false;

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

        // 检查是否拿着斧头（通过名字判断）
        GameObject currentItem = FindObjectOfType<ItemPickup>().currentItem;
        if (currentItem == null || !currentItem.name.Contains("Axe"))
        {
            Debug.Log("你没有拿着斧头！");
            return;
        }

        // 掉落柴火
        if (currentHits < dropPositions.Length && firewoodPrefab != null)
        {
            Instantiate(firewoodPrefab, dropPositions[currentHits].position, Quaternion.identity);
        }

        currentHits++;
        Debug.Log("你砍了木头！当前砍了：" + currentHits + " 次");

        if (currentHits >= maxHits)
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
