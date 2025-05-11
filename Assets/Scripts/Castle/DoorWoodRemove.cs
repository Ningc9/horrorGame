using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWoodRemove : MonoBehaviour
{
    public GameObject wood;
    private bool isHiden = true;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    // Check player's current item
                    ItemPickup pickup = FindObjectOfType<ItemPickup>();
                    if (pickup != null && pickup.isHoldingItem && pickup.currentItem != null)
                    {
                        // Check if holding crowbar
                        if (pickup.currentItem.name.Contains("CrowBar"))
                        {
                            isHiden = false;
                            wood.SetActive(isHiden);
                            Debug.Log("Wood has been removed!");
                        }
                        else
                        {
                            Debug.Log("You need a crowbar to remove the wood!");
                        }
                    }
                    else
                    {
                        Debug.Log("You are not holding any tools!");
                    }
                }
            }
        }
    }
}
