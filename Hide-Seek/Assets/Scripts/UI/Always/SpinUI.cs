using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinUI : MonoBehaviour, ISpin
{
    [SerializeField]
    protected float rotationSpeed = 60f;

    protected void Update()
    {
        spin();
    }

    virtual public void spin()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
