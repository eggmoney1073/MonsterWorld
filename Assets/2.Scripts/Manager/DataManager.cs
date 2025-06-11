using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DefineEnums;
using DefineStructs;

public class DataManager : SingletonDontDestroyOnLoad<DataManager>
{
    string _path;
    PlayerData _newData;

    Slot _currentSlot;
    PlayerData _currentData;

    Dictionary<Slot, PlayerData> _slotData;

    public Slot _CurrentSlot { get { return _currentSlot;  } set { _currentSlot = value; } }
    public PlayerData _CurrentData { get { return _currentData; } set { _currentData = value; } }

    protected override void Awake()
    {
        base.Awake();

        _path = Application.persistentDataPath + "/SaveData";
        if (!Directory.Exists(_path))
            Directory.CreateDirectory(_path);
        _path += "/";
        Debug.Log(_path);

        _newData = new PlayerData()
        {
            Level = 0,
            HP = 0,
            EXP = 0,
            Position = Vector3.zero,

            Monster_Type = new MonsterType[5],
            Monster_Level = new int[5]
        };

        for (int i = 0; i < 5; i++)
        {
            _newData.Monster_Type[i] = MonsterType.None;
            _newData.Monster_Level[i] = 0;
        }
    }


    //// Test Code
    //void Start()
    //{
    //    for (int i = 0; i < 3; i++)
    //    {
    //        string StrData = JsonUtility.ToJson(_newData);
    //        Slot slot = (Slot)i;
    //        File.WriteAllText(_path + slot.ToString(), StrData);
    //    }
    //}

    public PlayerData GetSlotData(Slot slot)
    {
        return _slotData[slot];
    }

    public void SetCurrentData(bool isNew)
    {
        if (isNew)
            _currentData = _newData;
        else
            _currentData = _slotData[_currentSlot];

    }

    public void LoadAllData()
    {
        _slotData = new Dictionary<Slot, PlayerData>();

        int slotCount = (int)Slot.Max;

        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = (Slot)i;
            string fileName = (slot).ToString();

            if (File.Exists(_path + fileName))
            {
                string JsonData = File.ReadAllText(_path + fileName);
                PlayerData data = JsonUtility.FromJson<PlayerData>(JsonData);
                _slotData.Add(slot, data);
            }
            else
                _slotData.Add(slot, _newData);
        }        
    }

    public void SaveData(PlayerData data)
    {
        Debug.Log("Saving game");
        Debug.Log(data.Level);
        string StrData = JsonUtility.ToJson(data);
        File.WriteAllText(_path + _currentSlot.ToString(), StrData);
    }

    public void DeleteCurrnetSlotData()
    {
        File.Delete(_path + _currentSlot.ToString());
    }
}
