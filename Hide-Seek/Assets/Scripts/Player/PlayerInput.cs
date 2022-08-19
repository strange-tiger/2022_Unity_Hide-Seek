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

    public bool FullMapOn { get; private set; }
    // public Vector3 ViewPosition { get; private set; }

    private PlayerHealth _health;
    private void Awake()
    {
        MoveFront = 0f;
        MoveRight = 0f;
        RotateX = 0f;
        RotateY = 0f;
        FullMapOn = false;

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
        FullMapOn = false;
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
            FullMapOn = !FullMapOn;
        }
    }
}
