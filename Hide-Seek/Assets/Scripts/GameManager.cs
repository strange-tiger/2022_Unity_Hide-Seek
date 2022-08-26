using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public UnityEvent OnGameOver = new UnityEvent();
    public UnityEvent OnEscape = new UnityEvent();
    public UnityEvent<int> OnKeyChanged = new UnityEvent<int>();
    public int CurrentKey
    {
        get
        {
            return _currentKey;
        }
        set
        {
            _currentKey = value;
            OnKeyChanged?.Invoke(_currentKey);
        }
    }
    public UnityEvent AllKeysCollected = new UnityEvent();
    public UnityEvent<int> OnHealthChanged = new UnityEvent<int>();
    public int HealthCount
    {
        get
        {
            return _healthCount;
        }
        set
        {
            _healthCount = value;
            OnHealthChanged?.Invoke(_healthCount);
        }
    }
    public UnityEvent<bool> OnPause = new UnityEvent<bool>();
    public bool IsPause
    {
        get
        {
            return _isPause;
        }
        set
        {
            _isPause = value;
            OnPause?.Invoke(_isPause);
        }
    }
    [Header("Escape Conditions")]
    public int KeyCountMax = 10;
    public int InitHealthCount = 3;

    private int _healthCount;
    private int _currentKey = 0;
    private bool _isGameOver = false;
    private bool _isEscape = false;
    private bool _isPause = false;
    private void Start()    
    {
        reset();
    }

    private void reset()
    {
        _currentKey = 0;
        _healthCount = InitHealthCount;
        _isGameOver = false;
        _isEscape = false;
        IsPause = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            IsPause = !IsPause;
        }
    }

    public void start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            reset();
            SceneManager.LoadScene(1);
        }
    }

    public void Quit()
    {
        Debug.Log("Quit");

        if (_isGameOver)
        {
            Application.Quit();
            return;
        }
        if (_isEscape)
        {
            Application.Quit();
            return;
        }
        if (IsPause)
        {
            Application.Quit();
            return;
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Application.Quit();
            return;
        }
    }

    public void Restart()
    {
        if (_isGameOver)
        {
            reset();
            SceneManager.LoadScene(1);
            return;
        }
        if (_isEscape)
        {
            reset();
            SceneManager.LoadScene(1);
            return;
        }
        if (IsPause)
        {
            reset();
            SceneManager.LoadScene(1);
            return;
        }
    }

    public void LoadTitle()
    {
        if (_isGameOver)
        {
            reset();
            SceneManager.LoadScene(0);
            return;
        }
        if (_isEscape)
        {
            reset();
            SceneManager.LoadScene(0);
            return;
        }
        if (IsPause)
        {
            reset();
            SceneManager.LoadScene(0);
            return;
        }
    }

    public void AddKey()
    {
        ++CurrentKey;
        if(CurrentKey == KeyCountMax)
        {
            AllKeysCollected?.Invoke();
        }
    }

    public void SubHealth()
    {
        --HealthCount;
    }

    public void End()
    {
        if (HealthCount <= 0)
        {
            _isGameOver = true;
            OnGameOver?.Invoke();
        }
    }

    public void Escape()
    {
        _isEscape = true;
        OnEscape?.Invoke();
    }
}