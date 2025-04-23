using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairRotator : MonoBehaviour
{
    public float interactDistance = 3f; // 交互距离
    private Transform player;
    public float rotationAngle = 90f;

    public GameObject fenceToHide;
    private float totalRotation = 0f;
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
        totalRotation += Mathf.Abs(rotationAngle); // 累加旋转角度

        Debug.Log("椅子旋转了，总角度：" + totalRotation);

        if (!fenceHidden && totalRotation >= 180f && fenceToHide != null)
        {
            fenceToHide.SetActive(false);
            fenceHidden = true;
            Debug.Log("栅栏消失了！");
        }
    }
}
