using UnityEngine;

public class AnswerChecker : MonoBehaviour
{
    public GameObject questionPanel;
    public GameObject rewardObject;

    public void ChooseAnswer(string answer)
    {
        if (answer == "C")
        {
            Debug.Log("Correct! You chose the right mirror.");
            if (rewardObject != null)
                rewardObject.SetActive(true);
        }
        else
        {
            Debug.Log("Wrong answer.");
        }

        if (questionPanel != null)
            questionPanel.SetActive(false);
    }
}
