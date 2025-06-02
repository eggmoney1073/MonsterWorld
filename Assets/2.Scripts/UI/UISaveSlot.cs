using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineEnums;
using DefineStructs;

public class UISaveSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _level;
    [SerializeField] TextMeshProUGUI[] _monster;

    bool _isNew;
    bool _isSlotEmpty;
    Slot _slot;
    PlayerData _data;
    GameObject _emptySlot;

    public void Button_SlotClick()
    {
        if (!_isNew && _isSlotEmpty)
        {
            TitleManager._Instance.ClickEmptySlot();
        }
        else
        {
            DataManager._Instance._CurrentSlot = _slot;
            DataManager._Instance.SetCurrentData(_isNew);
            LoadingManager._Instance.LoadSceneAsync(SceneState.Game);
        }
    }

    public void SetSlot(Slot slot, bool isNew = false)
    {
        _emptySlot = transform.GetChild(3).gameObject;

        _isNew = isNew;
        _slot = slot;
        _data = DataManager._Instance.GetSlotData(slot);
        _isSlotEmpty = _data.Level == 0;

        _level.text = _data.Level.ToString();

        for (int i = 0; i < 5; i++)
            _monster[i].text = _data.Monster_Type[i].ToString();

        _emptySlot.SetActive(_isSlotEmpty);
    }
}
