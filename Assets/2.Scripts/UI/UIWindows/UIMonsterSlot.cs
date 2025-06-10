using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineEnums;

public class UIMonsterSlot : MonoBehaviour
{
    [SerializeField] Image _monsterImage;
    [SerializeField] TextMeshProUGUI _monsterName;
    [SerializeField] TextMeshProUGUI _monsterLevel;

    int _slotNumber;
    UISummon _uiSummon;

    public void InitSlot(int slotNumber, UISummon uiSummon)
    {
        _slotNumber = slotNumber;
        _uiSummon = uiSummon;
    }

    public void SetMonsterSlot(Sprite monsterImage, MonsterType type, int level)
    {
        _monsterImage.sprite = monsterImage;
        _monsterName.text = type.ToString();
        _monsterLevel.text = level.ToString();
    }

    public void Button_MonsterSlot()
    {
        _uiSummon.SetMonsterIndex(_slotNumber);
        UIManager._Instance.CloseUIWindow();
    }
}
