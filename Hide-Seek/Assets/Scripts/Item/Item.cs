using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Detectable
{
    public float rotationSpeed = 60f;

    protected void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
}
