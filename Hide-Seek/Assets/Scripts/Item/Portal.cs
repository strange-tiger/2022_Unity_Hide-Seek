using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Item
{
    private AudioSource _audio;
    [SerializeField]
    private float _GetPortalDist = 2f;
    private void Awake()
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

        Vector3 distance = other.transform.position - transform.position;
        if (distance.sqrMagnitude < _GetPortalDist)
        {
            GameManager.Instance.Escape();
            gameObject.SetActive(false);
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
