using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Item
{
    [SerializeField]
    private float _GetPortalDist = 2f;
    private new void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);

        if (other.tag != "Player")
        {
            return;
        }

        Vector3 distance = other.transform.position - transform.position;
        if (distance.sqrMagnitude < _GetPortalDist)
        {
            GameManager.Instance.Escape();
        }
    }

    private new void Update()
    {
        base.Update();
    }

    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
