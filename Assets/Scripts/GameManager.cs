using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] float baseSpeed = 6f, speedRamp = 0.1f, maxSpeed = 18f, dashMultiplier = 2f, scorePerUnit = 1f;
    [SerializeField] GameObject pausePanel, gameOverPanel;

    public bool DashActive { get; set; }
    public bool IsOver { get; private set; }
    public int HighScore { get; private set; }
    public float Speed => IsOver ? 0f : baseSpeed * dashMul;
    public int Score => (int)score;
    public int DisplayHigh => Mathf.Max(HighScore, Score);

    float score, dashMul = 1f;
    InputAction pause;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        pause = new InputAction("Pause", InputActionType.Button, "<Keyboard>/escape");
    }

    void OnEnable() => pause.Enable();
    void OnDisable() => pause.Disable();

    void Start()
    {
        if (pausePanel) pausePanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (pause.WasPressedThisFrame() && !IsOver)
        {
            if (Time.timeScale == 0f) Resume(); else Pause();
        }
        if (IsOver || Time.timeScale == 0f) return;
        dashMul = Mathf.MoveTowards(dashMul, DashActive ? dashMultiplier : 1f, (dashMultiplier - 1f) / 0.1f * Time.deltaTime);
        baseSpeed = Mathf.Min(maxSpeed, baseSpeed + speedRamp * Time.deltaTime);
        score += Speed * Time.deltaTime * scorePerUnit;
    }

    public void AddScore(int points) => score += points;

    public void Pause()
    {
        if (IsOver) return;
        Time.timeScale = 0f;
        if (pausePanel) pausePanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        if (pausePanel) pausePanel.SetActive(false);
    }

    public void GameOver()
    {
        IsOver = true;
        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt("HighScore", HighScore);
            PlayerPrefs.Save();
        }
        if (gameOverPanel) gameOverPanel.SetActive(true);
    }
}
