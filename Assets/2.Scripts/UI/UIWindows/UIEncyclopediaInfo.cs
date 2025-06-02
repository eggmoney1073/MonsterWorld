using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEncyclopediaInfo : MonoBehaviour
{
    GameObject _infoWindow;
    GameObject _notOpenWindow;

    public void InitInfoWindow()
    {
        _infoWindow = transform.GetChild(0).gameObject;
        _notOpenWindow = transform.GetChild(1).gameObject;

        CloseInfo();
    }

    public void OpenInfo()
    {
        _infoWindow.SetActive(true);
        _notOpenWindow.SetActive(false);
    }

    public void CloseInfo()
    {
        _infoWindow.SetActive(false);
        _notOpenWindow.SetActive(true);
    }
}
