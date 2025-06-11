using DefineEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISummon : UIWindowBase
{
    [SerializeField] Sprite[] _monsterSprites;
    [SerializeField] GameObject _informationGO;

    UIMonsterSlot[] _monsterSlots;
    GameObject _summonWindow;

    bool _isSelectState;

    int _monsterIndex;

    public int _MonsterIndex { get { return _monsterIndex; } set { _monsterIndex = value; } }
    public bool _IsSelectState { get { return _isSelectState; } set { _isSelectState = value; } }

    public override void Init()
    {
        _summonWindow = transform.GetChild(0).gameObject;
        _monsterSlots = new UIMonsterSlot[5];
        for (int i = 0; i < 5; i++)
        {
            UIMonsterSlot slot = _summonWindow.transform.GetChild(i + 1).GetComponent<UIMonsterSlot>();
            slot.InitSlot(i, this);
            _monsterSlots[i] = slot;
        }
        HideUI();
    }

    public override void ShowUI()
    {
        base.ShowUI();
        _summonWindow.SetActive(true);
        _informationGO.SetActive(_isSelectState);

        for (int i = 0; i < 5; i++)
        {
            int level = PlayerManager._Instance.GetPlayerMonsterLevel(i);
            MonsterType type = PlayerManager._Instance.GetPlayerMonsterType(i);
            Sprite image = _monsterSprites[(int)type];

            _monsterSlots[i]._IsSelectState = _isSelectState;
            _monsterSlots[i].SetMonsterSlot(image, type, level);
        }
    }

    public override void HideUI()
    {
        base.HideUI();
        _summonWindow.SetActive(false);
    }
}
