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

    private int _value;

    public int Value
    {
        get => _value;
        private set => _value = value;
    }

    public void OnEnable()
    {
        Value = GameController.Random.Range(MinValue, MaxValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<SnakeHead>(out var player)) return;

        player.Parent.AddHP(Value);

        Despawn();
    }
}
