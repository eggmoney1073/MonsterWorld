using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameClear : UIWindowBase
{
    public override void Init()
    {
        HideUI();
    }

    public void Button_Exit()
    {
        InGameManager._Instance.SaveGame();
        InGameManager._Instance.GoToTitle();
    }

    public override void ShowUI()
    {
        gameObject.SetActive(true);
        base.ShowUI();
    }

    public override void HideUI()
    {
        gameObject.SetActive(false);
        base.HideUI();
    }
}
