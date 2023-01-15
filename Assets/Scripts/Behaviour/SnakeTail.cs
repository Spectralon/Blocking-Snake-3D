using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SnakeTail : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody
    {
        get
        {
            if (_rigidbody == null) TryGetComponent(out _rigidbody);
            return _rigidbody;
        }
        private set => _rigidbody = value;
    }

    public bool IsInit { get; private set; } = false;

    private Snake Parent;

    private float VelocityFactor;

    public void Init(Snake parent, float velocityFactor)
    {
        Parent = parent;
        VelocityFactor = velocityFactor;
        IsInit = true;
    }

    // �������� �� ��, ��� ��������� ������ ���������� �������� � FixedUpdate,
    // � ����� ������ ����������� ����� �������� ��������� ���� ��� � Update.
    // ����� ���� ����������� ������� �� �����, � ���������� Update ����������� ����.
    // � ��� ����� ������� ����������� ��������������� ��������.
    private void Update()
    {
        if (!IsInit) return;

        Rigidbody.AddForce(Parent.GrowDirection * VelocityFactor);
    }
}
