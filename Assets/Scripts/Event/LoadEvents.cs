using signal.osnet.signals;

public class LoadEvents:SignalEvents
{
    public LoadEvents()
		: base(new string[]{
			"onLoadOk"
            ,"onLoadErr"
		})
	{
	}

    public Signal onLoadOk
    {
        get { return getSignal("onLoadOk"); }
    }

    public Signal onLoadErr
    {
        get { return getSignal("onLoadErr"); }
    }
}

