using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("pick up setting")]
    public KeyCode pickupKey = KeyCode.E;  // ʰȡ��Ʒ�İ���
    public float pickupRange = 2.0f;       // ʰȡ����
    public Transform handPosition;         // �ֵ�λ�ã���Ʒ��������������
    public LayerMask pickupLayer;          // ��ʰȡ��Ʒ�Ĳ�

    [Header("current situation")]
    public GameObject currentItem;         // ��ǰ���е���Ʒ
    public bool isHoldingItem = false;     // �Ƿ����ڳ�����Ʒ

    private Camera playerCamera;

    [Header("current item name")]
    public string currentItemName = "";  // ��ǰ���ŵ���Ʒ����

    public static ItemPickup instance;

    void Awake()
    {
        instance = this;
    }

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
            if (hit.collider.CompareTag("Pickup"))
            {
                PickupItem(hit.collider.gameObject);
            }
            else
            {
                Debug.Log("������岻�ܱ�����" + hit.collider.name);
            }
        }
    }

    void PickupItem(GameObject item)
    {
        // �������Ʒ������
        currentItem = item;
        isHoldingItem = true;
        currentItemName = item.name; // ���浱ǰ��Ʒ������

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
            currentItemName = "";
            isHoldingItem = false;
        }
    }
}