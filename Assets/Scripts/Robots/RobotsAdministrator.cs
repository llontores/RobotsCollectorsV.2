using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class RobotsAdministrator : MonoBehaviour
{
    [SerializeField] private Transform _newRobotsSpawnpoint;
    [SerializeField] private Transform _oresReceiver;
    [SerializeField] private Robot _robotsPrefab;
    [SerializeField] private Robot[] _inputRobots;
    [SerializeField] private Transform _storage;
    [SerializeField] private OresCounter _oresCounter;
    [SerializeField] private CollectorsBase _collectorsBasePrefab;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private CollectorsBase _collectorsBase;

    private Queue<Ore> _ores = new Queue<Ore>();
    private List<Robot> _robots = new List<Robot>();

    public UnityAction BaseBuilt;

    public void InitializeSpawner(Spawner spawner)
    {
        _spawner = spawner;
        _spawner.OreSpawned += TryBringOre;
    }

    public void AddOre(Ore ore)
    {
        _ores.Enqueue(ore);
        TryBringOre();
    }

    public void TryBringOre()
    {
        Robot result = _robots.FirstOrDefault(robot => robot.IsUsing == false);
        if (result != null && _spawner.QueueCount > 0)
        {
            Ore currentOre = _spawner.DequeueOres();
            result.BringOre(currentOre);
        }
    }

    public void TryAddRobot()
    {
        Robot addedRobot = Instantiate(_robotsPrefab, _newRobotsSpawnpoint.position, Quaternion.identity);
        addedRobot.SetBase(_oresReceiver, _storage);
        addedRobot.WorkingStateChanged += TryBringOre;
        _robots.Add(addedRobot);
        _oresCounter.AddRobot(addedRobot);
        TryBringOre();
    }

    public void AddRobotToList(Robot robot)
    {
        _robots.Add(robot);
        robot.WorkingStateChanged += TryAskRobotToWork;
        TryAskRobotToWork();
    }
    
    public void ResetRobotsList()
    {
        _robots.Clear();
    }

    public void TryBuildBase(NewBaseFlag newBaseFlag)
    {
        Robot result = _robots.FirstOrDefault(robot => robot.IsUsing == false);

        if (result != null)
        {
            result.GoToNewBaseFlag(newBaseFlag, _collectorsBasePrefab);
            result.BuiltBase += RemoveRobotFromList;
            result.GetComponent<RobotCollisionHandler>().GetNewBaseFlag += InitializeNewBase;
            BaseBuilt?.Invoke();
        }

    }

    private void InitializeNewBase(Robot result)
    {
        result.BuildBase(_oresCounter.Shop, _spawner);
    }

    private void RemoveRobotFromList(Robot robot)
    {
        _robots.Remove(robot);
        robot.BuiltBase -= RemoveRobotFromList;
    }

    private void TryAskRobotToWork()
    {
        if (_oresCounter.NewBasePriority == true && _oresCounter.TryBuyNewBase())
        {
            TryBuildBase(_collectorsBase.NewBaseFlag);
        }
        else
        {
            TryBringOre();
        }
    }

    private void OnEnable()
    {
        if (_spawner != null)
            _spawner.OreSpawned += TryBringOre;

        for (int i = 0; i < _inputRobots.Length; i++)
        {
            _robots.Add(_inputRobots[i]);
            _robots[i].SetBase(_oresReceiver, _storage);
            _oresCounter.AddRobot(_robots[i]);
        }

        for (int i = 0; i < _robots.Count; i++)
        {
            _robots[i].WorkingStateChanged += TryAskRobotToWork;
        }
    }

    private void OnDisable()
    {
        if (_spawner != null)
            _spawner.OreSpawned -= TryBringOre;
        for (int i = 0; i < _robots.Count; i++)
        {
            _robots[i].WorkingStateChanged -= TryAskRobotToWork;
        }
    }
}