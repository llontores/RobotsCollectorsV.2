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
    public event UnityAction<bool, Transform> MovingStateChanged;
    public event UnityAction<Robot> BuiltBase;
    public event UnityAction OreBrought;

    public event UnityAction WorkingStateChanged;
    private CollectorsBase _collectorsBasePrefab;
    private bool _isUsing;
    private Transform _startPosition;
    private RobotMover _mover;
    private Ore _target;
    private Coroutine _moveJob;
    private NewBaseFlag _newBaseFlag;
    

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
        _startPosition = receiver;

    }

    public void GoToNewBaseFlag(NewBaseFlag newBaseFlag,CollectorsBase collectorsBasePrefab)
    {
        _collectorsBasePrefab = collectorsBasePrefab;
        MovingStateChanged?.Invoke(true, newBaseFlag.gameObject.transform);
        _isUsing = true;
        _newBaseFlag = newBaseFlag;
    }

    private void GoBack()
    {
        MovingStateChanged?.Invoke(false, _startPosition);
        _mover.PickUpOre();
        MovingStateChanged?.Invoke(true, _startPosition);
    }

    public void BuildBase(Shop shop, Spawner spawner)
    {
        MovingStateChanged?.Invoke(false, _startPosition);
        CollectorsBase spawnedBase = Instantiate(_collectorsBasePrefab, transform.position, Quaternion.identity);
        BuiltBase?.Invoke(this);
        spawnedBase.AddNewRobot(this);
        spawnedBase.InitializeComponents(shop, spawner);
        _startPosition = spawnedBase.gameObject.transform;
        OresCounter newBaseOresCounter = spawnedBase.GetComponent<OresCounter>();
        newBaseOresCounter.AddRobot(this);
        _newBaseFlag.gameObject.SetActive(false );
    }

    private void GetBase()
    {
        if (_mover.IsBringingOre)
        {
            MovingStateChanged?.Invoke(false, _target.gameObject.transform);
            OreBrought?.Invoke();
            _isUsing = false;
            WorkingStateChanged?.Invoke();
        }
    }
}