using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingObject : MonoBehaviour
{
    protected FlowSpawner Parent;
    protected Sequence Sequence;
    protected GameController GameController;

    public void Init(FlowSpawner parent, Sequence sequence, GameController gameController) 
    {
        Parent = parent;
        Sequence = sequence;
        GameController = gameController;
    }

    public void TogglePause()
    {
        Sequence?.TogglePause();
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
