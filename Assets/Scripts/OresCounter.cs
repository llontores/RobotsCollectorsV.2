using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OresCounter : MonoBehaviour
{
    [SerializeField] private Shop _shop;
    [SerializeField] private RobotsAdministrator _robotsAdministrator;
    [SerializeField] private CollectorsBase _collectorsBase;

    private int _counter = 0;
    private List<Robot> _robots = new List<Robot>();
    private bool _newBasePriority = false;
    public void AddRobot(Robot newRobot)
    {
        _robots.Add(newRobot);
        newRobot.OreBrought += IncreaseOres;
    }

    public void IncreaseOres(Ore ore)
    {
        _counter++;

        if (_newBasePriority == true)
            TryBuildBase();

        else
            TryBuyNewRobot();

    }

    private void TryBuildBase()
    {
        if (_counter >= _shop.NewBasePrice)
        {
            //_robotsAdministrator.TryBuildBase();
            _counter -= _shop.NewBasePrice;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _robots.Count; i++)
        {
            _robots[i].OreBrought -= IncreaseOres;
        }
    }

    private void TryBuyNewRobot()
    {
        if (_counter >= _shop.RobotPrice)
        {
            _robotsAdministrator.TryAddRobot();
            _counter -= _shop.RobotPrice;
        }
    }

    private void NewBasePriorityChange()
    {
        _newBasePriority = true;
    }
}