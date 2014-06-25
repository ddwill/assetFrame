using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using signal.osnet.signals;
using LitJson;
using System;
using System.Globalization;
public enum WebPlatform
{
    AppStore,
    IOS91,
    PCTest
}
public enum Platform
{
    Windows,
    IOS,
    Android,
    WP
}
/// <summary>
/// 全局实体管理类
/// </summary>
public class Global
{
    private static Global _impl;

    private Global()
    {
    }
    public static Global GetInstance()
    {
        if (_impl == null)
        {
            _impl = new Global();
        }
        return _impl;
    }

    #region 调试配置

    /// <summary>
    /// 远程输出连接IP
    /// </summary>
    public const string Trace_IPAddress = "192.168.1.36";
    /// <summary>
    /// 远程输出连接端口
    /// </summary>
    public const int Trace_Port = 9438;
    /// <summary>
    /// 输出模式
    /// </summary>
    public const Trace.TraceMode Trace_Mode = Trace.TraceMode.All;

    #endregion

	#region 系统配制
    /// <summary>
    /// 发布平台
    /// </summary>
    public static WebPlatform webPlatform = WebPlatform.PCTest;
    /// <summary>
    /// 当前运行平台
    /// </summary>
    public static Platform platform = Platform.Windows;

    /// <summary>
    /// 当前区域信息
    /// </summary>
    public static CultureInfo CurrentCulture
    {
        get
        {
            return CultureInfo.CreateSpecificCulture("zh-CN");
        }
    }

    /// <summary>
    /// 版本路径
    /// </summary>
    public string urlVersion
    {
        get
        {
            if (platform == Platform.IOS)
                return "http://203.195.186.211:8080/AssetVersion_Android";
                //return "http://192.168.1.7/AssetVersion_IOS";
            else if (platform == Platform.Android)
                return "http://203.195.186.211:8080/AssetVersion_Android";
            //return "http://192.168.1.7/AssetVersion_Android";
            else
                //return "http://192.168.1.10:8081/AssetVersion";
                return "http://192.168.1.7/AssetVersion";
        }
    }

    public string docFile
    {
        get
        {
            if (platform == Platform.IOS)
                return "/build/OverBuild_IOS";
            else if (platform == Platform.Android)
                return "/build/OverBuild_Android";
            else
                return "/build/OverBuild";
        }
    }
    /// <summary>
    /// 版本文件名
    /// </summary>
    public string fileVersion = "/AssetVersion.xml";
    /// <summary>
    /// 回收缓存资源时间，防止立即回收产生的BUG
    /// </summary>
    public int UnLoadAssetTime = 2000;
    /// <summary>
    /// 服务器中心地址
    /// </summary>
    public string user_center_address = "http://192.168.1.11";
    //public string user_center_address = "http://192.168.1.10:8080";
    //public string user_center_address = "http://192.168.1.25:81";
    //public string user_center_address = "http://203.195.186.211:85";
    /// <summary>
    /// 服务器游戏逻辑地址
    /// </summary>
    public string user_game_address = "http://192.168.1.11";
    //public string user_game_address = "http://192.168.1.10:8080";
    //public string user_game_address = "http://192.168.1.25:81";
    //public string user_game_address = "http://203.195.186.211:85";
    /// <summary>
    /// 文件服务器地址
    /// </summary>
    public string game_file_address = "http://192.168.1.11";
    //public string game_file_address = "http://192.168.1.10:8080";
    //public string game_file_address = "http://192.168.1.25:81";
    //public string game_file_address = "http://203.195.186.211:85";

    /// <summary>
    /// 资源包后缀
    /// </summary>
    public static string AssetPackageSuffix = ".assetbundle";


	/// <summary>
    /// 单页卡牌最大显示数量
    /// </summary>
	public const int MAX_PAGE_SHOW = 15;
    /// <summary>
    /// 物件移动间隔时间(毫秒)
    /// </summary>
    public const int Item_Move_Time_Gap = 50;

    /// <summary>
    /// 最少帮手数量，低于此数量时,会重新刷新
    /// </summary>
    public const int Min_Helper_Count = 1;
    //public bool UrlToBase64 = true;

    /// <summary>
    /// 用户设置本地存放文件名
    /// </summary>
    public const string LocalDataFileName = "UserConfig.dat";

    /// <summary>
    /// 本地文件加密key
    /// </summary>
    public const string LocalDataKey = "12DCD7E148382DDAFB714321749A1EDB";

