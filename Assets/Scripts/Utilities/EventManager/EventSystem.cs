using System;
using System.Collections.Generic;

public class EventSystem
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    private static EventSystem _instance;

    public static EventSystem instance
    {
        get
        {
            if (_instance == null)
                _instance = new EventSystem();
            return _instance;
        }
    }

    private Dictionary<string, List<EventListener>> _eventList = null;
    public EventSystem()
    {
        _eventList = new Dictionary<string, List<EventListener>>();
    }

    public EventListener AddListener(string eventCode, Object caller, Action<object[]> callback)
    {
        LogUtils.instance.Log(GetClassName(), "AddListener", "EVENT_CODE:", eventCode);
        if (!_eventList.ContainsKey(eventCode))
            _eventList.Add(eventCode, new List<EventListener>());
        List<EventListener> listEventListener = GetListenersList(eventCode);
        EventListener listener = GetListener(eventCode, caller, callback);
        if (listener == null)
        {
            listener = new EventListener(eventCode, eventCode + "_" + listEventListener.Count.ToString(), caller, callback);
            listEventListener.Add(listener);
        }
        return listener;
    }

    public void DispatchEvent(string eventCode, object[] eventParam = null)
    {
        LogUtils.instance.Log(GetClassName(), "DispatchEvent", "EVENT_CODE:", eventCode);
        List<EventListener> listEventListener = GetListenersList(eventCode);
        if (listEventListener == null)
        {
            LogUtils.instance.Log(GetClassName(), "DispatchEvent", "EVENT_CODE:", eventCode, "DOES NOT HAVE ANY SUBCRIBERS");
            return;
        }
        foreach (EventListener listener in listEventListener)
        {
            if (listener.isEnabled && !listener.isRemoved)
                listener.callback(eventParam);
        }

        for (int i = 0; i < listEventListener.Count; i++)
        {
            if (listEventListener[i].isRemoved)
                listEventListener.RemoveAt(i);
        }
    }

    public void SetEnabledListener(EventListener listener, Boolean isEnabled)
    {
        LogUtils.instance.Log(GetClassName(), "DispatchEvent", "LISTENER_ID::", listener.listenerId, "IS_ENABLED", isEnabled.ToString());
        listener.isEnabled = isEnabled;
    }

    public void RemoveListener(string eventCode, EventListener listener)
    {
        LogUtils.instance.Log(GetClassName(), "RemoveListener", "EVENT_CODE:", eventCode, "LISTENER_ID:", listener.listenerId);
        EventListener existedListener = GetListener(eventCode, listener.listenerId);
        if (existedListener == null)
        {
            LogUtils.instance.Log(GetClassName(), "RemoveListener", "EVENT_CODE:", eventCode, "LISTENER DOES NOT EXIST!");
            return;
        }
        existedListener.isRemoved = true;
    }

    public void RemoveAllListenersOfEvent(string eventCode)
    {
        LogUtils.instance.Log(GetClassName(), "RemoveAllListenersOfEvent", "EVENT_CODE:", eventCode);
        List<EventListener> list = GetListenersList(eventCode);
        foreach (EventListener listener in list)
            listener.isRemoved = true;
    }

    public void RemoveAllListeners()
    {
        _eventList.Clear();
    }

    private List<EventListener> GetListenersList(string eventCode)
    {
        if (!_eventList.ContainsKey(eventCode))
        {
            LogUtils.instance.Log(GetClassName(), "GetListenersList", "EVENT_CODE:", eventCode, "DOES NOT HAVE ANY SUBCRIBERS");
            return null;
        }
        List<EventListener> listEventListener;
        _eventList.TryGetValue(eventCode, out listEventListener);
        return listEventListener;
    }

    private EventListener GetListener(string eventCode, string listenerId)
    {
        List<EventListener> listEventListener = GetListenersList(eventCode);
        if (listEventListener == null)
            return null;
        return listEventListener.Find(listener => listener.listenerId.Equals(listenerId));
    }

    private EventListener GetListener(string eventCode, Object caller, Action<object[]> callback)
    {
        List<EventListener> listEventListener = GetListenersList(eventCode);
        if (listEventListener == null)
            return null;
        return listEventListener.Find(listener => listener.caller.Equals(caller) && listener.callback.Equals(callback));
    }
}

public class EventListener
{
    public string listenerId;
    public string eventCode;
    public Object caller;
    public bool isEnabled;
    public bool isRemoved;
    public delegate void EventCallback(params object[] args);
    public Action<object[]> callback;

    public EventListener(string eventCode, string listenerId, Object caller, Action<object[]> callback)
    {
        this.listenerId = listenerId;
        this.eventCode = eventCode;
        this.caller = caller;
        this.callback = callback;
        this.isEnabled = true;
        this.isRemoved = false;
    }
}