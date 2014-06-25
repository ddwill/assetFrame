using System;
using System.Collections.Generic;
using LitJson;

/// <summary>
/// 服务器推送更新消息
/// </summary>
public class NetUpdateData
{
    /// <summary>
    /// 更新字段名称
    /// </summary>
    public string UpdateKey;
    /// <summary>
    /// 更新内容
    /// </summary>
    //public JsonData UpdateData;
    public object UpdateData;

    public NetUpdateData(string key, object data)
    {
        UpdateKey = key;
        UpdateData = data;
    }
}
