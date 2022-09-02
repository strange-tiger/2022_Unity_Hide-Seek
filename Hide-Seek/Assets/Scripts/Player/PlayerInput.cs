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
#if UNITY_ANDROID == false
    [SerializeField]
    private string _MoveFrontAxisName = "Vertical";
    [SerializeField]
    private string _MoveRightAxisName = "Horizontal";
    [SerializeField]
    private string _RotateYAxisName = "Mouse X";
#endif
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
#if UNITY_ANDROID == false
    private bool _cursorLock = true;
#endif

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
#if UNITY_ANDROID == false
        _cursorLock = true;
#endif

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
#if UNITY_ANDROID == false
        CursorState();
#endif
        UpdateFullMapToggle();
        UpdateUseWard();
        UpdateUseLure();
    }

    public void UpdateMove()
    {
#if UNITY_ANDROID
        MoveFront = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
        MoveRight = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
#else
        MoveFront = Input.GetAxis(_MoveFrontAxisName);
        MoveRight = Input.GetAxis(_MoveRightAxisName);
#endif
        UpdateShake();
    }

    public void UpdateRotate()
    {
#if UNITY_ANDROID
        RotateY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
#else
        RotateY = Input.GetAxis(_RotateYAxisName);
#endif
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
#if UNITY_ANDROID == false
    private void CursorState()
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
#endif
    public void UpdateFullMapToggle()
    {
#if UNITY_ANDROID
        if(OVRInput.GetDown(OVRInput.Button.Three))
#else
        if(Input.GetKeyDown(KeyCode.M))
#endif
        {
            IsFullMap = !IsFullMap;
        }
    }

    public void UpdateUseWard()
    {
#if UNITY_ANDROID
        if(OVRInput.GetDown(OVRInput.Button.One))
#else
        if (Input.GetKey(KeyCode.E))
#endif
        {
            UseWard?.Invoke();
        }
    }

    public void UpdateUseLure()
    {
#if UNITY_ANDROID
        if(OVRInput.GetDown(OVRInput.Button.Two))
#else
        if (Input.GetKey(KeyCode.Q))
#endif
        {
            UseLure?.Invoke();
        }
    }

#if UNITY_ANDROID == false
    public void UnlockCursor() => _cursorLock = false;
    public void ToggleCursorLock(bool isPause) => _cursorLock = !isPause;
#endif
    public void PauseMove(bool isPause) => IsMoving = !isPause;
    private void OnEnable()
    {
#if UNITY_ANDROID == false
        GameManager.Instance.OnGameOver.AddListener(UnlockCursor);
        GameManager.Instance.OnEscape.AddListener(UnlockCursor);
        GameManager.Instance.OnPause.AddListener(ToggleCursorLock);
#endif
        GameManager.Instance.OnPause.AddListener(PauseMove);
    }

    private void OnDisable()
    {
#if UNITY_ANDROID == false
        UnlockCursor();
        CursorState();
        //GameManager.Instance.OnGameOver.RemoveListener(UnlockCursor);
        //GameManager.Instance.OnEscape.RemoveListener(UnlockCursor);
        //GameManager.Instance.OnPause.RemoveListener(ToggleCursorLock);
#endif
        //GameManager.Instance.OnPause.RemoveListener(PauseMove);
    }
}
