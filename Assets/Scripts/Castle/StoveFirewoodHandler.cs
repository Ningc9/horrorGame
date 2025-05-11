using UnityEngine;

public class StoveFirewoodHandler : MonoBehaviour
{
    public GameObject[] firewoodSlots; // ¯���ڲ���4�����
    public GameObject flameObject;     // ����Ч������Ĭ��δ��ѡ��
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // ��ʼ״̬�������л�
        foreach (GameObject slot in firewoodSlots)
        {
            if (slot != null)
                slot.SetActive(false);
        }

        if (flameObject != null)
            flameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // �Ҽ����
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    HandleRightClick();
                }
            }
        }
    }

    void HandleRightClick()
    {
        ItemPickup pickup = FindObjectOfType<ItemPickup>();
        if (pickup == null || !pickup.isHoldingItem || pickup.currentItem == null)
        {
            Debug.Log("��û�������κ���Ʒ");
            return;
        }

        string itemName = pickup.currentItem.name;

        if (itemName.Contains("Firewood"))
        {
            TryAddFirewood(pickup);
        }
        else if (itemName.Contains("FireBall"))
        {
            TryIgnite();
        }
    }

    /*void TryAddFirewood(ItemPickup pickup)
    {
        foreach (GameObject slot in firewoodSlots)
        {
            if (!slot.activeSelf)
            {
                slot.SetActive(true);
                Destroy(pickup.currentItem);
                pickup.currentItem = null;
                pickup.isHoldingItem = false;
                Debug.Log("�����һ����");
                return;
            }
        }

        Debug.Log("¯���Ѿ����ˣ�");
    }*/

    void TryAddFirewood(ItemPickup pickup)
    {
        bool hasAnyInactive = false;

        foreach (GameObject slot in firewoodSlots)
        {
            if (!slot.activeSelf)
            {
                hasAnyInactive = true;
                break;
            }
        }

        if (!hasAnyInactive)
        {
            Debug.Log("¯���Ѿ����ˣ�");
            return;
        }

        // һ���Ե���ȫ�����
        foreach (GameObject slot in firewoodSlots)
        {
            slot.SetActive(true);
        }

        Destroy(pickup.currentItem);
        pickup.currentItem = null;
        pickup.isHoldingItem = false;

        Debug.Log("�����һ����ȫ��������");
    }


    void TryIgnite()
    {
        int activeFirewoodCount = 0;

        foreach (GameObject slot in firewoodSlots)
        {
            if (slot.activeSelf)
            {
                activeFirewoodCount++;
            }
        }

        if (activeFirewoodCount < 4)
        {
            Debug.Log("������� 4 ������ܵ�ȼ��");
            return;
        }

        if (flameObject != null && !flameObject.activeSelf)
        {
            flameObject.SetActive(true);
            Debug.Log("¯���ѵ�ȼ��");
        }
    }

}
