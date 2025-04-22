using UnityEngine;

public class Platform : MonoBehaviour
{
    public string requiredItemName = "SmallStatue";       // Ҫ�����ϵ���������
    public GameObject lionOnPlatform;              // ƽ̨�ϵ�ʨ�ӣ�Ĭ�����أ�

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // �Ҽ����
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    TryPlaceLion();
                }
            }
        }
    }

    void TryPlaceLion()
    {
        // �������Ƿ������Ʒ������ָ����ʨ��
        if (ItemPickup.instance.isHoldingItem &&
            ItemPickup.instance.currentItemName == requiredItemName)
        {
            // 1. ����������ϵ�ʨ��
            Destroy(ItemPickup.instance.currentItem);

            // 2. ����ֳ�״̬
            ItemPickup.instance.currentItem = null;
            ItemPickup.instance.currentItemName = "";
            ItemPickup.instance.isHoldingItem = false;

            // 3. ����ƽ̨�ϵ�ʨ��
            lionOnPlatform.SetActive(true);

            Debug.Log("ʨ���ѷ��õ�ƽ̨�ϣ�");
        }
        else
        {
            Debug.Log("������û��ʨ�ӻ�����ȷ����Ʒ��");
        }
    }
}
