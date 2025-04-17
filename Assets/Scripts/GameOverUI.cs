using UnityEngine;
using TMPro; // Add this namespace for TextMeshPro

public class GameOverUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI winnerText; // Change the type to TextMeshProUGUI

    public void ShowGameOver(string result)
    {
        panel.SetActive(true);
        winnerText.text = $"{result}";
    }

    public void HideGameOver()
    {
        panel.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("ChessScene");
    }
}