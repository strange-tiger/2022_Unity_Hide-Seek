using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public string MoveFrontAxisName = "Vertical";
    public string MoveRightAxisName = "Horizontal";
    public string RotateXAxisName = "Mouse Y";
    public string RotateYAxisName = "Mouse X";

    public float MoveFront { get; private set; }
    public float MoveRight { get; private set; }

    public float RotateX { get; private set; }
    public float RotateY { get; private set; }

    public event Action<bool> OnFullMap;
    public bool IsFullMap { get; private set; }

    public event Action UseWard;

    private PlayerHealth _health;
    private bool _cursorLock = true;
    private void Awake()
    {
        MoveFront = 0f;
        MoveRight = 0f;
        RotateX = 0f;
        RotateY = 0f;
        IsFullMap = false;

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
        _cursorLock = true;
    }

    private void Update()
    {
        LockCursor();
        UpdateFullMapToggle();
        UpdateUseWard();
    }

    public void UpdateMove()
    {
        MoveFront = Input.GetAxis(MoveFrontAxisName);
        MoveRight = Input.GetAxis(MoveRightAxisName);
    }

    public void UpdateRotate()
    {
        //RotateX = -Input.GetAxis(RotateXAxisName);
        RotateY = Input.GetAxis(RotateYAxisName);
    }

    public void UpdateFullMapToggle()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            IsFullMap = !IsFullMap;
            OnFullMap?.Invoke(IsFullMap);
        }
    }

    public void UpdateUseWard()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseWard?.Invoke();
            // Debug.Log("Set");
        }
    }

    private void LockCursor()
    {
        if (_cursorLock)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void SetCursorLock() => _cursorLock = false;
    private void OnEnable()
    {
        GameManager.Instance.OnGameOver.AddListener(SetCursorLock);
        GameManager.Instance.OnEscape.AddListener(SetCursorLock);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameOver.RemoveListener(SetCursorLock);
        GameManager.Instance.OnEscape.RemoveListener(SetCursorLock);
    }
}
