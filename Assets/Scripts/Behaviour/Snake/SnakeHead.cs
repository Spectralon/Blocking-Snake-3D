using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class SnakeHead : MonoBehaviour
{
    [SerializeField] private ParticleSystem _winParticles;
    public ParticleSystem WinParticles => _winParticles;


    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody
    {
        get
        {
            if (_rigidbody == null) TryGetComponent(out _rigidbody);
            return _rigidbody;
        }
    }

    public Snake Parent { get; private set; }

    private Sequence MoveSequence;

    public void Init(Snake parent)
    {
        Parent = parent;
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
