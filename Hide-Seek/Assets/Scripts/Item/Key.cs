using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    private new void Awake()
    {
        base.Awake();
    }

    private new void Update()
    {
        base.Update();
    }

    private new void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);

        float distance = (other.transform.position - transform.position).sqrMagnitude;
        if(distance < getDist)
        {
            GameManager.Instance.AddKey();
            gameObject.SetActive(false);
        }
    }
    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}