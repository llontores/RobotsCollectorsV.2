using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotCollisionHandler : MonoBehaviour
{
    public event UnityAction GetOre;
    public event UnityAction GetBaseBack;
    public event UnityAction GetNewBaseFlag;
    private Ore _target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CollectorsBase collectorsBase))
        {
            GetBaseBack?.Invoke();
        }

        if(other.TryGetComponent(out NewBaseFlag newBaseFlag))
        {
            GetNewBaseFlag?.Invoke();
        }
    }

    public void SetTarget(Ore target)
    {
        _target = target;
    }
}