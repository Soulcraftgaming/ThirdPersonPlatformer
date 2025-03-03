using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Fields to adjust in Inspector
    [SerializeField] private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;


    //GameManager Singleton - used to initalize GameObject
    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;

        //show updated score on GUI
        scoreText.text = $"Score: {score}"; 
    }

    public int GetScore()
    {
        return score;
    }
}
