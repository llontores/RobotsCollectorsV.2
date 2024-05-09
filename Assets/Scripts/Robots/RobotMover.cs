using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _inventoryPoint;
    [SerializeField] private RobotCollisionHandler _handler;
    [SerializeField] private float _getTargetDistance;

    private Vector3 _destination;
    private Transform _target;
    private Coroutine _moveOreJob;
    private Coroutine _moveJob;
    private Vector3 _storage;
    private Vector3 _startPosition;
    private Robot _robot;
    private void OnEnable()
    {
        _robot.OreBrought += PutOre;
        _robot.MovingStateChanged += ControlMoving;
    }

    private void OnDisable()
    {
        _robot.OreBrought -= PutOre;
        _robot.MovingStateChanged -= ControlMoving;

    }

    private void Awake()
    {
        _robot = GetComponent<Robot>();
    }

    public void SetTarget(Ore target)
    {
        _target = target.transform;
    }

    private void ControlMoving(bool isMoving, Transform target)
    {
        if (isMoving)
            _moveJob = StartCoroutine(Move(target));
        else if (isMoving == false)
            StopCoroutine(_moveJob);
    }

    private IEnumerator Move(Transform targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition.position) > _getTargetDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, _speed * Time.deltaTime);
            yield return null;
        }
    }

    public void PickUpOre()
    {
        _moveOreJob = StartCoroutine(MoveOre(_startPosition));
    }

    public void PutOre()
    {
        if (_moveOreJob != null)
        {
            StopCoroutine(_moveOreJob);
            _target.gameObject.SetActive(false);
        }

    }

    private IEnumerator MoveOre(Vector3 destination)
    {

        while (true)
        {
            _target.position = _inventoryPoint.position;

            yield return null;
        }
    }
}