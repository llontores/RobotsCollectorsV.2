using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private int _robotPrice;

    public int RobotPrice => _robotPrice;

    private Vector3 _newBaseFlagPosition;
    private int _clicksCounter = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            CollectorsBase collectorsBase;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (_clicksCounter == 0)
                {
                    //print(hit.collider.gameObject.name);
                    collectorsBase = hit.collider.GetComponent<CollectorsBase>();

                    if (collectorsBase != null)
                    {
                        _clicksCounter++;
                        print("� ������� �� ����" + collectorsBase.gameObject.name);
                    }
                    else
                        _clicksCounter = 0;
                }
                else if (_clicksCounter == 1)
                {
                    Vector3 mousePosition = Input.mousePosition;

                    _newBaseFlagPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x,
                    mousePosition.y, Camera.main.nearClipPlane));
                    //collectorsBase.SetNewBaseFlag(_newBaseFlag);
                    _clicksCounter = 0;
                    print(_newBaseFlagPosition);
                }
            }

        }
    }
}