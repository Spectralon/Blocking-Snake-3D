using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    const string BEST_TEXT = "BEST: {0}";
    const string BEST_TEXT_FORMATTED = "BEST:\n<size=200%>{0}</size>";
    const string SCORE_TEXT_FORMATTED = "SCORE:\n<size=200%>{0}</size>";

    [SerializeField] private GameObject IngameScreen;
    [SerializeField] private Image ProgressBGSprite;
    [SerializeField] private Image ProgressSprite;
    [SerializeField, Range(-1, 1)] float ProgressOffsetMax = 0;
    [SerializeField] private Image LevelBGSprite;
    [SerializeField] private Text LevelLabel;
    [SerializeField] private TextMeshProUGUI ScoreLabel;
    [SerializeField] private TextMeshProUGUI BestLabel;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private TextMeshProUGUI ScoreWinLabel;
    [SerializeField] private TextMeshProUGUI BestWinLabel;
    [SerializeField] private GameObject LoseScreen;
    [SerializeField] private TextMeshProUGUI ScoreLoseLabel;
    [SerializeField] private TextMeshProUGUI BestLoseLabel;
    [SerializeField] private Button NextLevelButton;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button ReviveButton;

    private int _level;
    private int _score;
    private int _best;

    public int Level
    {
        get => int.TryParse(LevelLabel.text, out _level) ? _level : -1;
        set
        {
            _level = value;
            LevelLabel.text = (_level + 1).ToString();
        }
    }

    public int Score
    {
        get => int.TryParse(ScoreLabel.text, out _score) ? _score : -1;
        set
        {
            _score = value;
            ScoreLabel.text = _score.ToString();
            ScoreWinLabel.text = string.Format(SCORE_TEXT_FORMATTED, _score);
            ScoreLoseLabel.text = string.Format(SCORE_TEXT_FORMATTED, _score);
        }
    }

    public int Best
    {
        get => int.TryParse(Regex.Match(ScoreLabel.text, @"\d+").Value, out _best) ? _best : -1;
        set
        {
            _best = value;
            BestLabel.text = string.Format(BEST_TEXT, _best);
            BestWinLabel.text = string.Format(BEST_TEXT_FORMATTED, _best);
            BestLoseLabel.text = string.Format(BEST_TEXT_FORMATTED, _best);
        }
    }

    private GameController GameController;

    public void Init(GameController gameController)
    {
        GameController = gameController;
        GameController.EventsController.OnPlayerDie.AddListener(e => SetScreen(Screen.GameOver));
        GameController.EventsController.OnPlayerWin.AddListener(e => SetScreen(Screen.Win));

        NextLevelButton.onClick.AddListener(GameController.NextLevel);
        RestartButton.onClick.AddListener(GameController.ReloadLevel);

        SyncStats();
    }

    public void SetProgress(float progress) => ProgressSprite.fillAmount = Mathf.Clamp(progress * (1 + ProgressOffsetMax), 0, 1);

    public void SetScreen(Screen screen)
    {
        SyncStats();
        switch (screen)
        {
            case Screen.Ingame:
                IngameScreen.SetActive(true);
                WinScreen.SetActive(false);
                LoseScreen.SetActive(false);
                break;
            case Screen.GameOver:
                WinScreen.SetActive(false);
                LoseScreen.SetActive(true);
                break;
            case Screen.Win:
                WinScreen.SetActive(true);
                LoseScreen.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void SyncStats()
    {
        Level = GameController.Level;
        Score = GameController.Snake.Score;
        Best = GameController.Best;
    }

    public enum Screen
    {
        Ingame,
        GameOver,
        Win
    }
}
