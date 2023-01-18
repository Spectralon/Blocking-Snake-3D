using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WallObstacle : FlowingObject
{
    private int _headInstanceID = -1;

    private int HeadInstanceID
    {
        set => _headInstanceID = value;
        get
        {
            if(_headInstanceID < 0)
            {
                _headInstanceID = GameController?.Snake?.Head.gameObject.GetInstanceID() ?? -1;
            }
            return _headInstanceID;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() != HeadInstanceID) return;

        if (GameController.Controls.ControlsState == Controls.State.Idle) 
            GameController.PauseFlow();
        else
            GameController.Controls.Limit();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetInstanceID() != HeadInstanceID) return;

        GameController.ResumeFlow();
        GameController.Controls.Unlimit();
    }
}
