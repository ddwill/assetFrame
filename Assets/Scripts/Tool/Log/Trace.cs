using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 远程输出类
/// </summary>
public class Trace
{

    private static Trace instance = null;

    private static Trace GetInstance()
    {
        if (instance == null)
            instance = new Trace();
        return instance;
    }

#region 外部调用静态函数

    /// <summary>
    /// 重新连接
    /// </summary>
    public static void ReConnect()
    {
        GetInstance().Connect();
    }
    public static void Log(object msg)
    {
        GetInstance().TraceLog(msg);
    }

    public static void LogError(object msg)
    {
        GetInstance().TraceLogError(msg);
    }

    public static void LogWarning(object msg)
    {
        GetInstance().TraceLogWarning(msg);
    }

    public static void LogCallSvr(object msg)
    {
        GetInstance().TraceLogCallSvr(msg);
    }

    public static void LogCallSvrBack(object msg)
    {
        GetInstance().TraceLogCallSvrBack(msg);
    }

    public static void LogNetIn(object msg)
    {
        GetInstance().TraceLogNetIn(msg);
    }

    public static void LogNetOut(object msg)
    {
        GetInstance().TraceLogNetOut(msg);
    }

    public static void SetMono(MonoBehaviour mono)
    {
        GetInstance().mono = mono;
    }

#endregion

#region 内部调用函数

    public void TraceLog(object msg)
    {
        SendMsg(TraceType.TRACE_INFO, msg);
    }

    public void TraceLogError(object msg)
    {
        SendMsg(TraceType.TRACE_ERROR, msg);
    }

    public void TraceLogWarning(object msg)
    {
        SendMsg(TraceType.TRACE_WARNING, msg);
    }

    public void TraceLogException(object msg)
    {
        SendMsg(TraceType.TRACE_SYSNOTE, msg);
    }

    public void TraceLogCallSvr(object msg)
    {
        SendMsg(TraceType.TRACE_CALLSVR, msg);
    }

    public void TraceLogCallSvrBack(object msg)
    {
        SendMsg(TraceType.TRACE_CALLSVRET, msg);
    }

    public void TraceLogNetIn(object msg)
    {
        SendMsg(TraceType.TRACE_NETIN, msg);
    }

    public void TraceLogNetOut(object msg)
    {
        SendMsg(TraceType.TRACE_NETOUT, msg);
    }

#endregion

#region 基本逻辑

    /// <summary>
    /// 输出模式
    /// </summary>
    public enum TraceMode
    {
        /// <summary>
        /// 本地
        /// </summary>
        Local,
        /// <summary>
        /// 网络
        /// </summary>
        Net,
        /// <summary>
        /// 所有
        /// </summary>
        All,
    }

    /// <summary>
    /// 输出类型
    /// </summary>
    public enum TraceType
    {
        TRACE_ERROR = 0,
        TRACE_SYSNOTE = 1,
        TRACE_WARNING = 2,
        TRACE_INFO = 3,
        TRACE_NETIN = 4,
        TRACE_NETOUT = 5,
        TRACE_CALLSVR = 6,
        TRACE_CALLSVRET = 7,
    }

    private MonoBehaviour _mono;
    private Socket _clientSocket = null;
    private bool _linked = false;
    private int _count;
    private List<NetTracePack> _packlist = new List<NetTracePack>();
    private bool _running = false;

    private const int MAX_FAIL_COUNT = 300;
    private int _failcount;

    /// <summary>
    /// 构造
    /// </summary>
    public Trace()
    {
        Connect();
    }

    /// <summary>
    /// 重新连接
    /// </summary>
    public void Connect()
    {
        if (_clientSocket != null)
        {
            _clientSocket.Close();
            _clientSocket = null;
            _linked = false;
        }
        _count = 0;
        _failcount = 0;
        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipaddress = IPAddress.Parse(Global.Trace_IPAddress);
        IPEndPoint ipendpoint = new IPEndPoint(ipaddress, Global.Trace_Port);
        /*IAsyncResult result = */_clientSocket.BeginConnect(ipendpoint, new AsyncCallback(ConnectCallBack), _clientSocket);
    }

