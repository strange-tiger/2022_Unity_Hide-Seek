using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinUI : MonoBehaviour, ISpin
{
    [SerializeField]
    protected float rotationSpeed = 60f;

    private void Update()
    {
        spin();
    }

    public void spin()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
}
