using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorsBase : MonoBehaviour
{
    [SerializeField] private RobotsAdministrator _administrator;

    public Vector3 NewBaseFlag => _newBaseFlag;
    public bool NewBasePriority => _newBasePriority;

    private bool _newBasePriority = false;
    private Vector3 _newBaseFlag;

    public void SetNewBaseFlag(Vector3 flagPosition)
    {
        _newBaseFlag = flagPosition;
        TryBuildNewBase();
    }

    private void TryBuildNewBase()
    {
        if (_newBaseFlag != null)
            _newBasePriority = true;
    }

}