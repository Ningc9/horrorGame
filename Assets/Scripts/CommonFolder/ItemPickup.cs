using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("ʰȡ����")]
    public KeyCode pickupKey = KeyCode.E;  // ʰȡ��Ʒ�İ���
    public float pickupRange = 2.0f;       // ʰȡ����
    public Transform handPosition;         // �ֵ�λ�ã���Ʒ��������������
    public LayerMask pickupLayer;          // ��ʰȡ��Ʒ�Ĳ�

    [Header("��ǰ״̬")]
    public GameObject currentItem;         // ��ǰ���е���Ʒ
    public bool isHoldingItem = false;     // �Ƿ����ڳ�����Ʒ

    private Camera playerCamera;

    void Start()
    {
        // ��ȡ��������
        playerCamera = Camera.main;

        // ȷ���ֵ�λ���Ѿ�����
        if (handPosition == null)
        {
            Debug.LogError("δ�����ֵ�λ�ã�����Inspector��ָ��handPosition");
        }
    }

    void Update()
    {
        // ������ʰȡ��ʱ
        if (Input.GetKeyDown(pickupKey))
        {
            if (isHoldingItem)
            {
                // ����Ѿ�������Ʒ�������
                DropItem();
            }
            else
            {
                // ����ʰȡ��Ʒ
                TryPickupItem();
            }
        }
    }

    void TryPickupItem()
    {
        RaycastHit hit;

        // ����������ķ�������
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // ����Ƿ��п�ʰȡ����Ʒ
        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
        {
            // �ҵ��˿�ʰȡ����Ʒ
            PickupItem(hit.collider.gameObject);
        }
    }

    void PickupItem(GameObject item)
    {
        // �������Ʒ������
        currentItem = item;
        isHoldingItem = true;

        // ��ȡ��Ʒ��Rigidbody������У�
        Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();
        if (itemRigidbody != null)
        {
            // ��������Ч��
            itemRigidbody.isKinematic = true;
        }

        // ������Ʒ����ײ������ѡ��
        Collider itemCollider = item.GetComponent<Collider>();
        if (itemCollider != null)
        {
            itemCollider.enabled = false;
        }

        // ����Ʒ����Ϊ�ֵ��Ӷ���
        item.transform.SetParent(handPosition);

        // ������Ʒ�ı���λ�ú���ת
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Debug.Log("��ʰȡ��Ʒ: " + item.name);
    }

    void DropItem()
    {
        if (currentItem != null)
        {
            // �Ƴ��������ϵ
            currentItem.transform.SetParent(null);

            // ������������Ч��
            Rigidbody itemRigidbody = currentItem.GetComponent<Rigidbody>();
            if (itemRigidbody != null)
            {
                itemRigidbody.isKinematic = false;

                // ����Ʒһ����΢����ǰ�׳�������ѡ��
                itemRigidbody.AddForce(playerCamera.transform.forward * 2.0f, ForceMode.Impulse);
            }

            // ����������ײ��
            Collider itemCollider = currentItem.GetComponent<Collider>();
            if (itemCollider != null)
            {
                itemCollider.enabled = true;
            }

            Debug.Log("�ѷ�����Ʒ: " + currentItem.name);

            // �������
            currentItem = null;
            isHoldingItem = false;
        }
    }
}