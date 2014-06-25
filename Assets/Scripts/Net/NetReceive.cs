//using LitJson;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
/// <summary>
/// 响应状态
/// </summary>
public enum E_NetState
{
    /// <summary>
    /// 超时
    /// </summary>
    TimeOut = -1,
    /// <summary>
    /// 成功
    /// </summary>
    done = 1,
    /// <summary>
    /// 失败
    /// </summary>
    fail = 2,
    /// <summary>
    /// 服务器推送
    /// </summary>
    server = 3,
}

/// <summary>
/// 接收数据
/// </summary>
public class NetReceive
{
    /// <summary>
    /// 响应状态
    /// </summary>
    public E_NetState state;
    /// <summary>
    /// 请求指令名称
    /// </summary>
    public string command;
    /// <summary>
    /// 响应数据信息
    /// </summary>
    //public JsonData body;
    public MiniJsonData body;
    /// <summary>
    /// 特殊数据,暂未定
    /// </summary>
    //public JsonData spdata;
    public MiniJsonData spdata;
    /// <summary>
    /// 错误信息,当返回信息状态为E_NetState.fail时会附值
    /// </summary>
    public string errmsg;
    /// <summary>
    /// 扩展信息,客户端本地消息回传
    /// </summary>
    public object extdata;

    public NetReceive(string json, object extdata)
    {
        if (json.Length == 0)
        {
            state = E_NetState.TimeOut;
            Trace.LogError("TimeOut");
           /** MonoMainScene.mainCallBack.Add(new NetEvent(null, delegate(NetReceive receive)
            {
                UIAlter.ShowInfo("网络错误,请重新尝试");
                //如果战斗场景出错，则返回主场景
                if (MonoMainScene.NowScene == ScenesID.BattleMain)
                    MonoMainScene.GetNowScene().OpenScenes(ScenesID.GameMain);
            }));**/
            return;
        }
        this.extdata = extdata;

        try
        {
            Dictionary<string, object> jsondata = Json.Deserialize(json) as Dictionary<string, object>;
            //Debug.Log(((int)jsondata["state"]).ToString() + jsondata["command"]);
            state = (E_NetState)((int)jsondata["state"]);
            command = jsondata["command"].ToString();
            body = new MiniJsonData(jsondata["body"]);
            if (state == E_NetState.fail)
            {
                errmsg = body["msg"].ToString();
                Trace.LogError(command + "请求出错!  info:" + errmsg);
               /** MonoMainScene.mainCallBack.Add(new NetEvent(null, delegate(NetReceive receive)
                {
                    UIAlter.ShowInfo(errmsg);
                    //如果战斗场景出错，则返回主场景
                    if (MonoMainScene.NowScene == ScenesID.BattleMain)
                        MonoMainScene.GetNowScene().OpenScenes(ScenesID.GameMain);
                }));**/
                return;
            }
            //更新数据处理
            if (jsondata.ContainsKey("update"))
            {
                spdata = new MiniJsonData(jsondata["update"]);
                if (spdata != null && spdata.IsArray)
                {
                    foreach (object obj in spdata.listData)
                    {
                        MiniJsonData data = new MiniJsonData(obj);
                        if (data.Count == 2)
                        {
                            //NetManager.UpdateSeverMsg(new NetUpdateData(data["key"].ToString(), data["value"]));
                        }
                    }
                }
            }
            Trace.LogCallSvrBack("callback:" + command + " redata:" + json);
        }
        catch (System.Exception ex)
        {
            Trace.LogError(ex.Message + "; Data:" + ex.Data + ";Source: " + ex.Source
                + ";Source:" + ex.StackTrace + ";InnerException:" + ex.InnerException + ";TargetSite:" + ex.TargetSite);
        }
    }
    public NetReceive() { }

    /// <summary>
    /// 是否是完成的请求
    /// </summary>
    public bool isComplate
    {
        get { return state == E_NetState.done; }
    }

    /// <summary>
    /// 是否存在回发信息
    /// </summary>
    public bool hasMessage
    {
        get { return state == E_NetState.fail; }
    }
}