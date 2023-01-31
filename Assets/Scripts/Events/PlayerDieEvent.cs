using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDieEvent : UnityEvent<PlayerDieEvent>
{
    public Snake Player { get; private set; }

    public void Invoke(Snake player)
    {
        Player = player;
        base.Invoke(this);
    }
}

