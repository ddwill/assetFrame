using System;
using signal.osnet.signals;
using UnityEngine;
using System.Collections;

/// <summary>
/// 定时器,使用本地时间比较
/// </summary>
public class GTimer
{
	public delegate void TimerHandler();

    /// <summary>
    /// 最小延时默认0.016666f
    /// </summary>
    public static float s_MinDelay = 0.016666f;

    private long m_nDelay = 10000000;
    private int m_nCurrentCount;
    private int m_nRepeatCount;
    private bool m_bRunning;
    //private TimerEvents m_TimerEvent;
	private TimerHandler m_completehandler;
	private TimerHandler m_repeatHandler;

    private MonoBehaviour m_Mono;

    private long m_nCurDelay = 0;
    private long m_nLastTime = 0;
    private int m_nNowTimeID = 0;

    private bool m_bKilling = false;

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="dealy">延时（毫秒）</param>
    /**public GTimer(MonoBehaviour mono, int dealy)
    {

        m_nRepeatCount = 0;
        m_nDelay = dealy <= 0 ? 10000000 : dealy * 10000;
        m_Mono = mono;
        Init();
    }**/

	public GTimer(MonoBehaviour mono,int delay,TimerHandler handler)
	{
		m_nRepeatCount = 1;
		m_nDelay = delay <= 0 ? 10000000 : delay * 10000;
		m_Mono = mono;
		this.m_completehandler = handler;
		Init();
	}

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="dealy">延时（毫秒）</param>
    /// <param name="repeatCount">执行次数，0为无限</param>
    public GTimer(MonoBehaviour mono, int dealy, int repeatCount,TimerHandler handler)
    {
        m_nRepeatCount = repeatCount;
        m_nDelay = dealy <= 0 ? 10000000 : dealy * 10000;
        m_Mono = mono;
		m_completehandler = handler;
        Init();
    }

	public GTimer(MonoBehaviour mono, int dealy, int repeatCount,TimerHandler completehandler,TimerHandler repeatHandler)
	{
		m_nRepeatCount = repeatCount;
		m_nDelay = dealy <= 0 ? 10000000 : dealy * 10000;
		m_Mono = mono;
		m_completehandler = completehandler;
		m_repeatHandler = repeatHandler;
		Init();
	}

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        m_bRunning = false;
        m_nCurrentCount = 0;
        m_nCurDelay = 0;
        m_nLastTime = DateTime.Now.Ticks;
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public void DelThis()
    {
        if (m_bKilling)
            return;
        Stop();
        m_bKilling = true;
        if (m_Mono != null && m_Mono.gameObject.activeSelf)
            m_Mono.StartCoroutine(StartDelThis());
        else
        {
			//delete
        }
    }

    /// <summary>
    /// 开始销毁
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartDelThis()
    {
        yield return new WaitForSeconds(s_MinDelay);
        //m_TimerEvent.Dispose();
        //m_TimerEvent = null;
        m_Mono = null;
    }

    /// <summary>
    /// 设置延时（单位毫秒），如果指定的延迟为负数将默认为1秒
    /// </summary>
    /// <returns></returns>
    public int delay
    {
        get
        {
            return (int)(m_nDelay / 10000);
        }
        set
        {
            m_nDelay = value * 10000;
            if (m_nDelay <= 0)
                m_nDelay = 10000000;
        }
    }

    /// <summary>
    /// 设置的计时器运行总次数。如果重复计数设置为 0，则计时器将持续不断运行，直至调用了 stop() 方法或程序停止。如果重复计数不为 0，则将运行计时器，运行次数为指定的次数。如果设置的 repeatCount 总数等于或小于 currentCount，则计时器将停止并且不会再次触发。
    /// </summary>
    /// <returns></returns>
    public int repeatCount
    {
        get
        {
            return m_nRepeatCount;
        }
        set
        {
            m_nRepeatCount = value;
        }
    }

    /// <summary>
    /// [只读] 计时器从 0 开始后触发的总次数。如果已重置了计时器，则只会计入重置后的触发次数。
    /// </summary>
    /// <returns></returns>
    public int currentCount
    {
        get
        {
            return m_nCurrentCount;
        }
    }

    /// <summary>
    /// [只读] 计时器的当前状态；如果计时器正在运行，则为 true，否则为 false。
    /// </summary>
    /// <returns></returns>
    public bool running
    {
        get
        {
            if (m_Mono != null)
                return m_bRunning;
            else
                return false;
        }
    }

    /// <summary>
    /// 如果计时器尚未运行，则启动计时器。
    /// </summary>
    public void Start()
    {
        if (running || m_bKilling)
            return;
        if (m_Mono == null || m_Mono.gameObject.activeSelf == false)
        {
            DelThis();
            return;
        }
        m_bRunning = true;
        m_nLastTime = DateTime.Now.Ticks;
        ++m_nNowTimeID;
        m_Mono.StartCoroutine(UpdateTime(m_nNowTimeID));
    }

    /// <summary>
    /// 停止计时器。如果在调用 stop() 后调用 start()，则将继续运行计时器实例，运行次数为剩余的 重复次数（由 repeatCount 属性设置）。
    /// </summary>
    public void Stop()
    {
        m_bRunning = false;
    }

    /// <summary>
    /// 如果计时器正在运行，则停止计时器，并将 c回urrentCount 属性设为 0，这类似于秒表的重置按钮。然后，在调用 start() 后，将运行计时器实例，运行次数为指定的重复次数（由 repeatCount 值设置）。
    /// </summary>
    public void Reset()
    {
        m_nCurrentCount = 0;
        m_nCurDelay = 0;
        m_bRunning = false;
    }

