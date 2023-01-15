using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public readonly static System.Random Random = new(); //{ get; private set; }
    public readonly static bool Debug = true;

    [SerializeField] Controls _controls;
    [SerializeField] Snake _snake;
    [SerializeField] FlowSpawner[] _obstacleGenerators;

    public Controls Controls => _controls;
    public Snake Snake => _snake;
    public FlowSpawner[] ObstacleGenerators => _obstacleGenerators;

    private void Start()
    {
        Snake.Init(this);
        Controls.Init(this);

        foreach (var spawner in ObstacleGenerators)
        {
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
