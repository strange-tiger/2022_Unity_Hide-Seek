using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _input;
    private Rigidbody _rigidbody;
    private AudioSource _audio;
    private Camera _mainCam;
    private float _camInitPositionY;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
        _mainCam = Camera.main;
        _camInitPositionY = _mainCam.transform.localPosition.y;

        _input.OnMove -= SetShake;
        _input.OnMove += SetShake;
        _period = _IdleShakePeriod;

        _input.OnMove -= SoundRun;
        _input.OnMove += SoundRun;
    }

    private void Update()
    {
        move();
        rotate();
        shake();
    }

    [Header("Move")]
    [SerializeField]
    private float _MoveSpeed = 7f;
    private void move()
    {
        _input.UpdateMove();
        Vector3 deltaPosition = _MoveSpeed * Time.deltaTime * (_input.MoveFront * transform.forward + _input.MoveRight * transform.right);

        _rigidbody.MovePosition(_rigidbody.position + deltaPosition);
    }

    [SerializeField]
    private float _RotateYAxisSpeed = 300f;

    private void rotate()
    {
        _input.UpdateRotate();

        float rotationY = _RotateYAxisSpeed * _input.RotateY * Time.deltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0f, rotationY, 0f);

        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }

    [Header("Shake")]
    [SerializeField]
    private float _Amplitude = 0.1f;
    [SerializeField]
    private float _IdleShakePeriod = 3f;
    [SerializeField]
    private float _RunShakePeriod = 0.4f;

    private float _elapsedTime = 0f;
    private float _period;
    private void shake()
    {
        if (_elapsedTime > _period)
        {
            _elapsedTime = 0f;
        }
        _elapsedTime += Time.deltaTime;

        float shake = _Amplitude * Mathf.Sin(_elapsedTime * 2f * Mathf.PI / _period);
        _mainCam.transform.localPosition = new Vector3(0f, _camInitPositionY + shake, 0f);
    }

    public void SetShake(bool isMoving)
    {
        if (isMoving)
        {
            _period = _RunShakePeriod;
        }
        else
        {
            _period = _IdleShakePeriod;
        }
    }

    public void SoundRun(bool isMoving)
    {
        if (!isMoving)
        {
            _audio.Stop();
        }
        else if(!_audio.isPlaying)
        {
            _audio.Play();
        }
    }
}
