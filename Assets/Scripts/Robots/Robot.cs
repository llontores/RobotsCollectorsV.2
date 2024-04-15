using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RobotMover))]
public class Robot : MonoBehaviour
{
    [SerializeField] private RobotCollisionHandler _handler;
    public bool IsUsing => _isUsing;
    public event UnityAction<Ore> OreBrought;
    public event UnityAction<bool, Transform> MovingStateChanged;
    public event UnityAction WorkingStateChanged;

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
        _mover.SetParametres(_target.gameObject.transform, _storage.position, _oresReceiver.position);
        MovingStateChanged?.Invoke(true, target.gameObject.transform);
        //_moveJob = StartCoroutine(Move(_target.gameObject.transform));
        _isUsing = true;
        _handler.SetTarget(target);
    }

    public void SetBase(Transform receiver, Transform storage)
    {
        _oresReceiver = receiver;
        _storage = storage;
    }


    //private IEnumerator Move(Transform targetPosition)
    //{
    //    while(Vector3.Distance(transform.position,targetPosition.position) > _getTargetDistance)
    //    {
    //        _mover.Move(targetPosition.position);

    //        yield return null;
    //    }
    //}

    private void GoBack()
    {
        //EndMoveJob();
        MovingStateChanged?.Invoke(false, _oresReceiver);
        _mover.PickUpOre();
        MovingStateChanged?.Invoke(true, _oresReceiver);
        //_moveJob = StartCoroutine(Move(_oresReceiver));
    }

    private void GetBase()
    {
        if (IsUsing)
        {
            MovingStateChanged?.Invoke(false, _target.gameObject.transform);
            OreBrought?.Invoke(_target);
            _isUsing = false;
            WorkingStateChanged?.Invoke();
        }
    }
}