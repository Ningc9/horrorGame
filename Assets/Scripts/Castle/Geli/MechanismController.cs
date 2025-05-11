using UnityEngine;

public class MechanismController : MonoBehaviour
{
    [Header("—— 要隐藏的目标 ——")]
    [Tooltip("在 Inspector 里拖入：当右键点击机关时要隐藏的物体")]
    public GameObject targetObject;

    [Header("—— 交互设置 ——")]
    [Tooltip("从摄像机发射射线的最大距离")]
    public float interactDistance = 3f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (targetObject == null)
            Debug.LogWarning($"[{nameof(MechanismController)}] 未指定 targetObject");
    }

    void Update()
    {
        // 检测鼠标右键
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, interactDistance))
            {
                // 如果射线打到了挂这个脚本的机关模型
                if (hit.collider.gameObject == gameObject)
                {
                    if (targetObject != null)
                    {
                        targetObject.SetActive(false);
                        Debug.Log($"隐藏了：{targetObject.name}");
                    }
                    else
                    {
                        Debug.LogWarning("没有预先指定要隐藏的物体！");
                    }
                }
            }
        }
    }
}
