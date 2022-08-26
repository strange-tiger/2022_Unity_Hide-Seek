using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Item
{
    private AudioSource _audio;
    [SerializeField]
    private float _GetPortalDist = 2f;
    private new void Awake()
    {
        base.Awake();
        _audio = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        _audio.Play();
    }
    private void OnDisable()
    {
        _audio.Stop();
    }

    private new void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);

        if (other.tag != "Player")
        {
            return;
        }

        float distance = (other.transform.position - transform.position).sqrMagnitude;
        if (distance < _GetPortalDist)
        {
            GameManager.Instance.Escape();
        }
    }

    private new void Update()
    {
        marker.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }

    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
