using UnityEngine;

public class ChestOpener : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactionRange = 5f;
    
    private bool isOpen = false;
    private Camera mainCamera;
    private ItemPickup itemPickup;
    private BoxController boxController;

    private void Start()
    {
        mainCamera = Camera.main;
        itemPickup = FindObjectOfType<ItemPickup>();
        boxController = GetComponent<BoxController>();

        if (boxController == null)
        {
            Debug.LogError("ChestOpener: No BoxController component found!");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isOpen) // 右键点击且箱子未打开
        {
            TryOpenChest();
        }
    }

    private void TryOpenChest()
    {
        if (mainCamera == null || itemPickup == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            if (hit.collider.gameObject == gameObject)
            {
                // 检查是否拿着斧子
                if (itemPickup.isHoldingItem && 
                    itemPickup.currentItem != null && 
                    itemPickup.currentItem.name.Contains("Axe"))
                {
                    OpenChest();
                }
                else
                {
                    Debug.Log("You need an axe to open this chest!");
                }
            }
        }
    }

    private void OpenChest()
    {
        isOpen = true;
        boxController.IsOpen = true;
        Debug.Log("Chest opened!");
    }
} 