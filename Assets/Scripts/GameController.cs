using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public readonly static System.Random Random = new(); //{ get; private set; }
    public readonly static bool Debug = true;

    [SerializeField] Controls _controls;
    [SerializeField] Snake _snake;

    public Controls Controls => _controls;
    public Snake Snake => _snake;

    private void Start()
    {
        Snake.Init(this);
        Controls.Init(this);
    }
}
