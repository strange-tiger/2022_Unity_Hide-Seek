using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Detectable, ISpin
{
    [SerializeField]
    protected float rotationSpeed = 60f;
    [SerializeField]
    protected float getDist = 5f;
    protected int getterLayer;
    protected new void Awake()
    {
        base.Awake();
        getterLayer = LayerMask.NameToLayer("Getter"); ;
    }

    protected void Update()
    {
        spin();
    }

    public void spin()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
}
