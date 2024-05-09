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
    public bool IsBringingOre => _isBringingOre;
    public event UnityAction OreBrought;
    public event UnityAction<bool, Transform> MovingStateChanged;
    public event UnityAction WorkingStateChanged;

    private bool _isBringingOre;
    private Transform _storage;
    private bool _isUsing;
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

    public void BuildBase(Transform newBaseFlag)
    {
        MovingStateChanged?.Invoke(true, newBaseFlag.gameObject.transform);
        _isUsing = true;
    }

    private void GoBack()
    {
        MovingStateChanged?.Invoke(false, _oresReceiver);
        _mover.PickUpOre();
        MovingStateChanged?.Invoke(true, _oresReceiver);
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