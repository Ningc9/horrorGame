using UnityEngine;

// 此脚本用于紧急情况下传送玩家
public class EmergencyTeleport : MonoBehaviour
{
    [Header("传送设置")]
    public Transform[] teleportPoints;  // 可用的传送点
    public KeyCode emergencyKey = KeyCode.F5;  // 紧急传送按键
    
    [Header("UI提示")]
    public string helpMessage = "按F5键紧急传送到安全位置";
    public float messageDisplayTime = 5f;  // 消息显示时间
    
    private CharacterController playerController;
    private float messageTimer = 0f;
    private bool showMessage = false;
    
    void Start()
    {
        // 查找玩家
        playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<CharacterController>();
        if (playerController == null)
        {
            Debug.LogError("[EmergencyTeleport] Player not found!");
            return;
        }
        
        // 如果没有设置传送点，尝试查找场景中的传送点
        if (teleportPoints == null || teleportPoints.Length == 0)
        {
            // 尝试查找下楼传送器的目标点
            ReturnTeleporter[] downTeleporters = FindObjectsOfType<ReturnTeleporter>();
            if (downTeleporters != null && downTeleporters.Length > 0)
            {
                teleportPoints = new Transform[downTeleporters.Length];
                for (int i = 0; i < downTeleporters.Length; i++)
                {
                    if (downTeleporters[i].downPosition != null)
                    {
                        teleportPoints[i] = downTeleporters[i].downPosition;
                    }
                }
            }
        }
        
        Debug.Log($"[EmergencyTeleport] 初始化完成，找到 {teleportPoints?.Length ?? 0} 个可用传送点");
    }
    
    void Update()
    {
        if (playerController == null || teleportPoints == null || teleportPoints.Length == 0) return;
        
        // 检测紧急传送键
        if (Input.GetKeyDown(emergencyKey))
        {
            EmergencyTeleportToSafeLocation();
        }
        
        // 更新消息显示计时器
        if (showMessage)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0)
            {
                showMessage = false;
            }
        }
    }
    
    // 紧急传送到安全位置
    private void EmergencyTeleportToSafeLocation()
    {
        if (teleportPoints == null || teleportPoints.Length == 0)
        {
            Debug.LogWarning("[EmergencyTeleport] 没有可用的传送点!");
            return;
        }
        
        // 找到最近的传送点
        Transform bestTarget = teleportPoints[0];
        float closestDistance = float.MaxValue;
        
        foreach (Transform point in teleportPoints)
        {
            if (point == null) continue;
            
            float distance = Vector3.Distance(playerController.transform.position, point.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = point;
            }
        }
        
        if (bestTarget == null)
        {
            Debug.LogError("[EmergencyTeleport] 无法找到有效的传送点!");
            return;
        }
        
        // 执行传送
        Vector3 startPos = playerController.transform.position;
        Vector3 targetPos = bestTarget.position;
        
        Debug.Log($"[EmergencyTeleport] 紧急传送从 Y={startPos.y} 到 Y={targetPos.y}");
        
        // 禁用角色控制器
        playerController.enabled = false;
        
        // 设置位置
        playerController.transform.position = targetPos;
        
        // 重新启用角色控制器
        playerController.enabled = true;
        
        // 显示消息
        ShowMessage($"已传送到安全位置 Y={playerController.transform.position.y}");
    }
    
    // 显示消息
    private void ShowMessage(string message)
    {
        Debug.Log($"[EmergencyTeleport] {message}");
        showMessage = true;
        messageTimer = messageDisplayTime;
    }
    
    // 在屏幕上显示消息
    void OnGUI()
    {
        if (showMessage)
        {
            GUI.color = Color.yellow;
            GUI.Label(new Rect(10, 10, 500, 20), helpMessage);
        }
    }
} 