using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWinEvent : UnityEvent<PlayerWinEvent>
{
    public Snake Player { get; private set; }

    public void Invoke(Snake player)
    {
        Player = player;
        base.Invoke(this);
    }
}
