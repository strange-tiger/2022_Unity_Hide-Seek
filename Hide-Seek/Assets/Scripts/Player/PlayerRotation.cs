using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector2 _rotate;
    private float _rotateSpeed = 3f;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rotate = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        Quaternion deltaRotation = Quaternion.Euler(0f, _rotateSpeed * _rotate.x, 0f);

        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);

        //Vector2 mov2d = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        Vector3 dir = new Vector3(_rotate.x, 0, _rotate.y).normalized;
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0f;
        transform.position += (dir * _rotateSpeed * Time.deltaTime);
    }
}
