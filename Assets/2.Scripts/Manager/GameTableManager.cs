using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class GameTableManager : SingletonGameobject<GameTableManager>
{
    Dictionary<TableName, TableBase> _tableList;
    protected override void Awake()
    {
        base.Awake();
        _tableList = new Dictionary<TableName, TableBase>();
    }

    bool Load<T>(TableName name) where T : TableBase, new()
    {
        if (_tableList.ContainsKey(name))
            return true;

        string path = "Tables/";
        TextAsset tAsset = Resources.Load(path + name) as TextAsset;
        if (tAsset != null)
        {
            T t = new T();
            t.Load(tAsset.text);
            _tableList.Add(name, t);
        }
        else
            return false;

        return true;
    }

    public void AllLoadTables()
    {
        if (!Load<MonsterLevelTable>(TableName.MonsterLevelTable))
            Debug.Log("Fail MonsterLevelTable Load, Check Path and Name");

        if (!Load<PlayerLevelTable>(TableName.PlayerLevelTable))
            Debug.Log("Fail PlayerLevelTable Load, Check Path and Name");

        if (!Load<MonsterTable>(TableName.MonsterTable))
            Debug.Log("Fail MonsterTable Load, Check Path and Name");

        if (!Load<SkillTable>(TableName.SkillTable))
            Debug.Log("Fail SkillTable Load, Check Path and Name");

        if (!Load<PlayerWeaponTable>(TableName.PlayerWeaponTable))
            Debug.Log("Fail PlayerWeaponTable Load, Check Path and Name");
    }

    public TableBase Get(TableName name)
    {
        if (_tableList.ContainsKey(name))
            return _tableList[name];

        return null;
    }
}
