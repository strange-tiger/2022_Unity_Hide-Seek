using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Marker;
    public float MoveSpeed = 7f;
    public float RotateXAxisSpeed = 30f;
    public float RotateYAxisSpeed = 300f;
    // public float XAngleLimit = 60f;
    
    private PlayerInput _input;
    private Rigidbody _rigidbody;
    private Camera _mainCam;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _mainCam = Camera.main;
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
        Vector3 deltaPosition = MoveSpeed * Time.deltaTime * (_input.MoveFront * transform.forward + _input.MoveRight * transform.right);

        _rigidbody.MovePosition(_rigidbody.position + deltaPosition);
    }

    private float rotationYAmount = 0f;
    private float rotationX = 0f;
    private void rotate()
    {
        _input.UpdateRotate();
        rotationYAmount = RotateYAxisSpeed * _input.RotateY * Time.deltaTime;

        float rotationY = rotationYAmount;
        Quaternion deltaRotation = Quaternion.Euler(rotationX, rotationY, 0f);

        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }

    private float _elapsedTime = 0f;
    private float _amplitude = 0.1f;
    private float _period;
    private void shake()
    {
        if (_input.IsShake)
        {
            _period = 0.4f;
        }
        else
        {
            _period = 3f;
        }
        Debug.Log("shake" + _period);

        if (_elapsedTime > _period)
        {
            _elapsedTime = 0f;
        }

        _elapsedTime += Time.deltaTime;
        float shake = _amplitude * Mathf.Sin(_elapsedTime * 2 * Mathf.PI / _period);

        _mainCam.transform.localPosition = new Vector3(0f, 0.5f + shake, 0f);
    }
}
