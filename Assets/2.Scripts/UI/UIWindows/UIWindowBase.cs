using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowBase : MonoBehaviour
{
    bool _isOpened;

    public bool _isOpen { get { return _isOpened; } }

    public virtual void Init()
    {

    }

    public virtual void Button_Close()
    {
        HideUI();
    }

    public virtual void ShowUI()
    {
        _isOpened = true;
        InGameManager._Instance.PauseGame();
        InGameManager._Instance.CursorVisibleControl(true);
        UIManager._Instance._IsPause = _isOpen;
    }

    public virtual void HideUI()
    {
        _isOpened = false;
        InGameManager._Instance.ResumeGame();
        InGameManager._Instance.CursorVisibleControl(false);
        UIManager._Instance._IsPause = _isOpen;
    }
}
