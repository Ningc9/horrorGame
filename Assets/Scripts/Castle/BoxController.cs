using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public Transform lid;               // ��������
    public float openAngle = -90f;      // �򿪽Ƕ�
    public float openSpeed = 2f;        // ��/�ر��ٶ�
    private bool isOpen = false;        // �Ƿ��Ѿ���
    private Quaternion closedRotation;  // ���ӹر�ʱ�ĽǶ�
    private Quaternion openRotation;    // ���Ӵ�ʱ�ĽǶ�

    void Start()
    {
        // ��¼��ʼ�Ƕ�Ϊ���رա�
        closedRotation = lid.localRotation;

        // ����򿪽Ƕȣ��� X ����ת��
        openRotation = closedRotation * Quaternion.Euler(openAngle, 0, 0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isOpen = !isOpen; // �л�����״̬
        }

        // ƽ����ת��Ŀ��Ƕ�
        if (isOpen)
        {
            lid.localRotation = Quaternion.Slerp(lid.localRotation, openRotation, Time.deltaTime * openSpeed);
        }
        else
        {
            lid.localRotation = Quaternion.Slerp(lid.localRotation, closedRotation, Time.deltaTime * openSpeed);
        }
    }
}
