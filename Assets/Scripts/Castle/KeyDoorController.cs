using UnityEngine;

public class DoorController : MonoBehaviour
{
<<<<<<< HEAD
    public float interactionRange = 3f;
=======
    public ItemPickup playerPickup;           // 拖拽玩家的 ItemPickup 脚本
    public float interactionRange = 3f;       // 交互距离
    private Camera playerCamera;
>>>>>>> 426642d5cd8cf6ba8ebe6945160da9bb82f25bb0
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
<<<<<<< HEAD
        // ����Ļ���ķ���һ������
=======
        // 发射射线，判断玩家是否对着当前这扇门
>>>>>>> 426642d5cd8cf6ba8ebe6945160da9bb82f25bb0
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.gameObject == this.gameObject) // 是这扇门
            {
                if (PlayerInventory.hasKey)
                {
<<<<<<< HEAD
                    OpenDoor();
                }
                else
                {
                    Debug.Log("��û��Կ�ף��޷����ţ�");
=======
                    string itemName = playerPickup.currentItem.name;

                    if (itemName.Contains("GreenKey"))
                    {
                        OpenDoor();
                    }
                    else
                    {
                        Debug.Log("需要绿色钥匙才能打开这扇门！");
                    }
                }
                else
                {
                    Debug.Log("你手上什么都没有！");
>>>>>>> 426642d5cd8cf6ba8ebe6945160da9bb82f25bb0
                }
            }
        }
    }

    void OpenDoor()
    {
        isOpen = true;
<<<<<<< HEAD
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
=======
        transform.Rotate(Vector3.up, 90f);  // 简单旋转90度
        Debug.Log("门已打开！");
>>>>>>> 426642d5cd8cf6ba8ebe6945160da9bb82f25bb0
    }
}