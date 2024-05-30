using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(RobotMover))]
public class Robot : MonoBehaviour
{
    [SerializeField] private RobotCollisionHandler _handler;
    public bool IsUsing => _isUsing;
    public bool IsBuildingBase => _isBuildingBase;
    public bool IsBringingOre => _isBringingOre;
    public event UnityAction<bool, Transform> MovingStateChanged;
    public event UnityAction<Robot> BuiltBase;
    public event UnityAction OreBrought;
    public event UnityAction WorkingStateChanged;

    private CollectorsBase _collectorsBasePrefab;
    private bool _isBringingOre;
    private Transform _storage;
    private bool _isUsing;
    private bool _isBuildingBase;
    private Transform _oresReceiver;
    private RobotMover _mover;
    private Ore _target;
    private Coroutine _moveJob;

    private void OnEnable()
    {
        _handler.GetOre += GoBack;
        _handler.GetBaseBack += GetBase;
    }

    private void OnDisable()
    {
        _handler.GetOre -= GoBack;
        _handler.GetBaseBack -= GetBase;
    }

    private void Awake()
    {
        _mover = GetComponent<RobotMover>();
        _isUsing = false;
    }

    public void BringOre(Ore target)
    {
        _target = target;
        MovingStateChanged?.Invoke(true, target.gameObject.transform);
        _isUsing = true;
        _mover.SetTarget(_target);
        _handler.SetTarget(_target);
    }

    public void SetBase(Transform receiver, Transform storage)
    {
        _oresReceiver = receiver;
        _storage = storage;
    }

    public void GoToNewBaseFlag(Transform newBaseFlag,CollectorsBase collectorsBasePrefab)
    {
        _collectorsBasePrefab = collectorsBasePrefab;
        MovingStateChanged?.Invoke(true, newBaseFlag.gameObject.transform);
        _isUsing = true;
        _isBuildingBase = true;
    }

    private void GoBack()
    {
        MovingStateChanged?.Invoke(false, _oresReceiver);
        _mover.PickUpOre();
        MovingStateChanged?.Invoke(true, _oresReceiver);
    }

    public void BuildBase(Shop shop, Spawner spawner)
    {
        MovingStateChanged?.Invoke(false, _oresReceiver);
        CollectorsBase spawnedBase = Instantiate(_collectorsBasePrefab, transform.position, Quaternion.identity);
        BuiltBase?.Invoke(this);
        spawnedBase.AddNewRobot(this);
        spawnedBase.InitializeComponents(shop, spawner);
    }

    private void GetBase()
    {
        if (IsUsing)
        {
            MovingStateChanged?.Invoke(false, _target.gameObject.transform);
            OreBrought?.Invoke();
            _isUsing = false;
            WorkingStateChanged?.Invoke();
        }
    }
}