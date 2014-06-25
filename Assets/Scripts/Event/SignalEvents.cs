using System;
using System.Collections;
using signal.osnet.signals;
using System.Collections.Generic;

public class SignalEvents : IDisposable
{
    private Dictionary<string, Signal> _signals = new Dictionary<string, Signal>();
	public SignalEvents()
	{
	}
	public SignalEvents(string[] keys)
	{
		foreach (string key in keys)
		{
			addSignal(key);
		}
	}
	public void Dispose()
	{
		foreach (Signal signal in _signals.Values)
		{
			signal.removeAll();
		}
	}
    public Dictionary<string, Signal> signals
	{
		get { return _signals; }
	}
	public void addSignal(string key, Signal signal)
	{
		_signals[key] = signal;
	}
	public void addSignal(string key)
	{
		addSignal(key, new Signal());
	}
	public Signal getSignal(string key)
	{
		return _signals[key] as Signal;
	}
}
