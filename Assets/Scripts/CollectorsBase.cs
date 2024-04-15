using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorsBase : MonoBehaviour
{
    //[SerializeField] private OresCounter _counter;
    [SerializeField] private RobotsAdministrator _administrator;
    [SerializeField] private int _newBasePrice;


    private Transform _newBaseFlag;
    private int _oresAmount;
    private void OnEnable()
    {
        //_counter.OreCollected += CountOres;
    }

    private void OnDisable()
    {
        //_counter.OreCollected -= CountOres;
    }

    public void SetNewBaseFlag(Transform flagPosition)
    {
        _newBaseFlag.position = new Vector3(flagPosition.position.x, transform.position.y, flagPosition.position.z);
        print(_newBaseFlag.position);
        TryBuildNewBase();
    }

    private void CountOres(int oresAmount)
    {
        _oresAmount = oresAmount;
        //TryBuildNewBase();
    }

    private void TryBuildNewBase()
    {
        if (_newBaseFlag != null)
            if (_oresAmount >= _newBasePrice)
                print("������ � �������� ������ ����� �� �������� ����� ���� �� " + _newBaseFlag.position);
    }
}