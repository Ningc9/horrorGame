using UnityEngine;

public class WoodChop : MonoBehaviour
{
    public int maxHits = 4;
    public GameObject firewoodPrefab;      // ���Ԥ����
    public Transform[] dropPositions;      // ����λ�ã���� 4 ����

    private int currentHits = 0;
    private bool isDestroyed = false;

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

        // ����Ƿ����Ÿ�ͷ��ͨ�������жϣ�
        GameObject currentItem = FindObjectOfType<ItemPickup>().currentItem;
        if (currentItem == null || !currentItem.name.Contains("Axe"))
        {
            Debug.Log("��û�����Ÿ�ͷ��");
            return;
        }

        // ������
        if (currentHits < dropPositions.Length && firewoodPrefab != null)
        {
            Instantiate(firewoodPrefab, dropPositions[currentHits].position, Quaternion.identity);
        }

        currentHits++;
        Debug.Log("�㿳��ľͷ����ǰ���ˣ�" + currentHits + " ��");

        if (currentHits >= maxHits)
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
