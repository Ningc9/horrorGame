using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairRotator : MonoBehaviour
{
    public float interactDistance = 3f; // ��������
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
        totalRotation += Mathf.Abs(rotationAngle); // �ۼ���ת�Ƕ�

        Debug.Log("������ת�ˣ��ܽǶȣ�" + totalRotation);

        if (!fenceHidden && totalRotation >= 180f && fenceToHide != null)
        {
            fenceToHide.SetActive(false);
            fenceHidden = true;
            Debug.Log("դ����ʧ�ˣ�");
        }
    }
}
