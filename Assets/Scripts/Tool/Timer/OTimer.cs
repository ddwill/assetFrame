using UnityEngine;
using signal.osnet.signals;
using System.Collections;

/// <summary>
/// 定时器,按游戏内部时间比较
/// </summary>
public class OTimer 
{
	public delegate void TimerHandler();

	private float m_nDelay = 1000;
	private int m_nCurrentCount = 0;
	private int m_nRepeatCount = 0;
    private bool m_bRunning;
    private TimerEvents m_TimerEvent;
	private TimerHandler m_handler;
    private MonoBehaviour m_Mono;

    private int nowtimeid = 0;

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="dealy">延时（毫秒）</param>
    /// <param name="repeatCount">执行次数，0为无限</param>
    public OTimer(MonoBehaviour mono, int dealy, int repeatCount)
    {
        m_nRepeatCount = repeatCount;
        m_nDelay = (float)dealy / 1000f;
        m_Mono = mono;
        Init();
    }

	public OTimer(MonoBehaviour mono, int dealy, int repeatCount,TimerHandler handler)
	{
		m_nRepeatCount = repeatCount;
		m_nDelay = (float)dealy / 1000f;
		m_Mono = mono;
		Init();
		m_handler = handler;
	}

    private void Init()
	{
        m_bRunning = false;
        m_TimerEvent = new TimerEvents();
        m_nCurrentCount = 0;
        nowtimeid = 0;
	}
	
    /// <summary>
    /// 如果指定的延迟为负数将默认为1秒
    /// </summary>
    /// <returns></returns>
	public float delay
	{
		get
		{
			return m_nDelay;
		}
		set
		{
            m_nDelay = value / 1000f;
            if(m_nDelay <= 0)
			    m_nDelay = 1f;
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
            return m_bRunning;
        }
    }
	
	public void Start()
	{
		if(running)
			return;
		m_bRunning = true;
        ++nowtimeid;
        m_Mono.StartCoroutine(UpdateTime(nowtimeid));
	}
	
	public void Stop()
	{
		m_bRunning = false;
	}

    public void Reset()
    {
        m_nCurrentCount = 0;
        m_bRunning = false;
    }

    private IEnumerator UpdateTime(int runid)
    {
		yield return new WaitForSeconds(m_nDelay);
		RefreshTime(runid);
    }
	
	private void RefreshTime(int runid)
	{
		if (nowtimeid != runid)
            return;
		if (m_nRepeatCount != 0 && m_nCurrentCount >= m_nRepeatCount)
		{
            m_TimerEvent.TIMER_COMPLETE.dispatch(this);
			m_handler();
            Reset();
			return;
		}
		++m_nCurrentCount;
        if(m_bRunning)
        {
            m_TimerEvent.TIMER.dispatch(this);
            m_Mono.StartCoroutine(UpdateTime(runid));
        }
	}

    /// <summary>
    /// 每次定时器触发消息
    /// </summary>
    /// <returns></returns>
    public Signal onTimer
    {
        get
        {
            return m_TimerEvent.TIMER;
        }
    }

    /// <summary>
    /// 定时器结束消息，注：只有当m_nRepeatCount>0并且执行到结束时才会触发
    /// </summary>
    /// <returns></returns>
    public Signal onTimerComplete
    {
        get
        {
            return m_TimerEvent.TIMER_COMPLETE;
        }

    }

    #region 静态函数

    /// <summary>
    /// 添加定时器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="delay">延时（毫秒）</param>
    /// <returns></returns>
    public static OTimer AddTimer(MonoBehaviour mono, int delay)
    {
        return AddTimer(mono, delay, 0, null, null, false);
    }

    /// <summary>
    /// 添加定时器
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="delay">延时（毫秒）</param>
    /// <param name="ontimer">每次执行回调</param>
    /// <param name="startnow">是否立刻开始</param>
    /// <returns></returns>
    public static OTimer AddTimer(MonoBehaviour mono, int delay, Signal.listen ontimer, bool startnow)
    {
        return AddTimer(mono, delay, 0, ontimer, null, startnow);
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
    public static OTimer AddTimer(MonoBehaviour mono, int delay, int repeatCount, Signal.listen ontimer, bool startnow)
    {
        return AddTimer(mono, delay, repeatCount, ontimer, null, startnow);
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
    /// <returns></returns>
    public static OTimer AddTimer(MonoBehaviour mono, int delay, int repeatCount, Signal.listen ontimer, Signal.listen ontimercomplete, bool startnow)
    {
        OTimer timer = new OTimer(mono, delay, repeatCount);
        if (ontimer != null)
            timer.onTimer.add(ontimer);
        if (ontimercomplete != null)
            timer.onTimerComplete.add(ontimercomplete);
        if (startnow)
            timer.Start();
        return timer;
    }
    #endregion
	
}

