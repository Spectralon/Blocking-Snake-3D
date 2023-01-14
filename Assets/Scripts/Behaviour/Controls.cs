using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField] float _maxDistance = 10f;

    [SerializeField] float _minDistance = -10f;

    [SerializeField] float _sensitivity = 10f;

    public float MaxDistance => _maxDistance;

    public float MinDistance => _minDistance;

    public float Sensitivity => _sensitivity;

    public bool IsInit { get; private set; } = false;

    private Transform Head;

    private GameController GameController;

    private float Distance = 0f;

    public void Init(GameController gameController)
    {
        GameController = gameController;
        Head = GameController.Snake.Head.GetComponent<Transform>();
        IsInit = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsInit) return;

        if (Distance >= MinDistance && Input.GetKey(KeyCode.LeftArrow))
        {
            float distance = Sensitivity * Time.deltaTime;
            Head.Translate(Vector3.left * distance);
            Distance -= distance;
        }

        if (Distance <= MaxDistance && Input.GetKey(KeyCode.RightArrow))
        {
            float distance = Sensitivity * Time.deltaTime;
            Head.Translate(Vector3.right * distance);
            Distance += distance;
        }
    }
}
