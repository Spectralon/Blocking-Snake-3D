using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public readonly static System.Random Random = new(); //{ get; private set; }
    public readonly static bool Debug = true;

    #region Unity Editor input

    [SerializeField] Controls _controls;
    [SerializeField] Snake _snake;
    [SerializeField] FlowSpawner[] _obstacleGenerators;
    [SerializeField, Min(0.001f)] float _gameSpeed = 1f;

    public Controls Controls => _controls;
    public Snake Snake => _snake;
    public FlowSpawner[] ObstacleGenerators => _obstacleGenerators;
    public float GameSpeed => _gameSpeed;

    #endregion

    private void Start()
    {
        Snake.Init(this);
        Controls.Init(this);

        foreach (var spawner in ObstacleGenerators)
        {
            spawner.FlowTime /= GameSpeed;
            spawner.SpawnInterval /= GameSpeed;
            spawner.Init(this);
        }
    }

    public void PauseFlow()
    {
        foreach (var spawner in ObstacleGenerators)
        {
            spawner.Pause();
        }
    }

    public void ResumeFlow()
    {
        foreach (var spawner in ObstacleGenerators)
        {
            spawner.Resume();
        }
    }
}
