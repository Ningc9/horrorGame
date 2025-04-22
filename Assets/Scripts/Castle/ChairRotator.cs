using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairRotator : MonoBehaviour
{
    public float interactDistance = 3f; // ��������
    private Transform player;
    public float rotationAngle = 90f;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("δ�ҵ���Ҷ�����ȷ������� 'Player' ��ǩ��");
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
        Debug.Log("������ת�ˣ�");
    }
}
