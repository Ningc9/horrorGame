using UnityEngine;
using UnityEngine.Events;

public class LionStatuePuzzle : MonoBehaviour
{
    public enum Direction { North = 0, East = 1, South = 2, West = 3 }

    [System.Serializable]
    public struct StatueSetting
    {
        [Tooltip("雕像本身（挂 MeshCollider）的 Transform")]
        public Transform statueTransform;
        [Tooltip("该雕像需要面向的方向")]
        public Direction targetDirection;
    }

    [Header("─── 雕像设置 ───")]
    [Tooltip("配置所有参与谜题的狮子雕像及它们的目标方向")]
    public StatueSetting[] statues = new StatueSetting[3];

    [Header("─── 交互设置 ───")]
    [Tooltip("玩家与雕像交互的最远距离")]
    public float interactDistance = 3f;
    [Tooltip("每次旋转的角度（90 的倍数）")]
    public float rotationStep = 90f;
    [Tooltip("发射射线使用摄像机，务必确保主摄像机有 MainCamera 标签")]
    public Camera mainCam;

    [Header("─── 物体显隐 ───")]
    [Tooltip("谜题解开后要显示的物体（场景中预先设为 Inactive）")]
    public GameObject objectToShow;
    [Tooltip("谜题解开后要隐藏的物体（场景中预先设为 Active）")]
    public GameObject objectToHide;

    [Header("─── 可选事件 ───")]
    [Tooltip("谜题解开时额外触发的回调，如播放音效、动画等")]
    public UnityEvent onPuzzleSolved;

    private bool isSolved = false;

    void Start()
    {
        if (mainCam == null)
            mainCam = Camera.main;

        if (objectToShow != null) objectToShow.SetActive(false);
        if (objectToHide != null) objectToHide.SetActive(true);
    }

    void Update()
    {
        if (isSolved) return;

        // 计算玩家与每个雕像的距离，并投射射线
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCam.ScreenPointToRay(
                new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)
            );
            Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red, 1f);

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                // 遍历所有雕像，看是否点中了它
                for (int i = 0; i < statues.Length; i++)
                {
                    var setting = statues[i];
                    // 要点中的 Collider Transform 必须正好是 statueTransform
                    if (hit.collider.transform == setting.statueTransform)
                    {
                        // 旋转该雕像
                        RotateStatue(setting.statueTransform);
                        // 检查谜题是否完成
                        CheckPuzzle();
                        break;
                    }
                }
            }
        }
    }

    private void RotateStatue(Transform statue)
    {
        statue.Rotate(0f, rotationStep, 0f, Space.Self);
        Debug.Log($"[LionStatuePuzzle] 已旋转：{statue.name} +{rotationStep}°");
    }

    private void CheckPuzzle()
    {
        // 检查每个雕像的朝向是否都与目标一致
        foreach (var s in statues)
        {
            float currentY = s.statueTransform.localEulerAngles.y % 360f;
            float targetY = ((int)s.targetDirection) * 90f;
            // Mathf.DeltaAngle 处理 0↔360 边界差值
            if (Mathf.Abs(Mathf.DeltaAngle(currentY, targetY)) > 1f)
                return; // 只要有一个不符，就退出
        }

        // 都达成条件，解谜
        SolvePuzzle();
    }

    private void SolvePuzzle()
    {
        isSolved = true;
        Debug.Log("🎉 狮子谜题解开！");

        if (objectToShow != null) objectToShow.SetActive(true);
        if (objectToHide != null) objectToHide.SetActive(false);
        onPuzzleSolved?.Invoke();
    }
}
