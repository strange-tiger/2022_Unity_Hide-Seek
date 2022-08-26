using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Item
{
    private AudioSource _audio;
    private GameObject _effects;
    [SerializeField]
    private float _GetPortalDist = 2f;
    private new void Awake()
    {
        base.Awake();
        _effects = transform.GetChild(2).gameObject;
        _effects.SetActive(false);
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

    private new void Update()
    {
        marker.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }

    private new void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);

        if (other.tag != "Player")
        {
            return;
        }
        _effects.SetActive(true);

        float distance = (other.transform.position - transform.position).sqrMagnitude;
        if (distance < _GetPortalDist)
        {
            GameManager.Instance.Escape();
        }
    }

    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.tag != "Player")
        {
            return;
        }
        _effects.SetActive(false);
    }
}
