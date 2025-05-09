using UnityEngine;

public class StairsTeleporter : MonoBehaviour
{
    public Transform topPosition;      // 上楼的目标位置
    private CharacterController playerController;
    private bool isPlayerInTrigger = false;
    private float lastTeleportTime = 0f;  // 记录最后一次传送时间
    private const float TELEPORT_COOLDOWN = 0.5f;  // 传送冷却时间
    
    void Start()
    {
        // 查找玩家
        playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<CharacterController>();
        if (playerController == null)
        {
            Debug.LogError("[StairsTeleporter] Player not found! Make sure player has 'Player' tag and CharacterController");
            return;
        }
        
        // 检查上楼位置是否设置
        if (topPosition == null)
        {
            Debug.LogError("[StairsTeleporter] Top position not set!");
            return;
        }
        
        // 设置触发器
        BoxCollider triggerArea = GetComponent<BoxCollider>();
        if (triggerArea == null)
        {
            triggerArea = gameObject.AddComponent<BoxCollider>();
        }
        triggerArea.isTrigger = true;
        triggerArea.size = new Vector3(3f, 2f, 3f);
        triggerArea.center = new Vector3(0, 1f, 0);
        
        Debug.Log($"[StairsTeleporter] Initialized on {gameObject.name}. Target Y position: {topPosition.position.y}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            Debug.Log($"[StairsTeleporter] Player entered stairs zone at Y={other.transform.position.y}, Press ↑ to teleport up!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    void Update()
    {
        // 检查是否可以传送（包括冷却时间检查）
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.UpArrow) && 
            Time.time - lastTeleportTime >= TELEPORT_COOLDOWN)
        {
            TeleportUp();
        }
    }

    private void TeleportUp()
    {
        if (!isPlayerInTrigger || playerController == null || topPosition == null) return;

        // 如果玩家Y坐标接近顶部，不执行传送
        if (Mathf.Abs(playerController.transform.position.y - topPosition.position.y) < 1f)
        {
            Debug.Log("[StairsTeleporter] Player already at top, skipping teleport");
            return;
        }

        Vector3 startPos = playerController.transform.position;
        Vector3 targetPos = topPosition.position;
        Vector3 currentRotation = playerController.transform.eulerAngles;

        Debug.Log($"[StairsTeleporter] Starting teleport UP from Y={startPos.y} to Y={targetPos.y}");

        // 执行传送
        playerController.enabled = false;
        playerController.transform.position = targetPos;
        playerController.transform.eulerAngles = currentRotation;
        playerController.enabled = true;

        // 更新最后传送时间
        lastTeleportTime = Time.time;

        // 验证传送结果
        float actualY = playerController.transform.position.y;
        Debug.Log($"[StairsTeleporter] Successfully teleported up to Y={actualY}");
    }
} 