using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWoodRemove : MonoBehaviour
{
    public GameObject wood;

    private bool isHiden = true;

    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {

        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    isHiden=false ;
                    wood.SetActive(isHiden);
                }
            }

        }

    }
}
