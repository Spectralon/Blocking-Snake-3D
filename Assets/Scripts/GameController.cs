using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class GameController : MonoBehaviour
{
    const string LevelKey = "LevelIndex";
    const string BestScoreKey = "BestScore";
    const string CachedScoreKey = "CachedScore";

    public readonly static System.Random Random = new(); //{ get; private set; }
    public readonly static bool IsDebug = true;

    #region Unity Editor input

    [SerializeField] UIController _UIController;
    [SerializeField] EventsController _eventsController;
    [SerializeField] Controls _controls;
    [SerializeField] Snake _snake;
    [SerializeField] FlowSpawner[] _obstacleGenerators;
    [SerializeField] FlowSpawner _finishGenerator;
    [SerializeField, Min(0.001f)] float _gameSpeed = 1f;
    [SerializeField, Min(0.001f)] float _gameCycleTime = 60f;
    [SerializeField, Min(0.1f)] float _finishDelay = 0.5f;

    public EventsController EventsController => _eventsController;
    public UIController UIController => _UIController;
    public Controls Controls => _controls;
    public Snake Snake => _snake;
    public FlowSpawner[] ObstacleGenerators => _obstacleGenerators;
    public FlowSpawner FinishGenerator => _finishGenerator;
    public float GameSpeed => _gameSpeed;
    public float GameCycleTime => _gameCycleTime;
    public float FinishDelay => _finishDelay;

    #endregion


    public int CachedScore
    {
        get => PlayerPrefs.GetInt(CachedScoreKey, 0);
        private set
        {
            PlayerPrefs.SetInt(CachedScoreKey, value);
            PlayerPrefs.Save();
        }
    }

    public int Best
    {
        get => PlayerPrefs.GetInt(BestScoreKey, 0);
        private set
        {
            PlayerPrefs.SetInt(BestScoreKey, value);
            PlayerPrefs.Save();
        }
    }

    public int Level
    {
        get => PlayerPrefs.GetInt(LevelKey, 0);
        private set
        {
            PlayerPrefs.SetInt(LevelKey, value);
            PlayerPrefs.Save();
        }
    }

    public State GameState { get; private set; } = State.Ingame;

    public bool IsPlaying => GameState == State.Ingame;

    public bool IsPaused { get; private set; } = false;

    private float CurrentTime = 0;

    private void Awake()
    {
        DOTween.Clear();
        DOTween.ClearCachedTweens();
        GameState = State.Ingame;
    }

    private void Start()
    {
        Snake.Init(this);
        Controls.Init(this);
        EventsController.Init(this);
        UIController.Init(this);

        foreach (var spawner in ObstacleGenerators)
        {
            spawner.FlowTime /= GameSpeed;
            spawner.SpawnInterval /= GameSpeed;
            spawner.Init(this);
        }

        FinishGenerator.Init(this);

        StartCoroutine(WinTimer());
    }

    public IEnumerator WinTimer()
    {
        float targetTime = FinishDelay + GameCycleTime;
        bool finishSpawned = false;

        while (targetTime > CurrentTime)
        {
            if (!IsPaused)
            {
                CurrentTime += Time.deltaTime;
                UIController.SetProgress(CurrentTime / targetTime);

                if (CurrentTime > GameCycleTime && !finishSpawned)
                {
                    foreach (var spawner in ObstacleGenerators)
                    {
                        spawner.Pause(false);
                    }
                    FinishGenerator.Spawn();

                    finishSpawned = true;
                }
            }
            yield return null;
        }
    }

    public void Lose(Snake player)
    {
        GameState = State.Loss;
        EventsController.PlayerDied(player);
        PauseFlow();
    }

    public void Win(Snake player)
    {
        GameState = State.Won;
        EventsController.PlayerWon(player);
        PauseFlow();
    }

    public void PauseFlow()
    {
        if (!IsPlaying) return;
        foreach (var spawner in ObstacleGenerators)
        {
            spawner.Pause();
        }
        FinishGenerator.Pause();
        IsPaused = true;
    }

    public void ResumeFlow()
    {
        if (!IsPlaying) return;
        foreach (var spawner in ObstacleGenerators)
        {
            spawner.Resume();
        }
        FinishGenerator.Resume();
        IsPaused = false;
    }

    public void ReloadLevel()
    {
        Snake.Score = CachedScore;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Level++;
        CachedScore = Snake.Score;
        ReloadLevel();
    }

    public enum State
    {
        Ingame,
        Won,
        Loss
    }
}
