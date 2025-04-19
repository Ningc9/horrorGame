using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ŵĿ��ƽű�
public class DoorController : MonoBehaviour
{
    public ItemPickup playerPickup; // ��ק��ҵ� ItemPickup �ű�

    private bool isOpen = false;

    void Start()
    {
        
    }

    void Update()
    {
        // ���� E ��ʱ���Կ���
        if (Input.GetMouseButtonDown(1) && !isOpen)
        {
            TryOpenDoor();
        }
    }

    void TryOpenDoor()
    {
        if (playerPickup.isHoldingItem && playerPickup.currentItem != null)
        {
            string itemName = playerPickup.currentItem.name;

            if (itemName.Contains("CrowBar")) // ������˹�
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("��Ҫ�˹����ܴ������ţ�");
            }
        }
        else
        {
            Debug.Log("������ʲô��û�У�");
        }
    }

    void OpenDoor()
    {
        isOpen = true;

        // ��ת�� 90 �ȣ���Ҳ������Э������ת��
        transform.Rotate(Vector3.up, 90f);

        Debug.Log("���Ѵ򿪣�");
    }
}
