using TMPro;
using UnityEngine;

public class GameOverStats : MonoBehaviour
{
    [SerializeField] TMP_Text finalText, bestText;

    void OnEnable()
    {
        var gm = GameManager.Instance;
        if (!gm) return;
        finalText.text = "Score: " + gm.Score;
        bestText.text = "Best: " + gm.HighScore;
    }
}
