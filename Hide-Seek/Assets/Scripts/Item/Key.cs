using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    public float GetItemDist = 2f;

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

        if (other.tag != "Player")
        {
            return;
        }

        Vector3 distance = other.transform.position - transform.position;
        if(distance.sqrMagnitude < GetItemDist)
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
