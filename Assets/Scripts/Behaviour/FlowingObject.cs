using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingObject : MonoBehaviour
{
    private FlowSpawner Parent;

    private Sequence Sequence;

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
