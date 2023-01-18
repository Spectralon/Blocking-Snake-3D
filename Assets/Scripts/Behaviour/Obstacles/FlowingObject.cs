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

    public void Despawn()
    {
        Sequence?.Complete();
        gameObject.SetActive(false);
    }
}
