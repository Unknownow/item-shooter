using System;
using System.Collections.Generic;

public class EventSystem
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    /// <summary>
    /// Singleton instance
    /// </summary>
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

    /// <summary>
    /// Event listeners dictionary
    /// Store all listeners 
    /// </summary>
    private Dictionary<string, List<EventListener>> _eventList = null;

    private long _eventListenerCount;

    /// <summary>
    /// Constructor
    /// </summary>
    private EventSystem()
    {
        _eventList = new Dictionary<string, List<EventListener>>();
        _eventListenerCount = 0;
    }

    // ========== Public methods ==========
    /// <summary>
    /// Add listener to certain event code
    /// </summary>
    /// <param name="eventCode">Event code</param>
    /// <param name="caller">Object create this listener</param>
    /// <param name="callback">Callback to call when event is dispatched</param>
    /// <returns></returns>
    public EventListener AddListener(string eventCode, Object caller, Action<object[]> callback)
    {
        LogUtils.instance.Log(GetClassName(), "AddListener", "EVENT_CODE:", eventCode);
        if (!_eventList.ContainsKey(eventCode))
            _eventList.Add(eventCode, new List<EventListener>());
        List<EventListener> listEventListener = GetListenersList(eventCode);
        EventListener listener = GetListener(eventCode, caller, callback);
        if (listener == null)
        {
            listener = new EventListener(eventCode, eventCode + "_" + _eventListenerCount, caller, callback);
            listEventListener.Add(listener);
            _eventListenerCount += 1;
        }
        return listener;
    }

    /// <summary>
    /// Dispatch certain event.
    /// </summary>
    /// <param name="eventCode">Event code</param>
    /// <param name="eventParam">Param of event</param>
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

    /// <summary>
    /// Set enable for listener.
    /// </summary>
    /// <param name="listener"></param>
    /// <param name="isEnabled"></param>
    public void SetEnabledListener(EventListener listener, Boolean isEnabled)
    {
        LogUtils.instance.Log(GetClassName(), "DispatchEvent", "LISTENER_ID::", listener.listenerId, "IS_ENABLED", isEnabled.ToString());
        listener.isEnabled = isEnabled;
    }

    /// <summary>
    /// Remove listener from list.
    /// </summary>
    /// <param name="eventCode">Event code of listener</param>
    /// <param name="listener">Listener want to remove</param>
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

    /// <summary>
    /// Remove all listeners of certain event code.
    /// </summary>
    /// <param name="eventCode"></param>
    public void RemoveAllListenersOfEvent(string eventCode)
    {
        LogUtils.instance.Log(GetClassName(), "RemoveAllListenersOfEvent", "EVENT_CODE:", eventCode);
        List<EventListener> list = GetListenersList(eventCode);
        foreach (EventListener listener in list)
            listener.isRemoved = true;
    }

    /// <summary>
    /// Remove all listeners
    /// </summary>
    public void RemoveAllListeners()
    {
        _eventList.Clear();
    }

    // ========== Private methods ==========
    /// <summary>
    /// Get listener list of certain event code
    /// </summary>
    /// <param name="eventCode"></param>
    /// <returns>List of listeners</returns>
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

    /// <summary>
    /// Get listener by ID
    /// </summary>
    /// <param name="eventCode"></param>
    /// <param name="listenerId"></param>
    /// <returns></returns>
    private EventListener GetListener(string eventCode, string listenerId)
    {
        List<EventListener> listEventListener = GetListenersList(eventCode);
        if (listEventListener == null)
            return null;
        return listEventListener.Find(listener => listener.listenerId.Equals(listenerId));
    }

    /// <summary>
    /// Get listener by caller and callback
    /// </summary>
    /// <param name="eventCode"></param>
    /// <param name="caller"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private EventListener GetListener(string eventCode, Object caller, Action<object[]> callback)
    {
        List<EventListener> listEventListener = GetListenersList(eventCode);
        if (listEventListener == null)
            return null;
        return listEventListener.Find(listener => listener.caller.Equals(caller) && listener.callback.Equals(callback));
    }
}

/// <summary>
/// Event listener object
/// </summary>
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