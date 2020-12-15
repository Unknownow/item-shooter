using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public string GetClassName()
    {
        return this.GetType().Name;
    }

    // ========== Singleton instance ==========
    private static GameFlowManager _instance;
    public static GameFlowManager instance
    {
        get
        {
            return _instance;
        }
    }
    private GameFlowManager()
    {
        if (_instance == null)
            _instance = this;
    }

    private bool _isPlaying;
    public bool isPlaying { get { return _isPlaying; } }
    // ========== Fields and properties ==========
    private int _countdownTime = 3;
    // ========== MonoBehaviour methods ==========
    private void Awake()
    {
        _isPlaying = false;
        // OnStartGame();
    }

    private void Start()
    {
        OnMainMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Time.timeScale = 1;
        if (Input.GetKeyDown(KeyCode.A))
            Time.timeScale = 0.2f;
        if (Input.GetKeyDown(KeyCode.S))
            Time.timeScale = 0;
        if (Input.GetKeyDown(KeyCode.D))
            Time.timeScale = 1.8f;
    }
    // ========== Public methods ==========
    public void OnMainMenu()
    {
        OnPause();
        Manager.instance.ResetPlayer();
        Manager.instance.ResetPoint();
        EventSystem.instance.DispatchEvent(EventCode.ON_MAIN_MENU);
        _isPlaying = true;
    }

    public void OnPause()
    {
        _isPlaying = false;
        StopAllCoroutines();
    }

    public void OnResume()
    {
        _countdownTime = 3;
        StartCoroutine("CountdownResumeGame");
    }

    public void OnStartGame()
    {
        OnPause();
        Manager.instance.ResetPlayer();
        Manager.instance.ResetPoint();
        EventSystem.instance.DispatchEvent(EventCode.ON_RESET_GAME);
        _countdownTime = 3;
        StartCoroutine("CountdownStartGame");
    }
    // ========== Private methods ==========
    private IEnumerator CountdownStartGame()
    {
        while (_countdownTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            FloatTextController.DoFloatStaticText(_countdownTime.ToString(), Vector3.zero, Vector3.one * 2, 0.8f, Colors.TURQUOISE);
            yield return new WaitForSeconds(0.9f);
            _countdownTime -= 1;
        }
        _isPlaying = true;
    }

    private IEnumerator CountdownResumeGame()
    {
        while (_countdownTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            FloatTextController.DoFloatStaticText(_countdownTime.ToString(), Vector3.zero, Vector3.one * 2, 0.8f, Colors.TURQUOISE);
            yield return new WaitForSeconds(0.9f);
            _countdownTime -= 1;
        }
        _isPlaying = true;
    }
}
