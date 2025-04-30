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
                    // ��ȡ��ҵ�ǰ�õ���Ʒ
                    ItemPickup pickup = FindObjectOfType<ItemPickup>();
                    if (pickup != null && pickup.isHoldingItem && pickup.currentItem != null)
                    {
                        // �ж��Ƿ����˹������������Crowbar��
                        if (pickup.currentItem.name.Contains("CrowBar"))
                        {
                            isHiden = false;
                            wood.SetActive(isHiden);
                            Debug.Log("ľ���ѱ��˿���");
                        }
                        else
                        {
                            Debug.Log("����Ҫ�˹������˿�ľ�壡");
                        }
                    }
                    else
                    {
                        Debug.Log("��û�������κι��ߣ�");
                    }
                }
            }
        }
    }
}
