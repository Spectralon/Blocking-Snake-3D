using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] Rigidbody _headPrefab;
    [SerializeField] HingeJoint _segmentPrefab;
    [SerializeField, Min(0)] float _interval = 2.5f;
    [SerializeField, Min(1)] int _maxSegments = 5;
    [SerializeField] float _velocityFactor = 2.5f;
    [SerializeField] Vector3 _growDirection = Vector3.back;

    public Vector3 GrowDirection => _growDirection;

    private Rigidbody _head;

    public Rigidbody Head
    {
        get
        {
            if (_head == null)
            {
                _head = Instantiate(_headPrefab.gameObject, transform).GetComponent<Rigidbody>();
                _head.constraints = RigidbodyConstraints.FreezeAll;
                _head.gameObject.SetActive(false);
            }
            return _head;
        }
    }

    private Rigidbody[] _segments;

    public Rigidbody[] Segments
    {
        get
        {
            if (_segments == null)
            {
                _segments = new Rigidbody[_maxSegments];
                int lastIndex = _maxSegments - 1;

                for (int i = 0; i < _maxSegments; i++)
                {
                    var segment = Instantiate(_segmentPrefab.gameObject, transform).GetComponent<HingeJoint>();
                    var prevSegment = i == 0 ? Head : _segments[i - 1];

                    segment.connectedBody = prevSegment;
                    segment.transform.localPosition = prevSegment.transform.localPosition + _interval * _growDirection;

                    if (i < lastIndex && segment.TryGetComponent<Collider>(out var collider)) collider.enabled = false;

                    _segments[i] = segment.GetComponent<Rigidbody>();
                    _segments[i].constraints = RigidbodyConstraints.FreezePositionY;
                    _segments[i].gameObject.SetActive(false);
                }

                _segments[lastIndex].gameObject
                    .AddComponent<SnakeTail>()
                    .Init(this, _velocityFactor);
            }
            return _segments;
        }
    }

    public bool IsInit { get; private set; } = false;

    private GameController GameController;

    public void Init(GameController gameController)
    {
        GameController = gameController;

        GrowDirection.Normalize();

        Head.GetComponent<SnakeHead>().Init(GameController.Controls);
        Head.gameObject.SetActive(true);

        foreach (var segment in Segments)
            segment.gameObject.SetActive(true);

        IsInit = true;
    }
}
