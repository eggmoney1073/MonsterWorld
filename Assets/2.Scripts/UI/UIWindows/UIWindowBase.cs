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
        UIManager._Instance.CloseAllWindow();
    }

    public virtual void ShowUI()
    {
        _isOpened = true;
    }

    public virtual void HideUI()
    {
        _isOpened = false;
    }
}
