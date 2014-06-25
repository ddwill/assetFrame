using signal.osnet.signals;

public class TimerEvents:SignalEvents
{
    public TimerEvents()
		: base(new string[]{
			"TIMER"
            ,"TIMER_COMPLETE"
		})
	{
	}

    public Signal TIMER
    {
        get { return getSignal("TIMER"); }
    }

    public Signal TIMER_COMPLETE
    {
        get { return getSignal("TIMER_COMPLETE"); }
    }
}

