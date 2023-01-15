using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Vector3 LastNormal = Vector3.up;

    public void Init(Controls controls)
    {
        Controls = controls;
    }

    public void TryMove(Vector3 direction)
    {
        //if (Vector3.Dot(direction, LastNormal) < 0)
        //{
        //    Debug.Log(Vector3.Dot(direction, LastNormal));
        //    return;
        //}
        transform.Translate(direction);
    }

    public Vector3 Project(Vector3 forward)
    {
        return forward - Vector3.Dot(forward, LastNormal) * LastNormal;
    }

    private void OnCollisionEnter(Collision collision)
    {
        LastNormal = collision.contacts[0].normal;
        //if (Vector3.Dot(Rigidbody.velocity.normalized, LastNormal) < 0) Controls.Limit();
    }

    private void OnCollisionExit(Collision collision)
    {
        LastNormal = Vector3.up;
        //Controls.Unlimit();
    }
}
