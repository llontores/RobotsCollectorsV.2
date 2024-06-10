using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorsBase : MonoBehaviour
{
    [SerializeField] private RobotsAdministrator _administrator;
    [SerializeField] private OresCounter _oresCounter;

    public NewBaseFlag NewBaseFlag => _newBaseFlag;
    private NewBaseFlag _newBaseFlag;


    public void SetNewBaseFlag(NewBaseFlag flag)
    {
        _newBaseFlag = flag;
        _oresCounter.SetNewBasePriority();
        if (_oresCounter.TryBuyNewBase())
            _administrator.TryBuildBase(_newBaseFlag);
        
    }

    public void AddNewRobot(Robot robot)
    {
        _administrator.AddRobotToList(robot);
    }

    public void InitializeComponents(Shop shop, Spawner spawner)
    {
        _oresCounter.InitializeShop(shop);
        _administrator.InitializeSpawner(spawner);
    }

    private void Start()
    {
        _oresCounter = GetComponent<OresCounter>();
    }
}