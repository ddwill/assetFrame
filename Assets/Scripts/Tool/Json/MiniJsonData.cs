using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

/// <summary>
/// 保存body中的Dictionary或者List形式
/// </summary>
public class MiniJsonData
{
    public Dictionary<string, object> dictionaryData;
    public List<object> listData;
    public string strData = "";

    public object this[string key]
    {
        get
        {
            return dictionaryData[key];
        }
    }
    public bool hasKey(string key)
    {
        if (dictionaryData == null)
            return false;
        return dictionaryData.ContainsKey(key);
    }
    /// <summary>
    /// 返回字典的Count
    /// </summary>
    public int Count
    {
        get
        {
            if (dictionaryData != null)
                return dictionaryData.Count;
            return 0;
        }
    }
    /// <summary>
    /// 返回字典的Keys
    /// </summary>
    public Dictionary<string, object>.KeyCollection Keys
    {
        get
        {
            if (dictionaryData != null)
                return dictionaryData.Keys;
            return null;
        }
    }
    /// <summary>
    /// 比较字典的key是否存在
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Contains(string key)
    {
        if (dictionaryData != null)
            return dictionaryData.ContainsKey(key);
        return false;
    }

    /// <summary>
    /// 是否是列表数据
    /// </summary>
    public bool IsArray
    {
        get
        {
            if (listData != null && listData.Count > 0)
                return true;
            return false;
        }
    }

    /// <summary>
    /// 是否是字符串数据
    /// </summary>
    public bool IsString
    {
        get
        {
            return strData.Length > 0;
        }
    }

    /// <summary>
    /// 是否为空
    /// </summary>
    public bool IsNull = false;

    public MiniJsonData(object jsondata)
    {
        if (jsondata is Dictionary<string, object>)
        {
            dictionaryData = jsondata as Dictionary<string, object>;
        }
        else if (jsondata is List<object>)
        {
            listData = jsondata as List<object>;
        }
        else
        {
            if (jsondata != null)
            {
                strData = jsondata.ToString();
                if (strData == "")
                    IsNull = true;
            }
            else
                IsNull = true;
        }
    }

    public override string ToString()
    {
        if (IsString)
            return strData;
        if (IsNull)
            return "null";
        if (IsArray)
        {
            string str = "";
            foreach(object obj in listData)
            {
                if (str.Length > 0)
                    str += ",";
                else
                    str += "[";
                str += obj.ToString();
            }
            str += "]";
            return str;
        }
        else if (dictionaryData != null)
        {
            string str = "";
            foreach (string key in dictionaryData.Keys)
            {
                if (str.Length > 0)
                    str += ",";
                str += key + "=" + dictionaryData[key];
            }
            return str;
        }
        return base.ToString();
    }
}
