using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WallObstacle : FlowingObject
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out SnakeHead player)) return;

        if (player.Controls.ControlsState == Controls.State.Idle) GameController.PauseFlow();
        else
            player.Controls.Limit();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out SnakeHead player)) return;
        GameController.ResumeFlow();
        player.Controls.Unlimit();
    }
}
