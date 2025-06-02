using DefineEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISummon : UIWindowBase
{
    [SerializeField] Sprite[] _monsterSprites;
    UIMonsterSlot[] _monsterSlots;
    GameObject _summonWindow;

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

    public void SetMonsterIndex(int index)
    {
        PlayerManager._Instance.ChangeMonster(index);
    }

    public override void ShowUI()
    {
        _summonWindow.SetActive(true);

        for (int i = 0; i < 5; i++)
        {
            int level = PlayerManager._Instance.GetPlayerMonsterLevel(i);
            MonsterType type = PlayerManager._Instance.GetPlayerMonsterType(i);
            Sprite image = _monsterSprites[(int)type];

            _monsterSlots[i].SetMonsterSlot(image, type, level);
        }
    }

    public override void HideUI()
    {
        _summonWindow.SetActive(false);
    }
}
