using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineEnums;

public class UIEncyclopedia : UIWindowBase
{
    [SerializeField] GameObject _content;
    [SerializeField] TextMeshProUGUI _totalMonster;
    [SerializeField] TextMeshProUGUI _findMonster;

    bool _isChanged;
    int _totalMonsterCount;

    Dictionary<MonsterType, bool> _monsterOpen;
    Dictionary<MonsterType, UIEncyclopediaInfo> _monsterInfo;

    GameObject _encyclopedia;
    Button _buttonClear;


    public override void Init()
    {
        _isChanged = true;

        _monsterOpen = new Dictionary<MonsterType, bool>();
        _monsterInfo = new Dictionary<MonsterType, UIEncyclopediaInfo>();

        int monsterCount = (int)MonsterType.Max;
        for (int i = 1; i < monsterCount; i++)
        {
            _monsterOpen.Add((MonsterType)i, false);
            UIEncyclopediaInfo infoWindow = _content.transform.GetChild(monsterCount - i - 1).GetComponent<UIEncyclopediaInfo>();
            infoWindow.InitInfoWindow();
            _monsterInfo.Add((MonsterType)i, infoWindow);
        }

        _totalMonsterCount = monsterCount - 1;
        _totalMonster.text = _totalMonsterCount.ToString();

        SetEncyclopedia();
        _encyclopedia = transform.GetChild(0).gameObject;
        _buttonClear = _encyclopedia.transform.GetChild(1).GetComponent<Button>();
        _buttonClear.interactable = false;
        _encyclopedia.SetActive(false);
    }

    public void Button_Clear()
    {
        UIManager._Instance.ShowGameOverUI(GameOverUI.GameClear);
        InGameManager._Instance.PauseGame();
        DataManager._Instance.DeleteCurrnetSlotData();
    }

    public override void ShowUI()
    {
        _encyclopedia.SetActive(true);
        SetEncyclopedia();

        base.ShowUI();
    }

    public override void HideUI()
    {
        _encyclopedia.SetActive(false);
        base.HideUI();
    }

    public void GetNewMonster(MonsterType monster)
    {
        _monsterOpen[monster] = true;
        CheckClear();
    }

    void CheckClear()
    {
        int findMonster = 0;
        int monsterCount = (int)MonsterType.Max;

        for (int i = 1; i < monsterCount; i++)
        {
            if (_monsterOpen[(MonsterType)i])
                findMonster++;
        }

        if(findMonster == _totalMonsterCount)
        {
            _buttonClear.interactable = true;
        }
    }

    void SetEncyclopedia()
    {
        if (!_isChanged)
            return;

        int findMonster = 0;
        int monsterCount = (int)MonsterType.Max;
        for (int i = 1; i < monsterCount; i++)
        {
            MonsterType monster = (MonsterType)i;
            if (_monsterOpen[monster])
            {
                findMonster++;
                _monsterInfo[monster].OpenInfo();
            }
        }

        _findMonster.text = findMonster.ToString();
    }
}
