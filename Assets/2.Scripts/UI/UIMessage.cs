using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMessage : MonoBehaviour
{
    bool _isShow;
    float _checkTime;
    GameObject _windowGO;

    void Awake()
    {
        _windowGO = transform.GetChild(0).gameObject;
        _checkTime = 0;
        _windowGO.SetActive(false);
    }

    void Update()
    {
        if(_isShow)
        {
            _checkTime += Time.deltaTime;
            if (_checkTime > 3)
            {
                _checkTime = 0;
                _windowGO.SetActive(false);
            }
        }

    }

    public void ShowWindow()
    {
        _isShow = true;
        _checkTime = 0;
        _windowGO.SetActive(true);
    }
}
