using UnityEngine;
using System.Collections.Generic;

// 此脚本用于调试和修复传送器问题
public class TeleporterDebugger : MonoBehaviour
{
    [Header("调试设置")]
    public bool enableDebug = true;
    public bool fixTeleporterIssues = true;
    public KeyCode debugKey = KeyCode.F2;
    
    [Header("传送器引用")]
    public StairsTeleporter[] upTeleporters;
    public ReturnTeleporter[] downTeleporters;
    
    private CharacterController playerController;
    private Vector3 lastPlayerPosition;
    private float lastLogTime;
    private const float LOG_INTERVAL = 2.0f; // 日志间隔时间
    
    void Start()
    {
        // 查找玩家
        playerController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<CharacterController>();
        if (playerController == null)
        {
            Debug.LogError("[TeleporterDebugger] Player not found!");
            return;
        }
        
        // 自动查找所有传送器
        if (upTeleporters == null || upTeleporters.Length == 0)
        {
            upTeleporters = FindObjectsOfType<StairsTeleporter>();
        }
        
        if (downTeleporters == null || downTeleporters.Length == 0)
        {
            downTeleporters = FindObjectsOfType<ReturnTeleporter>();
        }
        
        // 记录初始位置
        lastPlayerPosition = playerController.transform.position;
        
        // 输出初始调试信息
        Debug.Log($"[TeleporterDebugger] 找到 {upTeleporters.Length} 个上楼传送器和 {downTeleporters.Length} 个下楼传送器");
        LogTeleporterInfo();
    }
    
    void Update()
    {
        if (!enableDebug || playerController == null) return;
        
        // 按下调试键显示所有传送器信息
        if (Input.GetKeyDown(debugKey))
        {
            LogTeleporterInfo();
        }
        
        // 检测玩家位置变化
        if (Vector3.Distance(lastPlayerPosition, playerController.transform.position) > 1f)
        {
            if (Time.time - lastLogTime > LOG_INTERVAL)
            {
                Debug.Log($"[TeleporterDebugger] 玩家位置: Y={playerController.transform.position.y:F6}");
                lastLogTime = Time.time;
            }
            lastPlayerPosition = playerController.transform.position;
        }
        
        // 如果启用了修复功能，检查并修复传送器问题
        if (fixTeleporterIssues)
        {
            FixTeleporterIssues();
        }
    }
    
    // 输出所有传送器的详细信息
    private void LogTeleporterInfo()
    {
        Debug.Log("========== 传送器调试信息 ==========");
        Debug.Log($"玩家当前位置: Y={playerController.transform.position.y:F6}");
        
        // 上楼传送器信息
        Debug.Log("--- 上楼传送器 ---");
        foreach (var teleporter in upTeleporters)
        {
            if (teleporter == null) continue;
            
            string targetInfo = teleporter.topPosition != null 
                ? $"目标Y={teleporter.topPosition.position.y:F6}" 
                : "未设置目标位置!";
                
            Debug.Log($"上楼传送器: {teleporter.gameObject.name}, {targetInfo}, " +
                      $"位置Y={teleporter.transform.position.y:F6}");
                      
            // 检查碰撞器
            BoxCollider collider = teleporter.GetComponent<BoxCollider>();
            if (collider != null)
            {
                Debug.Log($"  碰撞器: isTrigger={collider.isTrigger}, " +
                          $"大小={collider.size}, 中心={collider.center}");
            }
            else
            {
                Debug.LogWarning($"  上楼传送器 {teleporter.gameObject.name} 没有碰撞器!");
            }
        }
        
        // 下楼传送器信息
        Debug.Log("--- 下楼传送器 ---");
        foreach (var teleporter in downTeleporters)
        {
            if (teleporter == null) continue;
            
            string targetInfo = teleporter.downPosition != null 
                ? $"目标Y={teleporter.downPosition.position.y:F6}" 
                : "未设置目标位置!";
                
            Debug.Log($"下楼传送器: {teleporter.gameObject.name}, {targetInfo}, " +
                      $"位置Y={teleporter.transform.position.y:F6}");
                      
            // 检查碰撞器
            BoxCollider collider = teleporter.GetComponent<BoxCollider>();
            if (collider != null)
            {
                Debug.Log($"  碰撞器: isTrigger={collider.isTrigger}, " +
                          $"大小={collider.size}, 中心={collider.center}");
            }
            else
            {
                Debug.LogWarning($"  下楼传送器 {teleporter.gameObject.name} 没有碰撞器!");
            }
        }
        
        Debug.Log("====================================");
    }
    
    // 修复常见的传送器问题
    private void FixTeleporterIssues()
    {
        // 确保所有传送器都有正确的碰撞器设置
        foreach (var teleporter in upTeleporters)
        {
            if (teleporter == null) continue;
            
            BoxCollider collider = teleporter.GetComponent<BoxCollider>();
            if (collider == null)
            {
                collider = teleporter.gameObject.AddComponent<BoxCollider>();
                Debug.Log($"[TeleporterDebugger] 为上楼传送器 {teleporter.gameObject.name} 添加了碰撞器");
            }
            
            // 确保碰撞器设置正确
            collider.isTrigger = true;
            collider.size = new Vector3(3f, 2f, 3f);
            collider.center = new Vector3(0, 1f, 0);
        }
        
        foreach (var teleporter in downTeleporters)
        {
            if (teleporter == null) continue;
            
            BoxCollider collider = teleporter.GetComponent<BoxCollider>();
            if (collider == null)
            {
                collider = teleporter.gameObject.AddComponent<BoxCollider>();
                Debug.Log($"[TeleporterDebugger] 为下楼传送器 {teleporter.gameObject.name} 添加了碰撞器");
            }
            
            // 确保碰撞器设置正确
            collider.isTrigger = true;
            collider.size = new Vector3(3f, 2f, 3f);
            collider.center = new Vector3(0, 1f, 0);
        }
    }
    
    // 在编辑器中可视化传送器
    private void OnDrawGizmos()
    {
        if (!enableDebug) return;
        
        // 画出玩家位置
        if (playerController != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(playerController.transform.position, 0.3f);
        }
        
        // 画出所有上楼传送器的连接线
        if (upTeleporters != null)
        {
            foreach (var teleporter in upTeleporters)
            {
                if (teleporter == null || teleporter.topPosition == null) continue;
                
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(teleporter.transform.position, teleporter.topPosition.position);
                Gizmos.DrawWireSphere(teleporter.transform.position, 0.5f);
                Gizmos.DrawWireSphere(teleporter.topPosition.position, 0.5f);
            }
        }
        
        // 画出所有下楼传送器的连接线
        if (downTeleporters != null)
        {
            foreach (var teleporter in downTeleporters)
            {
                if (teleporter == null || teleporter.downPosition == null) continue;
                
                Gizmos.color = Color.red;
                Gizmos.DrawLine(teleporter.transform.position, teleporter.downPosition.position);
                Gizmos.DrawWireSphere(teleporter.transform.position, 0.5f);
                Gizmos.DrawWireSphere(teleporter.downPosition.position, 0.5f);
            }
        }
    }
} 