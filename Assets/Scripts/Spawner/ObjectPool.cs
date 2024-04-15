using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private int _amount;

    private List<GameObject> _pool = new List<GameObject>();

    protected void Initialize(GameObject[] prefabs)
    {

        for (int i = 0; i < _amount; i++)
        {
            GameObject spawned = Instantiate(prefabs[Random.Range(0, prefabs.Length)], _container);
            spawned.SetActive(false);
            _pool.Add(spawned);
        }
    }

    protected bool TryGetObject(out GameObject result)
    {
        result = _pool.FirstOrDefault(p => p.activeSelf == false);

        return result != null;
    }
}