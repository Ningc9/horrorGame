using UnityEngine;
using System.Collections;

public class ReturnTeleporter : MonoBehaviour
{
    [Header("Teleport Settings")]
    [Tooltip("The position to teleport to when going down")]
    public Transform downPosition;  // 下楼的目标位置
    
    [Header("Debug")]
    [SerializeField] private bool showDebugGizmos = true;

    private CharacterController playerController;
    private bool isPlayerInTrigger = false;
    private float lastTeleportTime = 0f;
    private const float TELEPORT_COOLDOWN = 0.5f;
    private bool canTeleport = false;

    void Start()
    {
        // 查找玩家
        playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<CharacterController>();
        if (playerController == null)
        {
            Debug.LogError($"[ReturnTeleporter] Player not found! Make sure player has 'Player' tag and CharacterController");
            return;
        }
        
        // 检查下楼位置是否设置
        if (downPosition == null)
        {
            Debug.LogError($"[ReturnTeleporter] Down position not set on {gameObject.name}! Please set it in inspector");
            return;
        }

        SetupTriggerArea();
        Debug.Log($"[ReturnTeleporter] Initialized on {gameObject.name}. Target Y position: {downPosition.position.y}");
    }

    private void SetupTriggerArea()
    {
        BoxCollider triggerArea = GetComponent<BoxCollider>();
        if (triggerArea == null)
        {
            triggerArea = gameObject.AddComponent<BoxCollider>();
        }
        
        triggerArea.isTrigger = true;
        triggerArea.size = new Vector3(4f, 3f, 4f);
        triggerArea.center = new Vector3(0, 1.5f, 0);
    }

    void Update()
    {
        if (!isPlayerInTrigger || playerController == null) return;

        // 检查玩家是否在高处
        bool isPlayerHighEnough = playerController.transform.position.y > 1.0f;
        
        // 更新可传送状态
        canTeleport = isPlayerHighEnough && isPlayerInTrigger;

        // 检查传送输入（使用下箭头键）
        if (canTeleport && Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Time.time - lastTeleportTime >= TELEPORT_COOLDOWN)
            {
                TeleportDown();
            }
            else
            {
                Debug.Log($"[ReturnTeleporter] Teleport on cooldown. Wait {TELEPORT_COOLDOWN - (Time.time - lastTeleportTime):F1} seconds");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            Debug.Log($"[ReturnTeleporter] Player entered return zone at Y={other.transform.position.y}, Press ↓ to teleport down!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            canTeleport = false;
            Debug.Log("[ReturnTeleporter] Player left return zone");
        }
    }

    private void TeleportDown()
    {
        if (!canTeleport || playerController == null || downPosition == null) return;

        Vector3 startPos = playerController.transform.position;
        GameObject player = playerController.gameObject;
        
        try
        {
            // 禁用控制器
            playerController.enabled = false;
            
            // 设置新位置
            Vector3 targetPos = new Vector3(downPosition.position.x, 0.5f, downPosition.position.z);
            Debug.Log($"[ReturnTeleporter] Teleporting from Y={startPos.y} to Y={targetPos.y}");
            
            // 执行传送
            player.transform.position = targetPos;
            
            // 重新启用控制器
            playerController.enabled = true;
            
            // 更新状态
            lastTeleportTime = Time.time;
            canTeleport = false;
            
            // 强制更新
            playerController.Move(Vector3.zero);
            
            Debug.Log($"[ReturnTeleporter] Teleport complete. New Y position: {player.transform.position.y}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ReturnTeleporter] Teleport failed: {e.Message}");
            if (playerController != null) playerController.enabled = true;
        }
    }

    void OnDisable()
    {
        if (playerController != null && !playerController.enabled)
        {
            playerController.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;
        
        // 只在编辑器中显示触发区域
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + new Vector3(0, 1.5f, 0);
        Gizmos.DrawWireCube(center, new Vector3(4f, 3f, 4f));
        
        // 显示传送目标位置
        if (downPosition != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, downPosition.position);
            Gizmos.DrawWireSphere(downPosition.position, 0.5f);
        }
    }
} 