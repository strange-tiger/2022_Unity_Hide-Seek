using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    [SerializeField]
    private float _GetItemDist = 5f;
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

        float distance = (other.transform.position - transform.position).sqrMagnitude;
        if(distance < _GetItemDist)
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