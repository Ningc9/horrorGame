using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int stoneCount = 0;
    private int notesCollected = 0;

    public GameObject musicReward;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddStone()
    {
        stoneCount++;
        Debug.Log("Stones collected: " + stoneCount);
    }

    public bool HasAllStones()
    {
        return stoneCount >= 3;
    }

    public void CollectNote()
    {
        notesCollected++;
        Debug.Log("Music notes collected: " + notesCollected);

        if (notesCollected >= 5 && musicReward != null)
        {
            musicReward.SetActive(true);
            AddStone();
        }
    }
}
