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

    public int KeyCountMax = 10;
    public int InitHealthCount = 3;

    private int _healthCount;
    private int _currentKey = 0;
    private bool _isGameOver = false;
    private bool _isEscape = false;
    private void Start()    
    {
        reset();
    }

    private void reset()
    {
        CurrentKey = 0;
        HealthCount = InitHealthCount;
        _isGameOver = false;
        _isEscape = false;
    }

    public void Restart()
    {
        if (_isGameOver || _isEscape)
        {
            reset();
            SceneManager.LoadScene(0);
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