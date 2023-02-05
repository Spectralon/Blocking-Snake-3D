using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Finish : FlowingObject
{
    private Collider _collider;

    public Collider Collider
    {
        get
        {
            if (_collider == null) TryGetComponent(out _collider);
            return _collider;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameController.IsPlaying || !other.TryGetComponent<SnakeHead>(out var player)) return;

        player.Parent.Win();
    }
}
