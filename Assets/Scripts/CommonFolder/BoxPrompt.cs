using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 引入 UI 库
using System.Collections;
using TMPro;

public class BoxPrompt : MonoBehaviour
{
    public Transform player;
    public GameObject promptUI;         
    public Text pickupText;             
    public int triggerDistance = 3;
    public GameObject keyItem;         
    private bool hasPrompted = false;
    private bool isOpened = false;

    public Text completeText; 
    public string nextSceneName = "Level1"; 

    public TextMeshProUGUI promptText;

    void Start()
    {
        // 初始时隐藏提示
        if (promptUI != null)
            promptUI.SetActive(false);

        if (pickupText != null)
            pickupText.gameObject.SetActive(false); // 开始时隐藏捡起物品提示

        if(completeText !=null) 
            completeText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player == null || promptUI == null || keyItem == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 1. 检测是否进入箱子范围
        if (distance <= triggerDistance)
        {
            if (!hasPrompted)
            {
                promptUI.SetActive(true); // 显示提示
                hasPrompted = true;
            }

            // 2. 检测右键打开箱子
            if (Input.GetMouseButtonDown(1) && !isOpened)
            {
                Debug.Log("箱子被打开！");
                promptUI.SetActive(false); // 关闭打开箱子的提示
                pickupText.gameObject.SetActive(true); // 显示捡起物品的提示
                isOpened = true; // 标记箱子已打开
            }

            // 3. 检测按 E 键拾取物品
            if (Input.GetKeyDown(KeyCode.E) && isOpened)
            {
                PickupItem();
            }
        }
        else
        {
            // 4. 离开范围时，隐藏提示
            if (hasPrompted)
            {
                promptUI.SetActive(false);
                hasPrompted = false;
                pickupText.gameObject.SetActive(false); 
            }
        }
    }

    // 执行拾取物品的操作
    void PickupItem()
    {
        Debug.Log("已拾取物品: " + keyItem.name);
        pickupText.gameObject.SetActive(false); 

        if (completeText != null)
        {
            completeText.gameObject.SetActive(true);
            completeText.text = "Congratulation,  you will enter the next level";
        }

        StartCoroutine(LoadNextSceneAfterDelay(3f));
    }

    IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextSceneName);
    }

    public void ShowPrompt(string text)
    {
        if (promptText != null)
        {
            promptText.text = text;
            gameObject.SetActive(true);
        }
    }
    
    public void HidePrompt()
    {
        gameObject.SetActive(false);
    }
}
