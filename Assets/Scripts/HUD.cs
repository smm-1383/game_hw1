using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText, highScoreText;
    [SerializeField] Image[] hearts;
    [SerializeField] Player player;

    void Update()
    {
        var gm = GameManager.Instance;
        scoreText.text = "Score: " + gm.Score;
        highScoreText.text = "Best: " + gm.DisplayHigh;
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].color = i < player.Lives ? Color.white : new Color(0.25f, 0.25f, 0.25f, 0.6f);
    }
}
