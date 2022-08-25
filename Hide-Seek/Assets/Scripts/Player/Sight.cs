using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public PlayerInput input;

    protected GameObject marker;
    [SerializeField]
    protected string markerName = "Marker_S";
    protected void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).gameObject.name == markerName)
            {
                marker = transform.GetChild(i).gameObject;
                marker.SetActive(false);
                // Debug.Log($"Success:{marker.name}");
            }
        }

        input.OnFullMap -= OnSight;
        input.OnFullMap += OnSight;
    }

    public void OnSight(bool onSight)
    {
        marker.SetActive(onSight);
    }
}
