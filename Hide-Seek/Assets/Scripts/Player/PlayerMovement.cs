using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Marker;
    // public AudioClip FootStep;
    // public float XAngleLimit = 60f;
    
    private PlayerInput _input;
    private Rigidbody _rigidbody;
    private AudioSource _audio;
    private Camera _mainCam;
    private float _initPositionY;
    [SerializeField]
    private float _MoveSpeed = 7f;
    private float _rotateXAxisSpeed = 30f;
    [SerializeField]
    private float _RotateYAxisSpeed = 300f;
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
        _mainCam = Camera.main;
        _initPositionY = _mainCam.transform.localPosition.y;

        _input.OnMove -= SetShake;
        _input.OnMove += SetShake;
        _period = _idleShake;
    }

    private void Update()
    {
        move();
        rotate();
        shake();
    }

    private void move()
    {
        _input.UpdateMove();
        Vector3 deltaPosition = _MoveSpeed * Time.deltaTime * (_input.MoveFront * transform.forward + _input.MoveRight * transform.right);

        _rigidbody.MovePosition(_rigidbody.position + deltaPosition);
    }

    private float rotationYAmount = 0f;
    private float rotationX = 0f;
    private void rotate()
    {
        _input.UpdateRotate();
        rotationYAmount = _RotateYAxisSpeed * _input.RotateY * Time.deltaTime;

        float rotationY = rotationYAmount;
        Quaternion deltaRotation = Quaternion.Euler(rotationX, rotationY, 0f);

        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }

    private float _elapsedTime = 0f;
    private float _period;

    [SerializeField]
    private float _amplitude = 0.1f;
    [SerializeField]
    private float _idleShake = 3f;
    [SerializeField]
    private float _runShake = 0.4f;
    private void shake()
    {
        if (_elapsedTime > _period)
        {
            _elapsedTime = 0f;
        }
        
        _elapsedTime += Time.deltaTime;
        float shake = _amplitude * Mathf.Sin(_elapsedTime * 2f * Mathf.PI / _period);

        _mainCam.transform.localPosition = new Vector3(0f, _initPositionY + shake, 0f);
    }

    public void SetShake(bool isMoving)
    {
        if (isMoving)
        {
            _period = _runShake;

            if (!_audio.isPlaying)
            {
                _audio.Play();
                // Debug.Log("playing");
            }
        }
        else
        {
            _period = _idleShake;

            _audio.Stop();
            // Debug.Log("Stop");
        }
    }
}
