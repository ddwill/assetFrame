using System;
using System.Text;
using System.Collections;
using UnityEngine;

/// <summary>
/// 网络发送包
/// </summary>
public class NetTracePack
{
    /// <summary>
    /// 类型
    /// </summary>
    public Trace.TraceType packType;

    /// <summary>
    /// 内容
    /// </summary>
    private NetSender _message;
    
    /// <summary>
    /// 字节内容
    /// </summary>
    public byte[] bytedata;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    /// <param name="msg"></param>
    public NetTracePack(Trace.TraceType type, int count, object msg)
    {
        packType = type;
        _message = new NetSender();
        _message.AddMessageEx("f", count);
        _message.AddMessageEx("t", 0x7FFFFFFF);
        _message.AddMessageEx("e", (int)type);
        _message.AddMessageEx("v", new ArrayList() { MakeStr(msg) });
        bytedata = Encoding.UTF8.GetBytes(_message.ToTraceString(false));
    }

    public string message
    {
        get { return _message.ToTraceString(false); }
    }

    private string MakeStr(object obj)
    {
        if (obj == null)
            return "null";
        if (obj is string)
        {
            obj = (obj as string).Replace("\"", "＂");
            obj = (obj as string).Replace("{", "｛");
            obj = (obj as string).Replace("}", "｝");
        }
        return obj.ToString();
    }
}
