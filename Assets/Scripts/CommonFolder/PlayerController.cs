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
        // 获取组件
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        // 锁定并隐藏鼠标
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
        // 检测是否在地面上
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        // 如果在地面上且下落速度为负，重置垂直速度
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 设置一个小的负值，确保角色紧贴地面
        }
    }
    void HandleMovement()
    {
        // 获取输入
        float horizontal = Input.GetAxisRaw("Horizontal"); // A D
        float vertical = Input.GetAxisRaw("Vertical");     // W S
        // 获取摄像机的前方和右方向（忽略Y轴）
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;
        // 将Y轴分量设为0，保持水平移动
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        // 计算移动方向
        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;
        // 应用移动
        if (moveDirection.magnitude >= 0.1f)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    void HandleMouseLook()
    {
        // 获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        // 垂直旋转（上下看）
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, maxLookDownAngle, maxLookUpAngle);
        //playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.localRotation = Quaternion.Euler(rotationX, transform.localRotation.eulerAngles.y, 0);
        // 水平旋转（左右看）
        transform.Rotate(Vector3.up * mouseX);
    }
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }
    // 解锁鼠标的方法（例如打开菜单时）
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // 锁定鼠标的方法
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}