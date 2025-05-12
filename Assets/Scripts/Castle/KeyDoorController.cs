using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float interactionRange = 3f;
    private bool isOpen = false;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        // ����E��ʱ���Կ���
        if (Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            TryOpenDoor();
        }
    }

    void TryOpenDoor()
    {
        // ����Ļ���ķ���һ������
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                if (PlayerInventory.hasKey)
                {
                    OpenDoor();
                }
                else
                {
                    Debug.Log("��û��Կ�ף��޷����ţ�");
                }
            }
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        // ��ת��90�ȣ���Ҳ�����ö�����
        transform.Rotate(Vector3.up, 90f);
        Debug.Log("���Ѵ򿪣�");
        // ��ѡ�����ź���Կ����ʧ
        Camera cam = Camera.main;
        if (cam != null)
        {
            Transform holdPoint = cam.transform.Find("KeyHoldPoint");
            if (holdPoint != null && holdPoint.childCount > 0)
            {
                holdPoint.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}