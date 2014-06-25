using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

/// <summary>
/// 网络管理
/// </summary>
public class NetManager
{

	/// <summary>
	/// 回调函数
	/// </summary>
	/// <param name="receive"></param>
	public delegate void callback(NetReceive receive);

  /**  /// <summary>
    /// 是否在主线程中进行回调
    /// </summary>
    public static bool isMainCallback = true;


    /// <summary>
    /// 发送信息列表,用做检测重复发送消息判定
    /// </summary>
    public static List<string> sendlist = new List<string>();

    #region 消息地址管理

    /// <summary>
    /// 服务器中心地址
    /// </summary>
    protected static string server_user_center
    {
        get
        {
            return Global.GetInstance().user_center_address + "/user_center";
        }
    }
    /// <summary>
    /// 服务器游戏逻辑地址
    /// </summary>
    protected static string server_user_game
    {
        get
        {
            return Global.GetInstance().user_game_address + "/game_server";
        }
    }
    /// <summary>
    /// 文件服务器地址
    /// </summary>
    protected static string server_game_file
    {
        get
        {
            return Global.GetInstance().game_file_address + "/file_server";
        }
    }

    #endregion

    #region 获取服务器消息

    /// <summary>
    /// 获取服务器列表
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="fun">回调函数</param>
    public static void GetServerList(MonoBehaviour mono, callback fun)
    {
        CallServer(mono, server_user_center + "/servers!list", fun);
    }

    #endregion

    #region 登陆/注册消息

    /// <summary>
    /// 尝试使用IMEI号登陆
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="imei">IMEI手机唯一标识</param>
    /// <param name="fun">回调函数</param>
    public static void TryIMEILogin(MonoBehaviour mono, string imei, callback fun)
    {
        NetSender sender = new NetSender();
        sender.AddMessage("imei", imei);
        CallServer(mono, server_user_game + "/gameUser!imeiLogin", sender.ToString(), fun);
    }

    /// <summary>
    /// 正常使用用户名和密码登陆
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="username">用户名</param>
    /// <param name="pwd">密码</param>
    /// <param name="fun">回调函数</param>
    public static void Login(MonoBehaviour mono, string username, string pwd, callback fun)
    {
        NetSender sender = new NetSender();
        sender.AddMessage("userName", username);
        sender.AddMessage("pwd", pwd);
        CallServer(mono, server_user_game + "/gameUser!login", sender.ToString(), fun);
    }

    /// <summary>
    /// 绑定账号
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="imei">IMEI手机唯一标识</param>
    /// <param name="username">用户名</param>
    /// <param name="pwd">密码</param>
    /// <param name="fun">回调函数</param>
    [Obsolete]
    public static void BindingUser(MonoBehaviour mono, string imei, string username, string pwd, callback fun)
    {
        NetSender sender = new NetSender();
        sender.AddMessage("imei", imei);
        sender.AddMessage("username", username);
        sender.AddMessage("pwd", pwd);
        CallServer(mono, server_user_game + "/gameUser!bindingUser", sender.ToString(), fun);
    }

    /// <summary>
    /// 使用手机IMEI号创建账号
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="imei">IMEI手机唯一标识</param>
    /// <param name="usersource">用户来源</param>
    /// <param name="systemtype">系统类型</param>
    /// <param name="fun">回调函数</param>
    public static void imeiCreateUser(MonoBehaviour mono, string imei, string usersource, string systemtype, callback fun)
    {
        NetSender sender = new NetSender();
        sender.AddMessage("imei", imei);
        sender.AddMessage("userSource", usersource);
        sender.AddMessage("systemType", systemtype);
        CallServer(mono, server_user_game + "/gameUser!imeiCreateUser", sender.ToString(), fun);
    }

    /// <summary>
    /// 使用用户名密码创建账号
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="username">用户名</param>
    /// <param name="pwd">密码</param>
    /// <param name="usersorce">用户来源</param>
    /// <param name="systemtype">系统类型</param>
    /// <param name="fun">回调函数</param>
    public static void CreateUser(MonoBehaviour mono, string username, string pwd, string usersource, string systemtype, callback fun)
    {
        NetSender sender = new NetSender();
        sender.AddMessage("userName", username);
        sender.AddMessage("pwd", pwd);
        sender.AddMessage("userSource", usersource);
        sender.AddMessage("systemType", systemtype);
        CallServer(mono, server_user_game + "/gameUser!createUser", sender.ToString(), fun);
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="serverid">回调函数</param>
    /// <param name="fun">回调函数</param>
    public static void EnterGame(MonoBehaviour mono, int serverid, callback fun)
    {
        NetSender sender = new NetSender();
        sender.AddMessage("gameServerId", serverid);
        CallServer(mono, server_user_game + "/player!enter", sender.ToString(), false, Global.GetInstance().useracc.sessionId, fun);
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="serverid">服务器ID</param>
    /// <param name="name">角色名字</param>
    /// <param name="job">角色职业</param>
    /// <param name="fun">回调函数</param>
    public static void CreateRole(MonoBehaviour mono, int serverid, string name, int job, callback fun)
    {
        NetSender sender = new NetSender();
        sender.AddMessage("gameServerId", serverid);
        sender.AddMessage("name", name);
        sender.AddMessage("generalId", job);
        CallServer(mono, server_user_game + "/player!create", sender.ToString(), false, Global.GetInstance().useracc.sessionId, fun);
    }

    /// <summary>
    /// 获取随机名字
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="fun">回调函数</param>
    public static void GetRandomName(MonoBehaviour mono, callback fun)
    {
        NetSender sender = new NetSender();
        CallServer(mono, server_user_game + "/player!getRandomName", sender.ToString(), false, Global.GetInstance().useracc.sessionId, fun);
    }

    /// <summary>
    /// 删除帐号,测试用
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="imei"></param>
    /// <param name="fun"></param>
    public static void DelUserData(MonoBehaviour mono, string imei, callback fun)
    {
        NetSender sender = new NetSender();
        sender.AddMessage("imei", imei);
        CallServer(mono, server_user_game + "/player!clearData", sender.ToString(), false, fun);
    }

    /// <summary>
    /// 保存用户设置
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="setting">设置值</param>
    /// <param name="fun"></param>
    public static void SaveUserConfig(MonoBehaviour mono, int setting, callback fun)
    {
        NetSender sender = new NetSender();
        sender.AddMessage("setting", setting);
        CallServer(mono, server_user_game + "/player!updateSetting", sender.ToString(), false, Global.GetInstance().useracc.sessionId, fun);
    }
    #endregion

    #region Call服务器逻辑

    /// <summary>
    /// Call服务器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="url">url地址</param>
    /// <param name="fun">回调函数</param>
    protected static void CallServer(MonoBehaviour mono, string url, callback fun)
    {
        CallServer(mono, url, null, null, false, null, null, fun);
    }

    /// <summary>
    /// Call服务器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="url">url地址</param>
    /// <param name="iszip">接收类型是否是压缩类型</param>
    /// <param name="fun">回调函数</param>
    protected static void CallServer(MonoBehaviour mono, string url, bool iszip, callback fun)
    {
        CallServer(mono, url, null, null, iszip, null, null, fun);
    }

    /// <summary>
    /// Call服务器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="url">url地址</param>
    /// <param name="postdata">POST参数</param>
    /// <param name="fun">回调函数</param>
    protected static void CallServer(MonoBehaviour mono, string url, string postdata, callback fun)
    {
        CallServer(mono, url, postdata, null, false, null, null, fun);
    }

    /// <summary>
    /// Call服务器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="url">url地址</param>
    /// <param name="bytedata">byte[]数据</param>
    /// <param name="fun">回调函数</param>
    protected static void CallServer(MonoBehaviour mono, string url, byte[] bytedata, callback fun)
    {
        CallServer(mono, url, null, bytedata, false, null, null, fun);
    }

    /// <summary>
    /// Call服务器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="url">url地址</param>
    /// <param name="postdata">POST参数</param>
    /// <param name="iszip">接收类型是否是压缩类型</param>
    /// <param name="fun">回调函数</param>
    protected static void CallServer(MonoBehaviour mono, string url, string postdata, bool iszip, callback fun)
    {
        CallServer(mono, url, postdata, null, false, null, null, fun);
    }

    /// <summary>
    /// Call服务器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="url">url地址</param>
    /// <param name="postdata">POST参数</param>
    /// <param name="iszip">接收类型是否是压缩类型</param>
    /// <param name="head">头信息</param>
    /// <param name="fun">回调函数</param>
    protected static void CallServer(MonoBehaviour mono, string url, string postdata, bool iszip, string head, callback fun)
    {
        CallServer(mono, url, postdata, null, false, head, null, fun);
    }

    /// <summary>
    /// Call服务器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="url">url地址</param>
    /// <param name="postdata">POST参数</param>
    /// <param name="iszip">接收类型是否是压缩类型</param>
    /// <param name="head">头信息</param>
    /// <param name="exdata">扩展信息,客户端本地回传消息</param>
    /// <param name="fun">回调函数</param>
    protected static void CallServer(MonoBehaviour mono, string url, string postdata, bool iszip, string head, object extdata, callback fun)
    {
        CallServer(mono, url, postdata, null, false, head, null, fun);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="url">url地址</param>
    /// <param name="postdata">POST参数</param>
    /// <param name="bytedata">数组数据,当不为null时,优先使用此数据</param>
    /// <param name="iszip">接收类型是否是压缩类型</param>
    /// <param name="head">头信息</param>
    /// <param name="exdata">扩展信息,客户端本地回传消息</param>
    /// <param name="fun">回调函数</param>
    protected static void CallServer(MonoBehaviour mono, string url, string postdata, byte[] bytedata, bool iszip, string head, object extdata, callback fun)
    {
        //检测是否有重复发的消息
        if (sendlist.IndexOf(url) != -1)
            return;
        Trace.LogCallSvr("CallServer:" + url + " postdata:" + postdata + " isMainCallback:" + isMainCallback);
        HttpHelper.AsynSendData(url, postdata, bytedata, postdata == null ? HttpMethod.GET : HttpMethod.POST, iszip, head, delegate(string data)
        {
            sendlist.Remove(url);
            if (isMainCallback)
            {
                NetReceive receive = new NetReceive(data, extdata);
                if (!receive.hasMessage)
                    MonoMainScene.mainCallBack.Add(new NetEvent(receive, fun));
            }
            else
                fun(new NetReceive(data, extdata));
        });
    }

    #endregion

    #region 服务器update消息更新

    /// <summary>
    /// 经验
    /// </summary>
    private const string KEY_EXP = "exp";
    /// <summary>
    /// 体力
    /// </summary>
    private const string KEY_STAM = "stam";
    /// <summary>
    /// 等级
    /// </summary>
    private const string KEY_LEVEL = "level";
    /// <summary>
    /// 金币
    /// </summary>
    private const string KEY_GOLD = "gold";
    /// <summary>
    /// 银币
    /// </summary>
    private const string KEY_MONEY = "silver";
    /// <summary>
    /// 升级需要经验
    /// </summary>
    private const string KEY_LVUPEXP = "levelUpExp";
    /// <summary>
    /// 鲜花
    /// </summary>
    private const string KEY_FLOWER = "flower";
    /// <summary>
    /// 接收鲜花数量
    /// </summary>
    private const string KEY_ACCEPTFLOWER = "acceptFlower";
    /// <summary>
    /// 背包物品
    /// </summary>
    private const string KEY_BAGS = "bags";

    /// <summary>
    /// 背包物品
    /// </summary>
    private const string KEY_CARDS = "cards";

    /// <summary>
    /// 灵石
    /// </summary>
    private const string KEY_LINGSHI = "lingshi";

    /// <summary>
    /// 声望
    /// </summary>
    private const string KEY_PRESTIGE = "prestige";

    /// <summary>
    /// 阅历
    /// </summary>
    private const string KEY_SYSEXP = "sys_exp";

    /// <summary>
    /// 可领取任务
    /// </summary>
    private const string KEY_TASK_CAN_PICK = "task_can_pick";

    /// <summary>
    /// 进行中任务
    /// </summary>
    private const string KEY_TASK_PICK = "task_pick";

    /// <summary>
    /// 已完成任务
    /// </summary>
    private const string KEY_TASK_COMPLETE = "task_complete";

    /// <summary>
    /// 已删除任务
    /// </summary>
    private const string KEY_TASK_DEL = "task_del";

    /// <summary>
    /// vip经验
    /// </summary>
    private const string KEY_VIPEXP = "vipExp";

    /// <summary>
    /// vip等级
    /// </summary>
    private const string KEY_VIPLV = "vipLevel";

    /// <summary>
    /// 喇叭数量
    /// </summary>
    private const string KEY_HORN = "hornCount";

    /// <summary>
    /// 更新服务器消息
    /// </summary>
    /// <param name="data"></param>
    public static void UpdateSeverMsg(NetUpdateData data)
    {
        Trace.LogNetIn("update:" + data.UpdateKey + ":" + data.UpdateData);
        switch (data.UpdateKey)
        {
            case KEY_EXP:
                {
                }
                break;
            case KEY_STAM:
                {
                }
                break;
            case KEY_LEVEL:
                {
                }
                break;
            case KEY_GOLD:
                {
                }
                break;
            case KEY_MONEY:
                {
                }
                break;
            case KEY_LVUPEXP:
                {
                }
                break;
            case KEY_FLOWER:
                {
                }
                break;
            case KEY_ACCEPTFLOWER:
                {
                }
                break;
            //VIP经验
            case KEY_VIPEXP:
                {
                }
                break;
            //VIP等级
            case KEY_VIPLV:
                {
                }
                break;
            //喇叭数量
            case KEY_HORN:
                {
                }
                break;
            case KEY_BAGS:
                {
                    
                }
                break;
            case KEY_CARDS:
                {
                }
                break;
            case KEY_LINGSHI:
                {
                }
                break;
            case KEY_PRESTIGE:
                {
                }
                break;
            case KEY_SYSEXP:
                {
                }
                break;
            case KEY_TASK_CAN_PICK:
                {
                }
                break;
            case KEY_TASK_PICK:
                {
                }
                break;
            case KEY_TASK_COMPLETE:
                {
                }
                break;
            case KEY_TASK_DEL:
                {
                    string[] s = data.UpdateData.ToString().Split(',');
                }
                break;
            default:
                {
                    Trace.LogWarning("unknow update type:   key:" + data.UpdateKey + "   data:" + data.UpdateData);
                }
                break;
        }
    }

    #endregion**/
}
