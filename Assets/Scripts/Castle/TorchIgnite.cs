using UnityEngine;

public class TorchIgnite : MonoBehaviour
{
    public string torchName = "torch";          // ��ѵ�����
    public float igniteRange = 2.0f;            // ��ȼ����
    public GameObject flame;                    // ¯�ӵĻ���

    private Camera playerCamera;
    private ItemPickup itemPickup;

    void Start()
    {
        playerCamera = Camera.main;
        itemPickup = FindObjectOfType<ItemPickup>();

        if (flame != null)
            flame.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // �Ҽ�
        {
            TryIgniteHeldTorch();
        }
    }

    void TryIgniteHeldTorch()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, igniteRange))
        {
            // �Ƿ������¯�ӣ��Լ���
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log("�����¯��");

                if (flame != null && flame.activeSelf)
                {
                    Debug.Log(flame.activeSelf);

                    if (itemPickup != null && itemPickup.currentItem != null)
                    {
                        GameObject heldItem = itemPickup.currentItem;

                        // ʹ�������ж���Ʒ�Ƿ��ǻ��
                        if (heldItem.name.Contains(torchName))
                        {
                            Torch torch = heldItem.GetComponent<Torch>();
                            if (torch != null && !torch.isLit)
                            {
                                torch.Ignite(); // ��ȼ���
                                Debug.Log("��ѵ�ȼ�ˣ�");
                            }
                        }
                    }

                }
            }
        }
    }
}
