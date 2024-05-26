using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OresCounter : MonoBehaviour
{
    [SerializeField] private RobotsAdministrator _robotsAdministrator;
    [SerializeField] private Shop _shop;

    private int _counter = 0;
    private List<Robot> _robots = new List<Robot>();
    private bool _newBasePriority = false;
    public void AddRobot(Robot newRobot)
    {
        _robots.Add(newRobot);
        newRobot.OreBrought += IncreaseOres;
    }

    public void IncreaseOres()
    {
        _counter++;

        if (_newBasePriority == false)
        {
            TryBuyNewRobot();
        }
    }

    public bool BuyNewBase()
    {
        if (_counter >= _shop.NewBasePrice)
        {
            _counter -= _shop.NewBasePrice;
            return true;
        }
        return false;

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

    public void NewBasePriorityChange()
    {
        _newBasePriority = true;
    }
}