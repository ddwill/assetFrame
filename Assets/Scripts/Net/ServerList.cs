using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

/// <summary>
/// 服务器状态枚举
/// </summary>
public enum E_ServerState
{
    /// <summary>
    /// 关闭
    /// </summary>
    Close = 1,
    /// <summary>
    /// 开启/正常
    /// </summary>
    Open = 2,
    /// <summary>
    /// 维护
    /// </summary>
    Maintain = 3,
    /// <summary>
    /// 繁忙
    /// </summary>
    Busy = 4,
    /// <summary>
    /// 推荐
    /// </summary>
    Recommend = 5
}

/// <summary>
/// 服务器信息
/// </summary>
public class ServerInfo : IJsonSerialize
{
    /// <summary>
    /// 服务器标识
    /// </summary>
    public int serverID;
    /// <summary>
    /// 服务器名称
    /// </summary>
    public string serverName;
    /// <summary>
    /// 服务器IP
    /// </summary>
    public string serverHost;
    /// <summary>
    /// 服务端口
    /// </summary>
    public int serverPort;
    /// <summary>
    /// 服务器状态
    /// </summary>
    public E_ServerState state;

    /// <summary>
    /// 构造
    /// </summary>
    public ServerInfo()
    {
        serverID = -1;
        serverName = "";
        serverHost = "";
        serverPort = -1;
        state = E_ServerState.Close;
    }


    #region IJsonSerialize 成员

    public void Deserialize(MiniJsonData jsondata)
    {
        serverID = (int)jsondata["serverId"];
        serverName = jsondata["serverName"].ToString();
        serverHost = jsondata["serverHost"].ToString();
        serverPort = (int)jsondata["serverPort"];
        state = (E_ServerState)(int)jsondata["state"];

        //recommend = int.Parse(jsondata["recommend"].ToString());

    }

    //public JsonData Serialize()
    //{
    //    JsonData jsondata = new JsonData();
    //    jsondata["serverId"] = serverID;
    //    jsondata["serverName"] = serverName;
    //    jsondata["serverHost"] = serverHost;
    //    jsondata["serverPort"] = serverPort;
    //    jsondata["state"] = (int)state;
    //    return jsondata;
    //}

    #endregion
}

/// <summary>
/// 服务器列表
/// </summary>
public class ServerList : JsonListBase<ServerInfo>
{
    public List<ServerInfo> serverlist
    {
        get { return dataList; }
        set { dataList = value; }
    }

    /// <summary>
    /// 构造
    /// </summary>
    public ServerList()
    {
        serverlist = new List<ServerInfo>();
    }

    /// <summary>
    /// 根据ID 查找服务器
    /// </summary>
    /// <param name="id">服务器ID</param>
    /// <returns></returns>
    public ServerInfo FindServerByID(int id)
    {
        foreach (ServerInfo server in serverlist)
        {
            if (server.serverID == id)
            {
                return server;
            }
        }
        return null;
    }

    /// <summary>
    /// 根据服务器名 查找服务器
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public ServerInfo FindServerByName(string name)
    {
        foreach (ServerInfo server in serverlist)
        {
            if (server.serverName == name)
            {
                return server;
            }
        }
        return null;
    }
}