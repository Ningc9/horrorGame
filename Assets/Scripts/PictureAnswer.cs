using UnityEngine;

public class PictureAnswer : MonoBehaviour
{
    public GameObject choicePanel;
    public GameObject rewardObject;

    public void Choose(string answer)
    {
        if (answer == "A")
        {
            Debug.Log("Correct answer for Picture Puzzle.");
            if (rewardObject != null)
                rewardObject.SetActive(true);

            GameManager.instance.AddStone();
        }
        else
        {
            Debug.Log("Wrong picture answer.");
        }

        if (choicePanel != null)
            choicePanel.SetActive(false);
    }
}
