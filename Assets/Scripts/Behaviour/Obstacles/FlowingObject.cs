using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingObject : MonoBehaviour
{
    public bool IsActive { get; private set; } = true;

    protected FlowSpawner Parent;
    protected Sequence Sequence;
    protected GameController GameController;

    public virtual void Init(FlowSpawner parent, Sequence sequence, GameController gameController) 
    {
        Parent = parent;
        Sequence = sequence;
        GameController = gameController;
    }

    public void TogglePause()
    {
        IsActive = !IsActive;

        if (IsActive) Sequence?.Play();
        else Sequence?.Pause();
    }

    public virtual bool CanSpawnAt(Vector3 pos)
    {
        return true;
    }

    public virtual void Spawn()
    {
        gameObject.SetActive(true);
    }

    public virtual void Despawn()
    {
        Sequence?.Complete();
        Sequence = null;
        gameObject.SetActive(false);
    }
}
