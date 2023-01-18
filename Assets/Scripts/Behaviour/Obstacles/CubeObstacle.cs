using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CubeObstacle : FlowingObject
{
    private static List<CubeObstacle> ContactingObstacles = new();

    private static int Contacts => ContactingObstacles.Count;

    #region Unity Editor input

    [SerializeField] TMP_Text _label;
    [SerializeField] int _minValue = 1;
    [SerializeField] int _maxValue = 5;
    [SerializeField, Min(0)] float _damageInterval = 0.1f;
    [SerializeField, Min(0)] float _waitInterval = 0.1f;

    private TMP_Text Label => _label;
    private int MinValue => _minValue;
    private int MaxValue => _maxValue;
    private float DamageInterval => _damageInterval;
    private float WaitInterval => _waitInterval;

    #endregion

    private int _headInstanceID = -1;
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

    private int HeadInstanceID
    {
        set => _headInstanceID = value;
        get
        {
            if (_headInstanceID < 0)
            {
                _headInstanceID = GameController?.Snake?.Head.gameObject.GetInstanceID() ?? -1;
            }
            return _headInstanceID;
        }
    }

    private Snake DamageTarget;

    public override void Spawn()
    {
        Value = GameController.Random.Range(MinValue, MaxValue);
        base.Spawn();
        StartCoroutine(DamageDealer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<SnakeHead>(out var player)) return;

        DamageTarget = player.Parent;
        if (Contacts == 0) GameController.PauseFlow();
        ContactingObstacles.Add(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetInstanceID() != HeadInstanceID) return;

        DamageTarget = null;
        ContactingObstacles.Remove(this);
        if (Contacts == 0) GameController.ResumeFlow();
    }

    private void OnDisable()
    {
        StopCoroutine(DamageDealer());
    }

    private IEnumerator DamageDealer()
    {
        var damageInterval = new WaitForSeconds(DamageInterval);
        var interval = new WaitForSeconds(WaitInterval);
        var gameObject = this.gameObject;

        while (gameObject.activeSelf)
        {
            if(DamageTarget != null)
            {
                if (DamageTarget != null && Value > 0)
                {
                    DamageTarget.AddHP(-1);
                    Value--;
                    yield return damageInterval;
                }

                if (Value <= 0)
                {
                    DamageTarget = null;
                    ContactingObstacles.Remove(this);
                    Despawn();
                    if (Contacts == 0) GameController.ResumeFlow();
                }
            }
            yield return interval;
        }
    }

    private void OnApplicationQuit()
    {
        ContactingObstacles = new();
    }
}
