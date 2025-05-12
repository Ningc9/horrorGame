using UnityEngine;
using System.Collections;

public class StairwayInteraction : MonoBehaviour
{
    public Transform upperFloorPosition; // Upper floor target position
    public Transform lowerFloorPosition; // Lower floor target position
    public string upPromptText = "Press ↑ to go upstairs"; // Up prompt text
    public string downPromptText = "Press ↓ to go downstairs"; // Down prompt text
    private bool playerInRange = false;
    private PlayerController player;
    private BoxPrompt boxPrompt;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        boxPrompt = FindObjectOfType<BoxPrompt>();
        
        // Ensure there's a trigger collider
        if (!GetComponent<BoxCollider>())
        {
            BoxCollider triggerArea = gameObject.AddComponent<BoxCollider>();
            triggerArea.isTrigger = true;
            triggerArea.size = new Vector3(2f, 2f, 2f); // Adjust based on stair size
            triggerArea.center = new Vector3(0f, 1f, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (boxPrompt != null)
            {
                // Show both prompts if both directions are available
                string prompt = "";
                if (upperFloorPosition != null) prompt += upPromptText;
                if (lowerFloorPosition != null)
                {
                    if (prompt != "") prompt += "\n";
                    prompt += downPromptText;
                }
                boxPrompt.ShowPrompt(prompt);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (boxPrompt != null)
            {
                boxPrompt.HidePrompt();
            }
        }
    }

    void Update()
    {
        if (!playerInRange) return;

        // Handle up arrow key
        if (Input.GetKeyDown(KeyCode.UpArrow) && upperFloorPosition != null)
        {
            TeleportPlayer(upperFloorPosition);
        }
        // Handle down arrow key
        else if (Input.GetKeyDown(KeyCode.DownArrow) && lowerFloorPosition != null)
        {
            TeleportPlayer(lowerFloorPosition);
        }
    }

    void TeleportPlayer(Transform destination)
    {
        if (player != null && destination != null)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                // Temporarily disable CharacterController to allow teleportation
                controller.enabled = false;
                player.transform.position = destination.position;
                controller.enabled = true;
            }
        }
    }
} 