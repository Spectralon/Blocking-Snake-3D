using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowSpawner : MonoBehaviour
{
    #region Unity Editor input

    [SerializeField] FlowingObject _baseInstance;
    [SerializeField] Vector3 _spawnDirection = Vector3.right;
    [SerializeField, Min(0f)] float _intervalDistance = 5f;
    [SerializeField, Min(1)] int _flowLength = 5;
    [SerializeField, Min(1)] int _maxInstances = 20;
    [SerializeField] Vector3 _flowDirection = Vector3.back;
    [SerializeField, Min(0f)] float _flowDistance = 40f;
    [SerializeField, Min(0.01f)] float _flowTime = 10f;
    [SerializeField, Range(0f, 1f)] float _spawnChance = 0.1f;
    [SerializeField, Min(0.01f)] float _spawnInterval = 2f;

    public Vector3 SpawnDirection => _spawnDirection;
    public float IntervalDistance => _intervalDistance;
    public int FlowLength => _flowLength;
    public int MaxInstances => _maxInstances;
    public Vector3 FlowDirection => _flowDirection;
    public float FlowDistance => _flowDistance;
    public float FlowTime => _flowTime;
    public float SpawnChance => _spawnChance;
    public float SpawnInterval => _spawnInterval;

    #endregion

    private FlowingObject[] _instances;

    public bool IsActive { get; private set; } = true;

    private FlowingObject[] Instances
    {
        get
        {
            if (_baseInstance != null && _instances == null)
            {
                _instances = new FlowingObject[_maxInstances];

                for (int i = 0; i < _maxInstances; i++)
                {
                    var instance = Instantiate(_baseInstance.gameObject, transform);
                    instance.SetActive(false);
                    _instances[i] = instance.GetComponent<FlowingObject>();
                }
            }
            return _instances;
        }
        set => _instances = value;
    }

    private Sequence MoveSequence;
    private int NumInstances = 0;
    private GameController GameController;

    [ContextMenu("Init spawner")]
    public void Init(GameController gameController)
    {
        GameController = gameController;
        Reload();
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        var interval = new WaitForSeconds(SpawnInterval);

        while(true)
        {
            if(IsActive)
            {
                var pool = Instances.Where(e => !e.gameObject.activeInHierarchy).ToArray();
                for (int i = 0; i < Mathf.Min(FlowLength, pool.Length); i++)
                {
                    if (GameController.Random.Chance(SpawnChance))
                    {
                        Spawn(pool[i], i * IntervalDistance * SpawnDirection);
                    }
                }
            }
            yield return interval;
        }
    }

    public void Spawn(FlowingObject instance, Vector3 pos)
    {
        if (NumInstances >= MaxInstances || instance == null) return;

        instance.transform.localPosition = pos;
        instance.gameObject.SetActive(true);

        MoveSequence = DOTween.Sequence();
        MoveSequence
            .Append(instance.transform.DOLocalMove(pos + FlowDistance * FlowDirection, FlowTime))
            .SetEase(Ease.Linear)
            .AppendCallback(instance.Despawn);

        instance.Init(this, MoveSequence, GameController);
    }

    public void Pause()
    {
        if (IsActive) ToggleActive();
    }

    public void Resume()
    {
        if (!IsActive) ToggleActive();
    }

    [ContextMenu("Toggle Active")]
    public void ToggleActive()
    {
        IsActive = !IsActive;
        foreach (var item in Instances)
        {
            item.TogglePause();
        }
    }

    [ContextMenu("Reload spawner")]
    private void Reload()
    {
        if (Application.isPlaying)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }
        else
        {
            for (int i = transform.childCount; i > 0; --i)
                DestroyImmediate(transform.GetChild(0).gameObject);
        }
        SpawnDirection.Normalize();
        FlowDirection.Normalize();
        Instances = null;
    }

    private void OnDrawGizmos()
    {
        if (!GameController.Debug) return;

        if (_baseInstance == null ||
            !_baseInstance.TryGetComponent<Renderer>(out var renderer)
            ) return;

        Vector3 size = renderer.bounds.size;

        Color oldGizmos = Gizmos.color;
        Gizmos.color = Color.yellow;

        for (int i = 0; i < FlowLength; i++)
        {
            Gizmos.DrawWireCube(transform.position + i * IntervalDistance * SpawnDirection, size);
        }

        Gizmos.color = oldGizmos;
    }
}
