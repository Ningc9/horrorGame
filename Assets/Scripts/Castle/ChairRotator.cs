using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairRotator : MonoBehaviour
{
    public float interactDistance = 3f;
    private Transform player;
    public float rotationAngle = 90f;

    public GameObject fenceToHide;
    public ChairRotator otherChair; // �Է����ӵ�����

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

        if (fenceToHide == null)
        {
            Debug.LogWarning("δ���� fenceToHide��");
        }

        if (otherChair == null)
        {
            Debug.LogWarning("δ������һ�����ӵ����ã�");
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
        Debug.Log($"{gameObject.name} ������ת�ˣ�");

        CheckFacingEachOther();
    }

    void CheckFacingEachOther()
    {
        if (fenceHidden || otherChair == null || fenceToHide == null) return;

        // ��ȡ�������ӵ� forward ����
        Vector3 myForward = transform.forward;
        Vector3 toOther = (otherChair.transform.position - transform.position).normalized;
        float dot1 = Vector3.Dot(myForward, toOther);

        Vector3 otherForward = otherChair.transform.forward;
        Vector3 toMe = (transform.position - otherChair.transform.position).normalized;
        float dot2 = Vector3.Dot(otherForward, toMe);

        // �ж����������Ƿ񼸺�����棨dot ������ 1��
        if (dot1 > 0.9f && dot2 > 0.9f)
        {
            fenceToHide.SetActive(false);
            fenceHidden = true;
            otherChair.fenceHidden = true; // ˫��������Ϊ true
            Debug.Log("������������棬դ����ʧ�ˣ�");
        }
    }
}
