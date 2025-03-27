using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneInteract : MonoBehaviour
{
    // Start is called before the first frame update
    public float interactDistance = 3f; // 交互距离
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
            Debug.LogError("未找到玩家对象，请确保玩家有 'Player' 标签！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        // 计算玩家与物体的距离
        float distance = Vector3.Distance(transform.position, player.position);

        // **使用 Raycast 进行目标检测**
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // **必须在交互距离内，并且射线命中此物体**
        if (distance <= interactDistance && Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.gameObject == gameObject && Input.GetMouseButtonDown(1)) // 右键点击
            {
                LoadNextScene();
            }
        }
    }

    void LoadNextScene()
    {
        // 获取当前场景索引
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 5)
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            // 加载下一个场景
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