    private IEnumerator UpdateTime(int runid)
    {
        yield return new WaitForSeconds(s_MinDelay);
        RefreshTime(runid);
    }

    private void RefreshTime(int runid)
    {
        if (m_Mono == null && m_Mono.gameObject.activeSelf == true)
        {
            DelThis();
            return;
        }
        if (m_nNowTimeID != runid)
            return;
        long now = DateTime.Now.Ticks;
        long passtime = now - m_nLastTime;
        m_nLastTime = now;
        if (passtime < 0 || passtime >= 10000000)
        {
            m_Mono.StartCoroutine(UpdateTime(runid));
            return;
        }
        m_nCurDelay += passtime;
        while (CanOnUpdate())
            OnUpdate();
        if (!CheckOver() && m_bRunning)
        {
            m_Mono.StartCoroutine(UpdateTime(runid));
        }
    }

    private void OnUpdate()
    {
        m_nCurDelay -= m_nDelay;
        ++m_nCurrentCount;
        //m_TimerEvent.TIMER.dispatch(this);
		if(this.m_repeatHandler!=null) this.m_repeatHandler ();
    }

    private bool CanOnUpdate()
    {
        if (m_nRepeatCount > 0 && m_nCurrentCount >= m_nRepeatCount)
            return false;
        return m_nCurDelay >= m_nDelay;
    }

    private bool CheckOver()
    {

        if ( m_nCurrentCount >= m_nRepeatCount)
        {
            //m_TimerEvent.TIMER_COMPLETE.dispatch(this);
			//m_handler();
			m_completehandler();

            if(isOverDel)
                Reset();
            else
                DelThis();
            return true;
        }
        return false;
    }

    #region 暂废弃
    //     public void Update()
    //     {
    //         if (!running)
    //             return;
    //         long now = DateTime.Now.Ticks;
    //         int passtime = (int)(now - m_nLastTime);
    //         m_nLastTime = now;
    //         if (passtime < 0 || passtime >= 1000)
    //             return;
    //         m_nCurDelay += passtime;
    //         while (CanOnUpdate())
    //             OnUpdate();
    //         CheckOver();
    //     }
    #endregion


    /// <summary>
    /// 定时器结束消息，注：只有当m_nRepeatCount>0并且执行到结束时才会触发
    /// </summary>
    /// <returns></returns>
   /** public Signal onTimerComplete
    {
        get
        {
            return m_TimerEvent.TIMER_COMPLETE;
        }

    }**/

    /// <summary>
    /// 结束后是否直接回收
    /// </summary>
    public bool isOverDel = false;
    #region 静态函数

    /// <summary>
    /// 添加定时器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="delay">延时（毫秒）</param>
    /// <returns></returns>
    public static GTimer AddTimer(MonoBehaviour mono, int delay)
    {
        return AddTimer(mono, delay, 0, null, null, false, false);
    }

    /// <summary>
    /// 添加定时器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="delay">延时（毫秒）</param>
	/// <param name="repeatHandler">每次执行回调</param>
    /// <param name="startnow">是否立刻开始</param>
    /// <returns></returns>
    public static GTimer AddTimer(MonoBehaviour mono, int delay, TimerHandler repeatHandler, bool startnow)
    {
		return AddTimer(mono, delay, 0, repeatHandler, null, startnow, false);
    }

    /// <summary>
    /// 添加一次延迟定时器,结束后自动回收
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="delay">延时（毫秒）</param>
    /// <returns></returns>
	public static GTimer AddTimer(MonoBehaviour mono, int delay, TimerHandler repeatHandler)
    {
		return AddTimer(mono, delay, 1, repeatHandler, null, true, true);
    }

    /// <summary>
    /// 添加定时器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="delay">延时（毫秒）</param>
    /// <param name="repeatCount">执行次数，0=无限</param>
    /// <param name="ontimer">每次执行回调</param>
    /// <param name="startnow">是否立刻开始</param>
    /// <returns></returns>
	public static GTimer AddTimer(MonoBehaviour mono, int delay, int repeatCount, TimerHandler repeatHandler, bool startnow)
    {
		return AddTimer(mono, delay, repeatCount, repeatHandler, null, startnow, false);
    }
    /// <summary>
    /// 添加定时器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="delay">延时（毫秒）</param>
    /// <param name="repeatCount">执行次数，0=无限</param>
    /// <param name="ontimer">每次执行回调</param>
    /// <param name="ontimercomplete">定时器结束徽调</param>
    /// <param name="startnow">是否立刻开始</param>
    /// <param name="isOverDel">结束后是否删除</param>
    /// <returns></returns>
	public static GTimer AddTimer(MonoBehaviour mono, int delay, int repeatCount,  TimerHandler repeatHandler, TimerHandler completehandler, bool startnow)
    {
		return AddTimer(mono, delay, repeatCount, repeatHandler, completehandler, startnow, false); 
    }
    /// <summary>
    /// 添加定时器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="delay">延时（毫秒）</param>
    /// <param name="repeatCount">执行次数，0=无限</param>
    /// <param name="ontimer">每次执行回调</param>
    /// <param name="ontimercomplete">定时器结束徽调</param>
    /// <param name="startnow">是否立刻开始</param>
    /// <param name="isOverDel">结束后是否删除</param>
    /// <returns></returns>
	public static GTimer AddTimer(MonoBehaviour mono, int delay, int repeatCount, TimerHandler repeatHandler, TimerHandler completehandler, bool startnow,bool isOverDel)
    {
		GTimer timer = new GTimer(mono, delay, repeatCount,completehandler,repeatHandler);
        timer.isOverDel = isOverDel;

        if (startnow)
            timer.Start();
        return timer;
    }
    #endregion
}

