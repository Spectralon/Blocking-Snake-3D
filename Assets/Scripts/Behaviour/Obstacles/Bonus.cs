using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bonus : FlowingObject
{
    #region Unity Editor input

    [SerializeField] int _minValue = 1;
    [SerializeField] int _maxValue = 2;

    private int MinValue => _minValue;
    private int MaxValue => _maxValue;

    #endregion

    private Collider _collider;
    private int _value;

    public Collider Collider
    {
        get
        {
            if (_collider == null) TryGetComponent(out _collider);
            return _collider;
        }
    }

    public int Value
    {
        get => _value;
        private set => _value = value;
    }

    public override void Spawn()
    {
        Value = GameController.Random.Range(MinValue, MaxValue);
        base.Spawn();
    }

    public override bool CanSpawnAt(Vector3 pos)
    {
        return base.CanSpawnAt(pos) && Physics.OverlapSphere(pos, 0.1f).Length == 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<SnakeHead>(out var player)) return;

        player.Parent.AddHP(Value);

        Despawn();
    }
}
