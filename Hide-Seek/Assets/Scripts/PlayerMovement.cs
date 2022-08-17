using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float RotateSpeed = 30f;

    // private Camera _mainCam;
    private PlayerInput _input;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        // _mainCam = Camera.main;
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        move();
        rotate();
    }

    private void move()
    {
        Vector3 deltaPosition = MoveSpeed * Time.fixedDeltaTime * (_input.MoveFront * transform.forward + _input.MoveRight * transform.right);

        _rigidbody.MovePosition(_rigidbody.position + deltaPosition);
    }

    private float rotationX = 0f;
    private float _elapsedTime = 0f;
    private void rotate()
    {
        //transform.LookAt(_input.ViewPosition);

        //Ray ray = Camera.main.ScreenPointToRay(_input.ViewPosition);
        //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

        float rotationXAmount = RotateSpeed * _input.RotateX;
        float rotationYAmount = RotateSpeed * _input.RotateY;

        rotationX = Mathf.Clamp(rotationX + rotationXAmount, -60f, 60f);
        float rotationY = transform.eulerAngles.y + rotationYAmount;
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);

        if (_elapsedTime >= 1f)
        {
            _elapsedTime = 0f;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _elapsedTime);
        _elapsedTime += Time.fixedDeltaTime;
    }
}
