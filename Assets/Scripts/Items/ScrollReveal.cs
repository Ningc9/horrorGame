using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScrollReveal : MonoBehaviour
{
    [Header("Requirements")]
    public string requiredVaseId = "vase.002"; // ID of the required vase
    public TextMeshProUGUI scrollText; // Reference to the UI text component
    public Renderer scrollRenderer; // Reference to the scroll's renderer
    
    [Header("Settings")]
    public float textRevealSpeed = 0.05f; // Speed at which text appears
    public float interactionRange = 2f; // Range for right-click interaction

    [TextArea(5, 10)]
    public string fullText = @"Where velvet dreams begin at rest.
Then inked thoughts rise from the desk.
Beyond the stones, I fade throughout.
Atop the spire, beneath the stars I wait.

Only by holding my scepter and following in my footsteps can escape from the castle";

    private bool isRevealed = false;
    private Material scrollMaterial;
    private Color originalColor;
    private Camera mainCamera;
    private ItemPickup itemPickup;

    void Awake()
    {
        // Initialize components
        mainCamera = Camera.main;
        itemPickup = FindObjectOfType<ItemPickup>();
    }

    void Start()
    {
        // Initialize text hidden
        if (scrollText != null)
        {
            scrollText.text = "";
            scrollText.gameObject.SetActive(false);
        }

        // Save original material color
        if (scrollRenderer != null)
        {
            scrollMaterial = scrollRenderer.material;
            originalColor = scrollMaterial.color;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right click
        {
            CheckVaseInteraction();
        }
    }

    void CheckVaseInteraction()
    {
        if (isRevealed) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            if (hit.collider.gameObject == gameObject) // Hit the scroll
            {
                // Check if player is holding the correct vase
                if (itemPickup != null && itemPickup.isHoldingItem && 
                    itemPickup.currentItem != null && 
                    itemPickup.currentItem.name.Contains(requiredVaseId))
                {
                    RevealText();
                }
                else
                {
                    Debug.Log("You need to hold the special vase to reveal the text.");
                }
            }
        }
    }

    void RevealText()
    {
        if (isRevealed) return;
        isRevealed = true;

        // Activate text component and start reveal animation
        if (scrollText != null)
        {
            scrollText.gameObject.SetActive(true);
            StartCoroutine(RevealTextGradually());
        }
    }

    IEnumerator RevealTextGradually()
    {
        string currentText = "";
        scrollText.text = "";

        foreach (char letter in fullText)
        {
            currentText += letter;
            scrollText.text = currentText;
            yield return new WaitForSeconds(textRevealSpeed);
        }

        // Change scroll color to simulate wetness
        if (scrollMaterial != null)
        {
            Color wetColor = originalColor * 0.7f;
            scrollMaterial.color = wetColor;
        }
    }

    // 检查是否已经显示
    public bool IsRevealed()
    {
        return isRevealed;
    }

    // 重置卷轴状态（用于测试）
    public void ResetScroll()
    {
        isRevealed = false;
        if (scrollText != null)
        {
            scrollText.text = "";
            scrollText.gameObject.SetActive(false);
        }
        if (scrollMaterial != null)
        {
            scrollMaterial.color = originalColor;
        }
    }
} 