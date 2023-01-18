using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CubeObstacle : FlowingObject
{
    private static int Contacts = 0;

    private int _headInstanceID = -1;
    private int HeadInstanceID
    {
        set => _headInstanceID = value;
        get
        {
            if (_headInstanceID < 0)
            {
                _headInstanceID = GameController?.Snake?.Head.gameObject.GetInstanceID() ?? -1;
            }
            return _headInstanceID;
        }
    }

    private void Awake()
    {
        Contacts = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() != HeadInstanceID) return;

        if (Contacts == 0) GameController.PauseFlow();
        Contacts++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetInstanceID() != HeadInstanceID) return;

        Contacts--;
        if (Contacts == 0) GameController.ResumeFlow();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;

        if (other.gameObject.GetInstanceID() != HeadInstanceID) return;

        if (Contacts == 0) GameController.PauseFlow();
        Contacts++;
    }

    private void OnCollisionExit(Collision collision)
    {
        Collider other = collision.collider;

        if (other.gameObject.GetInstanceID() != HeadInstanceID) return;

        Contacts--;
        if (Contacts == 0) GameController.ResumeFlow();
    }
}
