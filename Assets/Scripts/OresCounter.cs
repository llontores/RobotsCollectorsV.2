using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OresCounter : MonoBehaviour
{
    [SerializeField] private Shop _shop;
    [SerializeField] private RobotsAdministrator _robotsAdministrator;
    private int _counter = 0;
    private List<Robot> _robots = new List<Robot>();
    public void AddRobot(Robot newRobot)
    {
        _robots.Add(newRobot);
        newRobot.OreBrought += IncreaseOres;
    }

    public void IncreaseOres(Ore ore)
    {
        _counter++;
        print(_counter);
        TryBuyNewRobot();
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
}