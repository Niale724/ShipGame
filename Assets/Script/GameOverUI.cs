using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour

{

//ui references

    [Header("UI Reference")]

    [SerializeField] private TextMeshProUGUI hpScoreText;  //display final hp
    [SerializeField] private TextMeshProUGUI finalScoreText; // displays fish collected

    [SerializeField] private TextMeshProUGUI resultText; //shows victory or game over

    [SerializeField] private Button restartButton;

    
    private void Start()
    {
        DisplayScores();


        restartButton.onClick.AddListener(RestartGame);
    }

    private void DisplayScores()
    {
        int finalHP = PlayerPrefs.GetInt("FinalHP", 0);
        int finalFishScore = PlayerPrefs.GetInt("FinalScore", 0);
        string result = PlayerPrefs.GetString("GameResult", "Game Over!");


        if (result == "Victory")
        {
            resultText.text = "You won!";
            resultText.color = Color.green;

        }

        else
        {
            resultText.text = "Game Over";


        }
            
        //display final stats
        hpScoreText.text = $"Final HP: {finalHP}";

        finalScoreText.text = $"Fish Collected: {finalFishScore} / 100";
        }




private void RestartGame()
    {
    SceneManager.LoadScene("GameScene");

    }
}
