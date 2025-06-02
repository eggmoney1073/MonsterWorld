using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOver : UIWindowBase
{
    public override void Init()
    {
        HideUI();
    }

    public override void Button_Close()
    {
        base.Button_Close();
    }

    public void Button_Exit()
    {
        InGameManager._Instance.SaveGame();
        InGameManager._Instance.GoToTitle();
    }

    public void Button_Respawn()
    {
        base.Button_Close();
        PlayerManager._Instance.Respawn();
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
