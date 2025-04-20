using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMelt : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject crowbarPrefab;   // �˹�Ԥ����
    public float interactionRange = 2f;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // �Ҽ����
        {
            TryMelt();
        }
    }

    void TryMelt()
    {
        // �����������һ�����߿��Ƿ�����Լ�
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                // �����������Ƿ��ǵ�ȼ�Ļ��
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
        // ��ԭ�������˹�
        if (crowbarPrefab != null)
        {
            Instantiate(crowbarPrefab, transform.position, Quaternion.identity);
        }

        // �ݻٱ���
        Destroy(gameObject);
    }
}
