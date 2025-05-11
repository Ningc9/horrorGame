using UnityEngine;

public class WoodChop : MonoBehaviour
{
    public int maxHits = 4;                     // �ɿ�����
    public GameObject firewoodPrefab;          // ���Ԥ����
    public Transform dropPosition;             // ������λ��

    private int currentHits = 0;
    private bool isDestroyed = false;
    private bool hasDroppedWood = false;       // �Ƿ��ѵ�������

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
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
                    TryChop();
                }
            }
        }
    }

    void TryChop()
    {
        if (isDestroyed) return;

        // ����Ƿ����Ÿ�ͷ
        GameObject currentItem = FindObjectOfType<ItemPickup>().currentItem;
        if (currentItem == null || !currentItem.name.Contains("Axe"))
        {
            Debug.Log("��û�����Ÿ�ͷ��");
            return;
        }

        currentHits++;
        Debug.Log("�㿳��ľͷ����ǰ���ˣ�" + currentHits + " ��");

        // ��һ�ο�ʱ����һ��ľͷ
        if (!hasDroppedWood && firewoodPrefab != null && dropPosition != null)
        {
            Instantiate(firewoodPrefab, dropPosition.position, Quaternion.identity);
            hasDroppedWood = true;
        }

        if (currentHits >= 1)
        {
            DestroyWood();
        }
    }

    void DestroyWood()
    {
        isDestroyed = true;
        Debug.Log("ľͷ����ȫ���飡");
        gameObject.SetActive(false);
    }
}
