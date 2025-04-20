using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoorController : MonoBehaviour
{
    public ItemPickup playerPickup;           // ��ק��ҵ� ItemPickup �ű�
    public float interactionRange = 3f;       // ��������
    private Camera playerCamera;
    private bool isOpen = false;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isOpen)
        {
            TryOpenDoor();
        }
    }

    void TryOpenDoor()
    {
        // �������ߣ��ж�����Ƿ���ŵ�ǰ������
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.gameObject == this.gameObject) // ��������
            {
                if (playerPickup.isHoldingItem && playerPickup.currentItem != null)
                {
                    string itemName = playerPickup.currentItem.name;

                    if (itemName.Contains("GreenKey"))
                    {
                        OpenDoor();
                    }
                    else
                    {
                        Debug.Log("��Ҫ��ɫԿ�ײ��ܴ������ţ�");
                    }
                }
                else
                {
                    Debug.Log("������ʲô��û�У�");
                }
            }
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        transform.Rotate(Vector3.up, 90f);  // ����ת90��
        Debug.Log("���Ѵ򿪣�");
    }
}
