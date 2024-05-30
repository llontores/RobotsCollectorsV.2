﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorsBase : MonoBehaviour
{
    [SerializeField] private RobotsAdministrator _administrator;
    [SerializeField] private OresCounter _oresCounter;

    public Transform NewBaseFlag => _newBaseFlag;
    public bool NewBasePriority => _newBasePriority;

    private bool _newBasePriority = false;
    private Transform _newBaseFlag;

    private void Start()
    {
        _oresCounter = GetComponent<OresCounter>();
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

    public bool TryBuildNewBase()
    {
        if (_newBaseFlag != null)
            _newBasePriority = true;
        _oresCounter.NewBasePriorityChange();
        return _administrator.TryBuildBase(_newBaseFlag);
    }

    public void InitializeComponents(Shop shop, Spawner spawner)
    {
        print("ЙОУ МЕНЯ ТУТ ИНИЦИАЛИЗИРУЮТ ААА");
        _oresCounter.InitializeShop(shop);
        _administrator.InitializeSpawner(spawner);
    }
}