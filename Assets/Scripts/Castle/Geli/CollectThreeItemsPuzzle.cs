using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectThreeItemsPuzzle : MonoBehaviour
{
    [Header("—— 引用设置 ——")]
    [Tooltip("拖入负责捡起物品的脚本组件")]
    [SerializeField] private ItemPickup playerPickup;
    [Tooltip("拖入“木头”预制件")]
    [SerializeField] private GameObject woodPrefab;
    [Tooltip("拖入“铁锭”预制件")]
    [SerializeField] private GameObject ingotPrefab;
    [Tooltip("拖入“锤子”预制件")]
    [SerializeField] private GameObject hammerPrefab;

    [Header("—— 隐藏的奖励对象 ——")]
    [Tooltip("在场景中预先放入并设为 Inactive")]
    [SerializeField] private GameObject hiddenReward;

    [Header("—— 交互设置 ——")]
    [Tooltip("玩家可右键放入物品的最远距离")]
    [SerializeField] private float interactionRange = 3f;

    [Header("—— 解谜完成事件 ——")]
    [Tooltip("在 Inspector 中添加额外的响应（比如播放动画、音效等）")]
    [SerializeField] private UnityEvent onPuzzleSolved;

    private bool hasWood = false;
    private bool hasIngot = false;
    private bool hasHammer = false;
    private bool isSolved = false;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        // 确保隐藏奖励一开始是 Inactive
        if (hiddenReward != null)
            hiddenReward.SetActive(false);
    }

    void Update()
    {
        if (isSolved) 
            return;

        // 右键检测
        if (Input.GetMouseButtonDown(1)
            && playerPickup != null
            && playerPickup.isHoldingItem
            && playerPickup.currentItem != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, interactionRange)
                && hit.collider.gameObject == gameObject)
            {
                TryAddItem();
            }
        }
    }

    private void TryAddItem()
    {
        var current = playerPickup.currentItem;
        var itemName = current.name;

        if (!hasWood && itemName.Contains(woodPrefab.name))
        {
            hasWood = true;
            Debug.Log("✅ 木头 已收集");
        }
        else if (!hasIngot && itemName.Contains(ingotPrefab.name))
        {
            hasIngot = true;
            Debug.Log("✅ 铁锭 已收集");
        }
        else if (!hasHammer && itemName.Contains(hammerPrefab.name))
        {
            hasHammer = true;
            Debug.Log("✅ 锤子 已收集");
        }
        else
        {
            Debug.Log("❌ 此物品不符合谜题要求");
            return;
        }

        // 从玩家手中移除该物品
        Destroy(current);
        playerPickup.currentItem = null;
        playerPickup.isHoldingItem = false;

        // 如果三件都收集齐，解谜
        if (hasWood && hasIngot && hasHammer)
            SolvePuzzle();
    }

    private void SolvePuzzle()
    {
        isSolved = true;
        Debug.Log("🎉 谜题完成！");

        // 触发额外事件
        onPuzzleSolved?.Invoke();

        // 激活奖励物体
        if (hiddenReward != null)
            hiddenReward.SetActive(true);
    }
}
