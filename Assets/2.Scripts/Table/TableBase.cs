using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TableBase
{
    Dictionary<string, Dictionary<string, string>> _sheetData;

    protected TableBase()
    {
        _sheetData = new Dictionary<string, Dictionary<string, string>>();
    }

    public void Add(string index, string colum, string val)
    {
        if (!_sheetData.ContainsKey(index))
            _sheetData.Add(index, new Dictionary<string, string>());
        if (!_sheetData[index].ContainsKey(colum))
            _sheetData[index].Add(colum, val);
        else
            Debug.LogErrorFormat("[Index:{0}, Column:{1}, Val:{2}] 같은 자료가 _sheetData에 있습니다."
                , index, colum, val);
    }

    public abstract void Load(string jsonData);

    public string ToS(string index, string colum)
    {
        string findVal = string.Empty;
        if (_sheetData.ContainsKey(index))
            _sheetData[index].TryGetValue(colum, out findVal);

        return findVal;
    }

    public string ToS(int index, string colum)
    {
        return ToS(index.ToString(), colum);
    }

    public int ToI(string index, string colum)
    {
        return int.Parse(ToS(index, colum));
    }

    public int ToI(int index, string colum)
    {
        return int.Parse(ToS(index, colum));
    }

    public float ToF(string index, string colum)
    {
        float result;
        if (float.TryParse(ToS(index, colum), out result))
            return result;
        return default;
    }
    public float ToF(int index, string colum)
    {
        float result;
        if (float.TryParse(ToS(index, colum), out result))
            return result;
        return default;
    }


    public bool ToBool(string index, string column)
    {
        string findVal = ToS(index, column);
        bool val = false;
        bool check = false;
        if (findVal.CompareTo("True") == 0)
            check = val = true;
        else if (findVal.CompareTo("False") == 0)
        {
            check = true;
            val = false;
        }

        if (check)
            return val;
        else
        {
            Debug.Log("오류 : 테이블데이타가 \"True\" or \"False\" 형태가 아닙니다.");
            return false;
        }
    }

    public bool ToBool(int index, string column)
    {
        return ToBool(index.ToString(), column);
    }

}