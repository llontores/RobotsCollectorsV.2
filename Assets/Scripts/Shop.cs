using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Shop : MonoBehaviour
{
    [SerializeField] private int _robotPrice;
    [SerializeField] private int _newBasePrice;
    [SerializeField] private NewBaseFlag _newBaseFlag;

    public int RobotPrice => _robotPrice;
    public int NewBasePrice => _newBasePrice;

    private int _clicksCounter = 0;
    private CollectorsBase _collectorsBase;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (_clicksCounter == 0)
                {
                    _collectorsBase = hit.collider.GetComponent<CollectorsBase>();

                    if (_collectorsBase != null)
                        _clicksCounter++;

                    else
                        _clicksCounter = 0;
                }
                else if (_clicksCounter == 1)
                {
                    if (hit.collider.TryGetComponent<CollectorsBase>(out CollectorsBase collectorsBase) == false)
                    {
                        Vector3 mousePosition = Input.mousePosition;
                        print(mousePosition);
                        _newBaseFlag.gameObject.transform.position = hit.point;
                        _collectorsBase.SetNewBaseFlag(_newBaseFlag.gameObject.transform);
                        _clicksCounter = 0;
                        if (_newBaseFlag.gameObject.activeSelf == false)
                            _newBaseFlag.gameObject.SetActive(true);
                    }

                }

            }

        }

    }
}