using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorsBase : MonoBehaviour
{
    [SerializeField] private RobotsAdministrator _administrator;

    public Transform NewBaseFlag => _newBaseFlag;
    public bool NewBasePriority => _newBasePriority;

    private bool _newBasePriority = false;
    private Transform _newBaseFlag;

    public void SetNewBaseFlag(Transform flag)
    {
        _newBaseFlag = flag;
        TryBuildNewBase();
    }

    private void TryBuildNewBase()
    {
        if (_newBaseFlag != null)
            _newBasePriority = true;
    }

}