    /// <summary>
    /// 连接成功
    /// </summary>
    /// <param name="asyncConnect"></param>
    private void ConnectCallBack(IAsyncResult asyncConnect)
    {
        //连接成功，发送握手信息
        NetSender sender = new NetSender();
        sender.AddMessageEx("t", 1);
        sender.AddMessageEx("ver", 0x00010000);
        sender.AddMessageEx("type", 0);
        sender.AddMessageEx("charset", "UTF-8");
        sender.AddMessageEx("uname", "");
        sender.AddMessageEx("pwd", "");
        string msg = sender.ToTraceString(true);
        byte[] senddata = Encoding.UTF8.GetBytes(msg);
        /*IAsyncResult asyncsend = */_clientSocket.BeginSend(senddata, 0, senddata.Length, SocketFlags.None, new AsyncCallback(HandOk), _clientSocket);
    }

    /// <summary>
    /// 握手成功
    /// </summary>
    /// <param name="asyncConnect"></param>
    private void HandOk(IAsyncResult asyncConnect)
    {
        _linked = true;
    }

    /// <summary>
    /// 发送信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    private void SendMsg(TraceType type, object data)
    {
        if (Global.Trace_Mode != TraceMode.Net)
            ShowLocalMsg(type, data);
        if (Global.Trace_Mode != TraceMode.Local)
            AppendNetMsg(type, data);
        
    }

    /// <summary>
    /// 显示本地消息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    private void ShowLocalMsg(TraceType type, object data)
    {
        switch (type)
        {
            case TraceType.TRACE_WARNING:
                {
                    Debug.LogWarning(data);
                }
                break;
            case TraceType.TRACE_ERROR:
                {
                    Debug.LogError(data);
                }
                break;
            default:
                {
                    Debug.Log(data);
                }
                break;
        }
        return;
    }

    /// <summary>
    /// 添加网络消息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    private void AppendNetMsg(TraceType type, object data)
    {
        ++_count;
        _packlist.Add(new NetTracePack(type, _count, data));
        if (!_running)
            RestartTrace();
    }

    /// <summary>
    /// 重启网络消息,替换mono时使用
    /// </summary>
    private void RestartTrace()
    {
        if (_failcount >= MAX_FAIL_COUNT)
            return;
        if (mono != null)
        {
            _running = true;
            mono.StartCoroutine(OnSendNetMsg());
        }
    }

    /// <summary>
    /// 准备发送消息
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnSendNetMsg()
    {
        yield return new WaitForSeconds(0.1f);
        if (_packlist.Count == 0)
        {
            _running = false;
        }
        else
        {
            SendNetMsg(_packlist[0]);
            if (mono != null)
                mono.StartCoroutine(OnSendNetMsg());
            else
                _running = false;
        }
    }

    /// <summary>
    /// 发送网络消息
    /// </summary>
    /// <param name="pack"></param>
    private void SendNetMsg(NetTracePack pack)
    {
        if (!_linked)
        {
            ++_failcount;
            return;
        }
        if (!_clientSocket.Connected)
        {
            Connect();
            return;
        }
        try
        {
            IAsyncResult asyncsend = _clientSocket.BeginSend(pack.bytedata, 0, pack.bytedata.Length, SocketFlags.None, null, _clientSocket);
            _packlist.Remove(pack);
            bool success = asyncsend.AsyncWaitHandle.WaitOne(5000, true);
            if (!success)
            {
                Debug.LogWarning("failed send message");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("send message error: " + pack.message + "  " + ex.ToString());
            _packlist.Remove(pack);
        }
    }

    public MonoBehaviour mono
    {
        get { return _mono; }
        set
        {
            if (_mono == value)
                return;
            _mono = value;
            RestartTrace();
        }
    }

#endregion

    
}
