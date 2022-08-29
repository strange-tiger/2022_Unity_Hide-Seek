using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Detectable : MonoBehaviour
{
    [Header("Detectable")]
    [SerializeField]
    protected LayerMask markerLayer;
    [SerializeField]
    protected GameObject marker;
    protected int playerLayer;
    protected int wardLayer;
    protected void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            if ((1 << transform.GetChild(i).gameObject.layer) == markerLayer.value)
            {
                marker = transform.GetChild(i).gameObject;
                marker.SetActive(false);
            }
        }

        playerLayer = LayerMask.NameToLayer("Player");
        wardLayer = LayerMask.NameToLayer("Ward");
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            marker.SetActive(true);
        }
        if (other.gameObject.layer == wardLayer)
        {
            marker.SetActive(true);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            marker.SetActive(false);
        }
        if (other.gameObject.layer == wardLayer)
        {
            marker.SetActive(false);
        }
    }
}
