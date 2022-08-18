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
    // public Vector3 ViewPosition { get; private set; }

    // private Camera _mainCam;
    private void Awake()
    {
        MoveFront = 0f;
        MoveRight = 0f;
        RotateX = 0f;
        RotateY = 0f;
    }

    private void FixedUpdate()
    {
        if (false /*게임 오버 상태*/)
        {
            MoveFront = 0f;
            MoveRight = 0f;
            RotateX = 0f;
            RotateY = 0f;
            // ViewPosition = Vector3.zero;

            return;
        }
    }

    public void UpdateMove()
    {
        MoveFront = Input.GetAxis(MoveFrontAxisName);
        MoveRight = Input.GetAxis(MoveRightAxisName);
    }

    public void UpdateRotate()
    {
        RotateX = -Input.GetAxis(RotateXAxisName);
        RotateY = Input.GetAxis(RotateYAxisName);
    }
}
