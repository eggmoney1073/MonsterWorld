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

    bool _isSelectState;
    int _slotNumber;
    UISummon _uiSummon;

    public bool _IsSelectState {  get { return _isSelectState; } set { _isSelectState = value; } }

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
        if (_isSelectState)
        {
            PlayerManager._Instance.SetCapturedMonster(_slotNumber);
            UIManager._Instance.CloseUIWindow();
        }
        else
        {
            _uiSummon._MonsterIndex = _slotNumber;
            PlayerManager._Instance.SetCurrentMonster(_slotNumber);
            UIManager._Instance.CloseUIWindow();
        }
    }
}
