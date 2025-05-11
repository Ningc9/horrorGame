using UnityEngine;

public class MusicNote : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.CollectNote();
            Destroy(gameObject);
        }
    }
}
