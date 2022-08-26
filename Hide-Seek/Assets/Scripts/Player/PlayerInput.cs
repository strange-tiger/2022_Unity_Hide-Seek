using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // 이동 입력
    public float MoveFront { get; private set; }
    public float MoveRight { get; private set; }

    public float RotateX { get; private set; }
    public float RotateY { get; private set; }

    [SerializeField]
    private string _MoveFrontAxisName = "Vertical";
    [SerializeField]
    private string _MoveRightAxisName = "Horizontal";
    [SerializeField]
    private string _RotateYAxisName = "Mouse X";

    // 플레이어 상태
    public event Action<bool> OnMove;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        set
        {
            _isMoving = value;
            OnMove?.Invoke(_isMoving);
        }
    }
    private bool _isMoving = false;
    private PlayerHealth _health;
    private bool _cursorLock = true;

    // 맵 확대축소
    public event Action<bool> OnFullMap;
    public bool IsFullMap
    {
        get
        {
            return _isFullMap;
        }
        set
        {
            _isFullMap = value;
            OnFullMap?.Invoke(_isFullMap);
        }
    }
    private bool _isFullMap = false;

    // 와드 사용
    public event Action UseWard;

    // 미끼 사용
    public event Action UseLure;
    private void Awake()
    {
        reset();
        _cursorLock = true;

        _health = GetComponent<PlayerHealth>();
        _health.OnDeath -= this.reset; 
        _health.OnDeath += this.reset; 
    }

    public void reset()
    {
        MoveFront = 0f;
        MoveRight = 0f;
        RotateX = 0f;
        RotateY = 0f;
        IsFullMap = false;
        IsMoving = false;
    }

    private void Update()
    {
        LockCursor();
        UpdateFullMapToggle();
        UpdateUseWard();
        UpdateUseLure();
    }

    public void UpdateMove()
    {
        MoveFront = Input.GetAxis(_MoveFrontAxisName);
        MoveRight = Input.GetAxis(_MoveRightAxisName);
        UpdateShake();
    }

    public void UpdateRotate()
    {
        RotateY = Input.GetAxis(_RotateYAxisName);
    }

    public void UpdateShake()
    {
        bool isMoving = false;

        if (Mathf.Abs(MoveFront) > 0.05f)
        {
            isMoving = true;
        }

        if (Mathf.Abs(MoveRight) > 0.05f)
        {
            isMoving = true;
        }
        
        if (IsMoving == isMoving)
        {
            return;
        }

        IsMoving = isMoving;
    }

    private void LockCursor()
    {
        Cursor.visible = !_cursorLock;
        if (_cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void UpdateFullMapToggle()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            IsFullMap = !IsFullMap;
        }
    }

    public void UpdateUseWard()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            UseWard?.Invoke();
        }
    }

    public void UpdateUseLure()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            UseLure?.Invoke();
        }
    }

    public void UnlockCursor() => _cursorLock = false;
    public void ToggleCursorLock(bool isPause) => _cursorLock = !isPause;
    public void PauseMove(bool isPause) => IsMoving = !isPause;
    private void OnEnable()
    {
        GameManager.Instance.OnGameOver.AddListener(UnlockCursor);
        GameManager.Instance.OnEscape.AddListener(UnlockCursor);
        GameManager.Instance.OnPause.AddListener(ToggleCursorLock);
        GameManager.Instance.OnPause.AddListener(PauseMove);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameOver.RemoveListener(UnlockCursor);
        GameManager.Instance.OnEscape.RemoveListener(UnlockCursor);
        GameManager.Instance.OnPause.RemoveListener(ToggleCursorLock);
        GameManager.Instance.OnPause.RemoveListener(PauseMove);
        UnlockCursor();
        LockCursor();
    }
}
