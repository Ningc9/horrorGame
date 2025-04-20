using UnityEngine;

public class TorchIgnite : MonoBehaviour
{
    public string torchName = "torch";          // 火把的名称
    public float igniteRange = 2.0f;            // 点燃距离
    public GameObject flame;                    // 炉子的火焰

    private Camera playerCamera;
    private ItemPickup itemPickup;

    void Start()
    {
        playerCamera = Camera.main;
        itemPickup = FindObjectOfType<ItemPickup>();

        if (flame != null)
            flame.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 右键
        {
            TryIgniteHeldTorch();
        }
    }

    void TryIgniteHeldTorch()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, igniteRange))
        {
            // 是否点中了炉子（自己）
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log("点击了炉子");

                if (flame != null && flame.activeSelf)
                {
                    Debug.Log(flame.activeSelf);

                    if (itemPickup != null && itemPickup.currentItem != null)
                    {
                        GameObject heldItem = itemPickup.currentItem;

                        // 使用名字判断物品是否是火把
                        if (heldItem.name.Contains(torchName))
                        {
                            Torch torch = heldItem.GetComponent<Torch>();
                            if (torch != null && !torch.isLit)
                            {
                                torch.Ignite(); // 点燃火把
                                Debug.Log("火把点燃了！");
                            }
                        }
                    }

                }
            }
        }
    }
}
