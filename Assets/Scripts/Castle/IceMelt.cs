using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMelt : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject crowbarPrefab;   // 撬棍预制体
    public float interactionRange = 2f;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 右键点击
        {
            TryMelt();
        }
    }

    void TryMelt()
    {
        // 从摄像机发射一条射线看是否打到了自己
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                // 检查玩家手中是否是点燃的火把
                GameObject currentItem = ItemPickup.instance?.currentItem;
                if (currentItem != null)
                {
                    Torch torch = currentItem.GetComponent<Torch>();
                    if (torch != null && torch.isLit)
                    {
                        Melt();
                    }
                }
            }
        }
    }

    void Melt()
    {
        // 在原地生成撬棍
        if (crowbarPrefab != null)
        {
            Instantiate(crowbarPrefab, transform.position, Quaternion.identity);
        }

        // 摧毁冰块
        Destroy(gameObject);
    }
}