    /// <summary>
    /// 最大密码长度
    /// </summary>
    public const int MAX_PASSWORD_LENGTH = 15;
    /// <summary>
    /// 最少密码长度
    /// </summary>
    public const int MIN_PASSWORD_LENGTH = 6;
    /// <summary>
    /// 最大帐号长度
    /// </summary>
    public const int MAX_ACCNAME_LENGTH = 15;
    /// <summary>
    /// 最小帐号长度
    /// </summary>
    public const int MIN_ACCNAME_LENGTH = 6;


    #endregion

    #region 实体缓存配置

    /// <summary>
    /// 清除玩家基本信息缓存间隔时间(10000000 = 1秒)
    /// </summary>
    public const long USER_SAMPLE_CACHE_DEL_TIME = 3000000000;
    /// <summary>
    /// 清除好友信息间隔时间(10000000 = 1秒)
    /// </summary>
    public const long CLEARFRIEND_TIME = 3000000000;

    #endregion

    #region 资源管理
    /// <summary>
    /// 主场景立刻加载的组员
    /// </summary>
    public List<string> LoadMainPaths = new List<string>(){
                                           "/1/NGUIAsset.assetbundle",
                                           "/1/NGUILoad.assetbundle",
                                            "/2/Prefab_Load.assetbundle"};
    /// <summary>
    /// 主场景Prefab路径包
    /// </summary>
    public string MainPrefabPaths = "/2/Prefab_Load.assetbundle";
    /// <summary>
    /// 游戏资源管理配置路径
    /// </summary>
    public string GameDataPath = "/1/GameData.assetbundle";
    /// <summary>
    /// Prefab路径包
    /// </summary>
    public string PrefabPaths = "/2/Prefabs.assetbundle";

	/// <summary>
	/// 游戏配置信息
	/// </summary>
	public GameConfigData gameConfigData = null;


    /// <summary>
    /// 场景资源统一加载配置
    /// </summary>
   /** public ScenesAssetData ScenesAssetData = null;

    /// <summary>
    /// 资源贴图大小配置信息
    /// </summary>
    public AssetSizeData assetSizeData = null;



    #endregion

    /// <summary>
    /// 玩家信息
    /// </summary>
    public User user = new User();

    /// <summary>
    /// 玩家账号信息
    /// </summary>
    public UserAcc useracc = new UserAcc();

    /// <summary>
    /// 游戏消息
    /// </summary>
    private GameEvents gameevents = new GameEvents();
    /// <summary>
    /// 服务器列表信息
    /// </summary>
    public ServerList serverlist = new ServerList();
    /// <summary>
    /// 系统公告列表
    /// </summary>
    public List<string> sysnotes = new List<string>();
    /// <summary>
    /// 当前服务器ID
    /// </summary>
    public int serverid = 1;

    #region 系统公告操作

    /// <summary>
    /// 更新系统公告
    /// </summary>
    /// <param name="obj">暂未确定类型</param>
    public void UpdateSysNotes(object obj)
    {
        //操作更新系统公告

        //推送更新消息
        Event_SysInfoUpdate.dispatch();
    }

    /// <summary>
    /// 添加默认公告
    /// </summary>
    public void AddDefSysNote()
    {
        sysnotes.Add("欢迎来到百战三界!");
        Event_SysInfoUpdate.dispatch();
    }

    /// <summary>
    /// 弹出单条系统信息
    /// </summary>
    /// <returns></returns>
    public string PopSysNote()
    {
        if (sysnotes.Count == 0)
        {
            return "";
        }
        string note = sysnotes[0];
        sysnotes.RemoveAt(0);
        return note;
    }
    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="obj">暂未确定类型</param>
    public void UpdateUserInfo(object obj)
    {
        Trace.Log("UpdateUserInfo");
        if (obj != null && obj is MiniJsonData)
            user.DeserializeBaseData(obj as MiniJsonData);
        //推送更新消息
        Event_UserUpdate.dispatch(user);
    }
    #endregion

    #region 消息管理

    /// <summary>
    /// 系统公告更新消息
    /// </summary>
    public Signal Event_SysInfoUpdate
    {
        get { return gameevents.SYSINFO_UPDATE; }
    }

    /// <summary>
    /// 用户更新消息
    /// </summary>
    public Signal Event_UserUpdate
    {
        get { return gameevents.USER_UPDATE; }
    }**/
    #endregion
}