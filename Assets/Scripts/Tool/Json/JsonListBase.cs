using LitJson;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Json序列化列表实体基类
/// </summary>
/// <typeparam name="T">别序列化的实体</typeparam>
public class JsonListBase<T> where T : IJsonSerialize, new()
{
    public List<T> dataList;

    /// <summary>
    /// 列表长度
    /// </summary>
    public int Count
    {
        get
        {
            if (dataList == null)
                return 0;
            return dataList.Count;
        }
    }

    ///// <summary>
    ///// 序列化
    ///// </summary>
    ///// <param name="jsondata"></param>
    //public void Deserialize(JsonData jsondata)
    //{
    //    dataList = JsonListBase<T>.Deserializes(jsondata);
    //}

    ///// <summary>
    ///// 反序列化
    ///// </summary>
    ///// <returns></returns>
    //public JsonData Serialize()
    //{
    //    return JsonListBase<T>.Serializes(dataList);
    //}

    ///// <summary>
    ///// 序列化列表数据
    ///// </summary>
    ///// <param name="jsondata"></param>
    ///// <returns></returns>
    //public static List<T> Deserializes(JsonData jsondata)
    //{
    //    List<T> dataList = null;
    //    if (jsondata!=null && jsondata.IsArray)
    //    {
    //        dataList = new List<T>();
    //        for (int i = 0; i < jsondata.Count; i++)
    //        {
    //            JsonData jsonData = jsondata[i];
    //            T data = new T();
    //            data.Deserialize(jsonData);
    //            dataList.Add(data);
    //        }
    //    }
    //    return dataList;
    //}

    ///// <summary>
    ///// 反序列化列表数据
    ///// </summary>
    ///// <param name="dataList"></param>
    ///// <returns></returns>
    //public static JsonData Serializes(List<T> dataList)
    //{
    //    JsonData jsondata = null;
    //    if (dataList != null)
    //    {
    //        jsondata = new JsonData();
    //        foreach (T data in dataList)
    //        {
    //            jsondata.Add(data.Serialize());
    //        }
    //    }
    //    return jsondata;
    //}

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="jsondata"></param>
    public virtual void Deserialize(List<object> jsondata)
    {
        dataList = JsonListBase<T>.Deserializes(jsondata);
    }

    /// <summary>
    /// 按位置获得对象
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public virtual T this[int index]
    {
        get {return dataList[index];}
    }


    /// <summary>
    /// 序列化列表数据
    /// </summary>
    /// <param name="jsondata"></param>
    /// <returns></returns>
    public static List<T> Deserializes(List<object> jsondata)
    {
        List<T> dataList = new List<T>();
        if (jsondata != null && jsondata.Count > 0)
        {
            jsondata.ForEach(delegate(object jsonData)
            {
                T data = new T();
                data.Deserialize(new MiniJsonData(jsonData));
                dataList.Add(data);
            });
        }
        return dataList;
    }
}