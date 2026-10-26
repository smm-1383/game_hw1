using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] TMP_Text highScoreText;

    void Start()
    {
        if (highScoreText) highScoreText.text = "Best: " + PlayerPrefs.GetInt("HighScore", 0);
    }

    public void PlayGame() => SceneManager.LoadScene("Game");

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void Resume() => GameManager.Instance.Resume();

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
