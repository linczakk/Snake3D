using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameplayController))]
public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance;

    private Text _scoreText;
    private int _scoreCount = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        _scoreText = GameObject.Find("Score").GetComponent<Text>();
    }

    public int GetScoreCount() => _scoreCount;

    public void IncreaseScore()
    {
        _scoreCount++;
        SetScoreUI();
    }

    public void DecreaseScore()
    {
        _scoreCount--;
        SetScoreUI();
    }

    private void SetScoreUI() => _scoreText.text = "Score: " + _scoreCount;

    public void SetBestScore(GameObject bestScoreHeaderUI, Text bestScoreTextUI)
    {
        var bestScore = default(int);

        if (BestScoreHandler.IsScoreBetter(_scoreCount, out bestScore))
        {
            bestScoreHeaderUI.SetActive(true);
        }

        bestScoreTextUI.text = bestScore.ToString();
    }
}
