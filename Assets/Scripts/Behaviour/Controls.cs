using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    #region Unity Editor input

    [SerializeField] float _maxDistance = 10f;
    [SerializeField, Min(0)] float _sensitivity = 10f;

    public float MaxDistance => _maxDistance;
    public float Sensitivity => _sensitivity;

    #endregion

    public State ControlsState { get; private set; } = State.Idle;

    public bool IsInit { get; private set; } = false;

    private SnakeHead Head;
    private GameController GameController;
    private float CurrentDistance = 0f;
    private bool LeftLimit = false;
    private bool RightLimit = false;

    public void Init(GameController gameController)
    {
        GameController = gameController;
        Head = GameController.Snake.Head.GetComponent<SnakeHead>();
        IsInit = true;
    }

    private void Update()
    {
        if (!IsInit) return;

        bool moving = false;

        if (!LeftLimit && CurrentDistance >= -MaxDistance && Input.GetKey(KeyCode.LeftArrow))
        {
            float distance = Sensitivity * Time.deltaTime;
            Head.TryMove(Vector3.left * distance);
            CurrentDistance -= distance;
            ControlsState = State.MovingLeft;
            moving = true;
        }

        if (!RightLimit && CurrentDistance <= MaxDistance && Input.GetKey(KeyCode.RightArrow))
        {
            float distance = Sensitivity * Time.deltaTime;
            Head.TryMove(Vector3.right * distance);
            CurrentDistance += distance;
            ControlsState = State.MovingRight;
            moving = true;
        }

        if(!moving) ControlsState = State.Idle;
    }

    public void Limit()
    {
        switch (ControlsState)
        {
            case State.MovingLeft:
                LeftLimit = true;
                Head.InterruptMove();
                break;
            case State.MovingRight:
                RightLimit = true;
                Head.InterruptMove();
                break;
            default:
                break;
        }
    }

    public void Unlimit()
    {
        LeftLimit = false;
        RightLimit = false;
    }

    public enum State
    {
        Idle,
        MovingLeft,
        MovingRight
    }
}
