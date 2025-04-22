using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    private GameObject music;

    private void Start()
    {
        music = GameObject.Find("BackgroundMusic");
    }
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Optional: freeze game
        music.GetComponent<AudioSource>().Stop();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        music.GetComponent<AudioSource>().Play();
    }

    public void RestartMenu()
    {
        SceneManager.LoadScene("menu");
        music.GetComponent<AudioSource>().Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
