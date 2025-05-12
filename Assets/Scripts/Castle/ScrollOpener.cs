using UnityEngine;
using System.Collections;

public class ScrollOpener : MonoBehaviour
{
    public GameObject letterCanvas; // �Ͻ��ż�Canvas
    private bool isInspecting = false;

    private Camera myCamera; // ����

    void Start()
    {
        letterCanvas.SetActive(false);
        myCamera = Camera.main; // ����

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isInspecting)
        {
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition); // ����
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    EnterInspectMode();
                }
            }
        }

        if (isInspecting && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitInspectMode();
        }
    }

    void EnterInspectMode()
    {
        isInspecting = true;
        letterCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    void ExitInspectMode()
    {
        isInspecting = false;
        letterCanvas.SetActive(false);
        Time.timeScale = 1f;

        StartCoroutine(ForceHideCursor());
    }

    IEnumerator ForceHideCursor()
    {
        yield return null; // ��һ֡
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}