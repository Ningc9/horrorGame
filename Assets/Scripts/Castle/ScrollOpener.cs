using UnityEngine;

public class ScrollOpener : MonoBehaviour
{
    public GameObject letterCanvas; // �Ͻ��ż�Canvas
    private bool isInspecting = false;

    private Camera camera;

    void Start()
    {
        letterCanvas.SetActive(false);

        camera = Camera.main;
    }

    void Update()
    {
        // �Ҽ������ʱ����
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

        // ESC�˳��鿴
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
        Cursor.lockState = CursorLockMode.None; // ������꣨��ѡ��
        Cursor.visible = true;
        Time.timeScale = 0f; // ��ͣ��Ϸ����ѡ��
    }

    void ExitInspectMode()
    {
        isInspecting = false;
        letterCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // ����������꣨��ѡ��
        Cursor.visible = false;
        Time.timeScale = 1f; // �ָ���Ϸ����ѡ��
    }
}
