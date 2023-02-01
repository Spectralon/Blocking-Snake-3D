using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsController : MonoBehaviour
{
    public readonly PlayerDieEvent OnPlayerDie = new();

    public readonly PlayerWinEvent OnPlayerWin = new();

    private GameController GameController;

    public void Init(GameController gameController)
    {
        GameController = gameController;
        OnPlayerDie.RemoveAllListeners();
        OnPlayerWin.RemoveAllListeners();
    }

    public void PlayerDied(Snake player)
    {
        OnPlayerDie.Invoke(player);
    }

    public void PlayerWon(Snake player)
    {
        OnPlayerWin.Invoke(player);
    }
}
