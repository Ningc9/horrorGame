using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairRotator : MonoBehaviour
{
    public float interactDistance = 3f;
    private Transform player;
    public float rotationAngle = 90f;

    public GameObject fenceToHide;
    public ChairRotator otherChair; // 对方椅子的引用

    private bool fenceHidden = false;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("未找到玩家对象，请确保玩家有 'Player' 标签！");
        }

        if (fenceToHide == null)
        {
            Debug.LogWarning("未设置 fenceToHide！");
        }

        if (otherChair == null)
        {
            Debug.LogWarning("未设置另一个椅子的引用！");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (distance <= interactDistance && Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.gameObject == gameObject && Input.GetMouseButtonDown(1))
            {
                RotateChair();
            }
        }
    }

    void RotateChair()
    {
        transform.Rotate(0f, rotationAngle, 0f);
        Debug.Log($"{gameObject.name} 椅子旋转了！");

        CheckFacingEachOther();
    }

    void CheckFacingEachOther()
    {
        if (fenceHidden || otherChair == null || fenceToHide == null) return;

        // 获取两个椅子的 forward 向量
        Vector3 myForward = transform.forward;
        Vector3 toOther = (otherChair.transform.position - transform.position).normalized;
        float dot1 = Vector3.Dot(myForward, toOther);

        Vector3 otherForward = otherChair.transform.forward;
        Vector3 toMe = (transform.position - otherChair.transform.position).normalized;
        float dot2 = Vector3.Dot(otherForward, toMe);

        // 判断两个椅子是否几乎面对面（dot 趋近于 1）
        if (dot1 > 0.9f && dot2 > 0.9f)
        {
            fenceToHide.SetActive(false);
            fenceHidden = true;
            otherChair.fenceHidden = true; // 双方都设置为 true
            Debug.Log("两个椅子面对面，栅栏消失了！");
        }
    }
}
