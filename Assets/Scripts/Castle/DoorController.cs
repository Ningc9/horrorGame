using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ŵĿ��ƽű�
public class DoorController : MonoBehaviour
{
    public ItemPickup playerPickup; // ��ק��ҵ� ItemPickup �ű�
    public float interactionRange = 3f;
    private bool isOpen = false;
    private Camera playerCamera;


    void Start()
    {
        playerCamera = Camera.main;
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
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.gameObject == this.gameObject) // ��������
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
