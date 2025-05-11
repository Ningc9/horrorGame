using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairRaycast : MonoBehaviour
{
    public float rayDistance = 5f;
    public LayerMask interactableLayer;
    public Text tooltipText; // Unity UI Text 或 TextMeshProUGUI

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        tooltipText.gameObject.SetActive(false); // 初始不显示提示
    }

    void Update()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            InteractableObject obj = hit.collider.GetComponent<InteractableObject>();
            if (obj != null)
            {
                tooltipText.text = obj.interactMessage;
                tooltipText.gameObject.SetActive(true);
            }
            else
            {
               tooltipText.gameObject.SetActive(false); 
            }
        }
        else
        {
            tooltipText.gameObject.SetActive(false);
        }
    }
}
