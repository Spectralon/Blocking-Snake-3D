using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class SnakeHead : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody
    {
        get
        {
            if (_rigidbody == null) TryGetComponent(out _rigidbody);
            return _rigidbody;
        }
    }

    public Controls Controls { get; private set; }

    private Sequence MoveSequence;

    public void Init(Controls controls)
    {
        Controls = controls;
    }

    public void TryMove(Vector3 direction)
    {
        MoveSequence = DOTween.Sequence();
        MoveSequence.Append(transform.DOBlendableLocalMoveBy(direction, 0.01f));
    }

    public void InterruptMove()
    {
        MoveSequence.Pause();
    }
}
