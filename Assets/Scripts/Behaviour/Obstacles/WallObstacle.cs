using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WallObstacle : FlowingObject
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out SnakeHead player)) return;

        if (player.Controls.ControlsState == Controls.State.Idle) Parent.Pause();   // TODO: Вынести в GameController и засинхронить для всех спавнеров
        else
            player.Controls.Limit();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out SnakeHead player)) return;
        Parent.Resume();    // TODO: Вынести в GameController и засинхронить для всех спавнеров
        player.Controls.Unlimit();
    }
}
