using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] Rigidbody _headPrefab;

    [SerializeField] HingeJoint _segmentPrefab;

    [SerializeField] float _interval = 2.5f;

    [SerializeField] int _maxSegments = 5;

    [SerializeField] Vector3 _direction = Vector3.back;

    [SerializeField] float _velocityFactor = 2.5f;

    public Vector3 Direction => _direction;

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
                    segment.transform.localPosition = prevSegment.transform.localPosition + _interval * _direction;

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

    private void Start()
    {
        Head.gameObject.SetActive(true);

        foreach (var segment in Segments)
            segment.gameObject.SetActive(true);


    }
}
