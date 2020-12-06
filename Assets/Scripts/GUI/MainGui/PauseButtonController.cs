using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseButtonController : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Fields and properties ==========
    private EventListener[] _eventListener;

    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        AddListeners();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
    // ========== Public methods ==========
    public void OnPauseClick()
    {
        GameFlowManager.instance.OnPause();
    }
    // ========== Event listener methods ==========
    private void AddListeners()
    {
        _eventListener = new EventListener[2];
        _eventListener[0] = EventSystem.instance.AddListener(EventCode.ON_RESET_GAME, this, OnResetGame);
        _eventListener[1] = EventSystem.instance.AddListener(EventCode.ON_MAIN_MENU, this, OnMainMenu);
    }

    private void RemoveListeners()
    {
        foreach (EventListener listener in _eventListener)
            EventSystem.instance.RemoveListener(listener.eventCode, listener);
    }

    private void OnResetGame(object[] eventParam)
    {
        gameObject.SetActive(true);
    }

    private void OnMainMenu(object[] eventParam)
    {
        gameObject.SetActive(false);
    }
}
