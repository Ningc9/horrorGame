using UnityEngine;

public class MirrorTrigger : MonoBehaviour
{
    public GameObject questionPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (questionPanel != null)
                questionPanel.SetActive(true);
        }
    }
}
