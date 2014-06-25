using System;
using LitJson;
using System.Collections.Generic;

/// <summary>
/// Json序列化接口
/// </summary>
public interface IJsonSerialize
{
    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="json"></param>
    //void Deserialize(JsonData jsondata);
    void Deserialize(MiniJsonData jsondata);
    /// <summary>
    /// 序列化
    /// </summary>
    /// <returns></returns>
    //JsonData Serialize();
}

