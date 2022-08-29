using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detectable : MonoBehaviour
{
    [Header("Detectable")]
    [SerializeField]
    protected LayerMask markerLayer;
    [SerializeField]
    protected GameObject marker;
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
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Ward")
        {
            marker.SetActive(true);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Ward")
        {
            marker.SetActive(false);
        }
    }
}
