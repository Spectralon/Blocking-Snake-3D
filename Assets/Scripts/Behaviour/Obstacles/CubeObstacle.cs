using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CubeObstacle : FlowingObject
{
    private static int Contacts = 0;

    private void Awake()
    {
        Contacts = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out SnakeHead player)) return;

        if(Contacts == 0) Parent.Pause();   // TODO: Вынести в GameController и засинхронить для всех спавнеров
        Contacts++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out SnakeHead player)) return;

        Contacts--;
        if (Contacts == 0) Parent.Resume(); // TODO: Вынести в GameController и засинхронить для всех спавнеров
    }
}
