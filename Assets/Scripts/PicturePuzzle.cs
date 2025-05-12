using UnityEngine;

public class PicturePuzzle : MonoBehaviour
{
    public GameObject choicePanel;

    private void OnMouseDown()
    {
        if (choicePanel != null)
            choicePanel.SetActive(true);
    }
}
