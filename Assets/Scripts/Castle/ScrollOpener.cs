using UnityEngine;

public class ScrollOpener : MonoBehaviour
{
    public GameObject letterCanvas; // 拖进信件Canvas
    private bool isInspecting = false;

    private Camera camera;

    void Start()
    {
        letterCanvas.SetActive(false);

        camera = Camera.main;
    }

    void Update()
    {
        // 右键点卷轴时触发
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    EnterInspectMode();
                }
            }

        }

        // ESC退出查看
        if (isInspecting && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitInspectMode();
        }
    }

    void EnterInspectMode()
    {
        Debug.Log("//////////////////////");
        isInspecting = true;
        letterCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // 解锁鼠标（可选）
        Cursor.visible = true;
        Time.timeScale = 0f; // 暂停游戏（可选）
    }

    void ExitInspectMode()
    {
        isInspecting = false;
        letterCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // 重新锁定鼠标（可选）
        Cursor.visible = false;
        Time.timeScale = 1f; // 恢复游戏（可选）
    }
}
