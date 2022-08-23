using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Marker;
    public float MoveSpeed = 7f;
    public float RotateXAxisSpeed = 30f;
    public float RotateYAxisSpeed = 300f;
    public float XAngleLimit = 60f;
    
    private PlayerInput _input;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        move();
        rotate();
    }

    private void move()
    {
        _input.UpdateMove();
        Vector3 deltaPosition = MoveSpeed * Time.deltaTime * (_input.MoveFront * transform.forward + _input.MoveRight * transform.right);

        _rigidbody.MovePosition(_rigidbody.position + deltaPosition);
    }

    // private float rotationXAmount = 0f;
    private float rotationYAmount = 0f;
    private float rotationX = 0f;
    private void rotate()
    {
        _input.UpdateRotate();
        //rotationXAmount = RotateXAxisSpeed * _input.RotateX;
        rotationYAmount = RotateYAxisSpeed * _input.RotateY * Time.deltaTime;

        //rotationX = Mathf.Clamp(rotationX + rotationXAmount, -XAngleLimit, XAngleLimit);
        //float rotationY = transform.eulerAngles.y + rotationYAmount;
        float rotationY = rotationYAmount;
        Quaternion deltaRotation = Quaternion.Euler(rotationX, rotationY, 0f);

        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        // Debug.Log("rotate");
    }
}
