using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("movement setting")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Camera playerCamera;
    [Header("view setting")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookUpAngle = 90f;
    [SerializeField] private float maxLookDownAngle = -90f;
    [Header("jump setting")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 0.4f;

    private float rotationX = 0f;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    void Start()
    {
        // ��ȡ���
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        // �������������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleMouseLook();
        HandleJump();
    }

    void CheckGrounded()
    {
        // ����Ƿ��ڵ�����
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        // ����ڵ������������ٶ�Ϊ�������ô�ֱ�ٶ�
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // ����һ��С�ĸ�ֵ��ȷ����ɫ��������
        }
    }
    void HandleMovement()
    {
        // ��ȡ����
        float horizontal = Input.GetAxisRaw("Horizontal"); // A D
        float vertical = Input.GetAxisRaw("Vertical");     // W S
        // ��ȡ�������ǰ�����ҷ��򣨺���Y�ᣩ
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;
        // ��Y�������Ϊ0������ˮƽ�ƶ�
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        // �����ƶ�����
        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;
        // Ӧ���ƶ�
        if (moveDirection.magnitude >= 0.1f)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    void HandleMouseLook()
    {
        // ��ȡ�������
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        // ��ֱ��ת�����¿���
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, maxLookDownAngle, maxLookUpAngle);
        //playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.localRotation = Quaternion.Euler(rotationX, transform.localRotation.eulerAngles.y, 0);
        // ˮƽ��ת�����ҿ���
        transform.Rotate(Vector3.up * mouseX);
    }
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }
    // �������ķ���������򿪲˵�ʱ��
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // �������ķ���
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}