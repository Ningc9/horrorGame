using UnityEngine;
using TMPro;

public class SimplePromptUI : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public static SimplePromptUI Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        // 初始时隐藏
        gameObject.SetActive(false);
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