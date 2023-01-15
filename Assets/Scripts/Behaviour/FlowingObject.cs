using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingObject : MonoBehaviour
{
    protected FlowSpawner Parent;

    protected Sequence Sequence;

    public void Init(FlowSpawner parent, Sequence sequence) 
    {
        Parent = parent;
        Sequence = sequence;
    }

    public void TogglePause()
    {
        Sequence?.TogglePause();
    }
}
