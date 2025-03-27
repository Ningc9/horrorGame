using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneInteract : MonoBehaviour
{
    // Start is called before the first frame update
    public float interactDistance = 3f; // ��������
    private Transform player;
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

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        // �������������ľ���
        float distance = Vector3.Distance(transform.position, player.position);

        // **ʹ�� Raycast ����Ŀ����**
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // **�����ڽ��������ڣ������������д�����**
        if (distance <= interactDistance && Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.gameObject == gameObject && Input.GetMouseButtonDown(1)) // �Ҽ����
            {
                LoadNextScene();
            }
        }
    }

    void LoadNextScene()
    {
        // ��ȡ��ǰ��������
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 5)
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            // ������һ������
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
