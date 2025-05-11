using UnityEngine;
using UnityEngine.SceneManagement;

public class StoneDoor : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManager.instance.HasAllStones())
        {
            Debug.Log("Door opened.");
            // SceneManager.LoadScene("NextLevel");
        }
        else
        {
            Debug.Log("You need all 3 stones to open the door.");
        }
    }
}
