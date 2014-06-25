using System;
using LitJson;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 发送数据
/// </summary>
public class NetSender
{

    private const string PAK_HEADER = "msg.";
    private const string PAK_TAIL = ".end";
    private const string AMF3_HAND_HEADER = "amf3:connect.";
    private const string JSON_HAND_HEADER = "json:connect.";

    /// <summary>
    /// 消息
    /// </summary>
    private string message;

    /// <summary>
    /// 是否是url格式
    /// </summary>
    private bool _isUrl = true;

    public NetSender()
    {
        message = "";
        _isUrl = true;
    }

    /// <summary>
    /// 全参数构造
    /// </summary>
    /// <param name="isUrl">是否是url格式</param>
    public NetSender(bool isUrl)
    {
        message = "";
        _isUrl = isUrl;
        if(!_isUrl)
            message = "noDecoder";
    }

    /// <summary>
    /// 添加键值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddMessage(string key, object value)
    {
        if (message.Length != 0)
            message += "&";
        message += key + "=";
        if (value is string)
        {
            string msg = GetString(value.ToString());
            if (msg.Length < 32766 && _isUrl)
                message += Uri.EscapeDataString(msg);
            else
                message += msg;
        }
        else
            message += value;
    }

#region trace使用

    /// <summary>
    /// 添加键值扩展,如果是字符串,自动加上"",为了和普通的不冲突,=自动使用＝
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddMessageEx(string key, object value)
    {
        if (message.Length != 0)
            message += "＆";
        message += key + "＝";
        if (value is ArrayList)
        {
            message += "[";
            string valuestr = "";
            foreach (object data in value as ArrayList)
            {
                if (valuestr.Length > 0)
                    valuestr += ",";
                valuestr += ((data is string) ? "\"" + data.ToString() + "\"" : data);
            }
            message += valuestr + "]";
        }
        else
        {
            message += ((value is string) ? "\"" + value.ToString() + "\"" : value);
        }
    }

    public string GetString(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "null";
        return value;
    }


    /// <summary>
    /// 获取json格式字符串
    /// </summary>
    /// <returns></returns>
    public string ToJson()
    {
        string json = "";
        string[] datas = message.Split('＆');
        json += "{";
        for (int i = 0; i < datas.Length; i++)
        {
            if (json.Length > 1)
                json += ",";
            string[] data = datas[i].Split('＝');
            json += "\"" + data[0] + "\"" + ":";
            if (data[1].IndexOf("\"") < 0)
            {
                json += (data[1] == "null" ? "" : data[1]);
            }
            else
            {
                json += (data[1] == "\"null\"" ? "\"\"" : data[1]);
            }
        }
        json += "}";
        return json;
    }

    /// <summary>
    /// 获取trace输出字符串
    /// </summary>
    /// <returns></returns>
    public string ToTraceString(bool ishand)
    {
        if (ishand)
            return JSON_HAND_HEADER + ToJson() + PAK_TAIL;
        return PAK_HEADER + "{\"k\":" + "0" + ",\"z\":0,\"d\":" + ToJson() + "}" + PAK_TAIL;
    }

#endregion

    /// <summary>
    /// 根据jsondata格式化(注意:只能解一层属性)
    /// </summary>
    /// <param name="jsondata"></param>
    public void FormatByJson(JsonData jsondata)
    {
        message = "";
        if (jsondata.IsObject)
        {
            ICollection keys = (jsondata as IDictionary).Keys;
            foreach (string key in keys)
            {
                if (message.Length != 0)
                    message += "&";
                message += "\"" + key + "\"=" + jsondata[key].ToString();
            }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return message;
    }
}

/// <summary>
/// Net事件多线程转到主线程事件寄存器
/// </summary>
public class NetEvent : ICallEvent
{
    /// <summary>
    /// 接受的数据
    /// </summary>
    public NetReceive receiveData { set; get; }

    /// <summary>
    /// 接受的数据
    /// </summary>
    public NetManager.callback callFun { set; get; }

    /// <summary>
    /// 全参构造
    /// </summary>
    /// <param name="receiveData"></param>
    /// <param name="callback"></param>
    public NetEvent(NetReceive receiveData, NetManager.callback callFun)
    {
         this.callFun = callFun;
         this.receiveData = receiveData;
    }

    #region ICallEvent 成员

    public void CallEvent()
    {
        callFun(receiveData);
    }

    #endregion

    
}
