using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPause : UIWindowBase
{
    GameObject _pauseWindow;

    public override void Init()
    {
        _pauseWindow = transform.GetChild(0).gameObject;

        HideUI();
    }

    public void Button_MainMenu()
    {
        InGameManager._Instance.SaveGame();
        InGameManager._Instance.GoToTitle();
    }

    public override void ShowUI()
    {
        base.ShowUI();
        _pauseWindow.SetActive(true);
    }

    public override void HideUI()
    {
        base.HideUI();
        _pauseWindow.SetActive(false);
    }
}
