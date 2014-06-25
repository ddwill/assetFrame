using signal.osnet.signals;

/// <summary>
/// 游戏消息
/// </summary>
public class GameEvents : SignalEvents
{
    public GameEvents()
		: base(new string[]{
			"USER_UPDATE"
            ,"SYSINFO_UPDATE"
            ,"WINDOWSTATE_UPDATE"
            ,"TASK_UPDATE"
		})
	{
	}

    /// <summary>
    /// 用户更新消息
    /// </summary>
    public Signal USER_UPDATE
    {
        get { return getSignal("USER_UPDATE"); }
    }

    /// <summary>
    /// 系统公告更新消息
    /// </summary>
    public Signal SYSINFO_UPDATE
    {
        get { return getSignal("SYSINFO_UPDATE"); }
    }

    /// <summary>
    /// 窗口状态更新
    /// </summary>
    public Signal WINDOWSTATE_UPDATE
    {
        get { return getSignal("WINDOWSTATE_UPDATE"); }
    }

    public Signal TASK_UPDATE
    {
        get { return getSignal("TASK_UPDATE"); }
    }

    /// <summary>
    /// Vip更新
    /// </summary>
    //public Signal VIP_UPDATE
    //{
    //    get { return getSignal("VIP_UPDATE"); }
    //}
}

