using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorsBase : MonoBehaviour
{
    [SerializeField] private RobotsAdministrator _administrator;

    public Transform NewBaseFlag => _newBaseFlag;
    public bool NewBasePriority => _newBasePriority;

    private Spawner _spawner;
    private bool _newBasePriority = false;
    private Transform _newBaseFlag;

    private void OnEnable()
    {
        _spawner.OreSpawned += _administrator.TryBringOre;
    }

    private void OnDisable()
    {
        _spawner.OreSpawned -= _administrator.TryBringOre;
    }

    public void SetNewBaseFlag(Transform flag)
    {
        _newBaseFlag = flag;
        TryBuildNewBase();
    }

    public void AddNewRobot(Robot robot)
    {
        _administrator.AddRobotToList(robot);
    }

    private void TryBuildNewBase()
    {
        if (_newBaseFlag != null)
            _newBasePriority = true;
        _administrator.TryBuildBase(_newBaseFlag);
    }

}