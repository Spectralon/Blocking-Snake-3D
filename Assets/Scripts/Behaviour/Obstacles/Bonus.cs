using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bonus : FlowingObject
{
    #region Unity Editor input

    [SerializeField] TMP_Text _label;
    [SerializeField] int _minValue = 1;
    [SerializeField] int _maxValue = 2;

    private TMP_Text Label => _label;
    private int MinValue => _minValue;
    private int MaxValue => _maxValue;

    #endregion

    private Collider _collider;
    private int _value;

    public int Value
    {
        get => _value;
        private set
        {
            _value = value;
            if (_value > 0)
                Label.text = _value.ToString();
            else
                Label.text = "";
        }
    }

    public Collider Collider
    {
        get
        {
            if (_collider == null) TryGetComponent(out _collider);
            return _collider;
        }
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
