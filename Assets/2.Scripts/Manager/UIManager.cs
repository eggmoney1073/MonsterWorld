using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineEnums;

public class UIManager : SingletonGameobject<UIManager>
{
    UIMonster _monsterUI;
    UIPlayer _playerUI;
    UIWeapon _weaponUI;
    CrossHair _crossHair;

    UIWindowBase _openUIWindow;

    bool _isPause;
    bool _isGameOver;

    Dictionary<InGameUI, UIWindowBase> _ingameUIWindows;
    Dictionary<GameOverUI, UIWindowBase> _gameOverUIWindows;

    public UIPlayer _PlayerUI { get { return _playerUI; } }
    public bool _GameOver { get { return _isGameOver; } set { _isGameOver = value; } }
    public bool _IsPause { get { return _isPause; } set { _isPause = value; } }

    public void ShowIngameUI(InGameUI ui)
    {
        _openUIWindow = _ingameUIWindows[ui];
        _openUIWindow.ShowUI();

        InGameManager._Instance.PauseGame();
        _isPause = true;
    }

    public void CloseUIWindow()
    {
        _openUIWindow.HideUI();
        InGameManager._Instance.ResumeGame();
        _isPause = false;

        _openUIWindow = null;
    }

    public void ShowGameOverUI(GameOverUI ui)
    {
        _gameOverUIWindows[ui].ShowUI();

        _isGameOver = true;
    }

    public void Respawn()
    {
        _isGameOver = false;
        _gameOverUIWindows[GameOverUI.GameOver].HideUI();
    }

    public void ChangeWeapon(PlayerWeapon weapon)
    {
        _weaponUI.SetWeapon(weapon);
    }

    public void ChangeMonster(MonsterType type)
    {
        _monsterUI.SetUIMonster(type);
    }

    public void GetNewMonster(MonsterType monster)
    {
        if (monster == MonsterType.None)
            return;
        

        UIEncyclopedia encyclopedia = (UIEncyclopedia)_ingameUIWindows[InGameUI.Encyclopedia];
        encyclopedia.GetNewMonster(monster);
    }

    public void Init()
    {
        _isGameOver = false;
        _isPause = false;

        _playerUI = transform.GetChild(0).GetComponent<UIPlayer>();
        _monsterUI = transform.GetChild(1).GetComponent<UIMonster>();
        _weaponUI = transform.GetChild(2).GetComponent<UIWeapon>();
        _crossHair = transform.GetChild(3).GetComponent<CrossHair>();

        _ingameUIWindows = new Dictionary<InGameUI, UIWindowBase>();
        _gameOverUIWindows = new Dictionary<GameOverUI, UIWindowBase>();

        string path = "UI/InGameUI/";
        GameObject prefab;
        UIWindowBase uiWindow;

        int uiCount = (int)InGameUI.Max;

        for (int i = 0; i < uiCount; i++)
        {
            InGameUI ui = (InGameUI)i;
            prefab = Resources.Load(path + ui.ToString() + "Canvas") as GameObject;
            uiWindow = Instantiate(prefab).GetComponent<UIWindowBase>();
            uiWindow.Init();
            _ingameUIWindows.Add(ui, uiWindow);
        }

        uiCount = (int)GameOverUI.Max;

        for (int i = 0; i < uiCount; i++)
        {
            GameOverUI ui = (GameOverUI)i;
            prefab = Resources.Load(path + ui.ToString() + "Canvas") as GameObject;
            uiWindow = Instantiate(prefab).GetComponent<UIWindowBase>();
            uiWindow.Init();
            _gameOverUIWindows.Add(ui, uiWindow);
        }
    }

    public void LateInit()
    {
        _monsterUI.InitMonsterUI();
        _weaponUI.InitUIWeapon();
    }

    void UIControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPause)
                CloseUIWindow();
            else
                ShowIngameUI(InGameUI.Pause);
        }

        if (!_isPause)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ShowIngameUI(InGameUI.Summon);
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ShowIngameUI(InGameUI.Encyclopedia);
            }
        }
    }

    void Update()
    {
        if (_isGameOver)
            return;

        UIControl();
    }
}
