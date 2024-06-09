using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OresCounter : MonoBehaviour
{
    [SerializeField] private RobotsAdministrator _robotsAdministrator;
    [SerializeField] private Shop _shop;

    public bool NewBasePriority => _newBasePriority;
    public Shop Shop => _shop;

    private int _counter = 0;
    private List<Robot> _robots = new List<Robot>();
    private bool _newBasePriority = false;
    public void AddRobot(Robot newRobot)
    {
        _robots.Add(newRobot);
        newRobot.OreBrought += IncreaseOres;
    }

    private void OnEnable()
    {
        _robotsAdministrator.BaseBuilt += UnsetNewBasePriority;
    }


    private void IncreaseOres()
    {
        _counter++;
        print(_counter + gameObject.name);
        if (_newBasePriority == false)
        {
            TryBuyNewRobot();
        }
    }

    public bool TryBuyNewBase()
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

        _robotsAdministrator.BaseBuilt -= UnsetNewBasePriority;
    }

    private void TryBuyNewRobot()
    {
        if (_counter >= _shop.RobotPrice)
        {
            _robotsAdministrator.TryAddRobot();
            _counter -= _shop.RobotPrice;
        }
    }

    public void SetNewBasePriority()
    {
        _newBasePriority = true;
        print($"йоу я тут хочу пострить базу {gameObject.name}");
    }

    private void UnsetNewBasePriority()
    {
        _newBasePriority = false;
    }

    public void InitializeShop(Shop shop)
    {
        _shop = shop;
    }
}