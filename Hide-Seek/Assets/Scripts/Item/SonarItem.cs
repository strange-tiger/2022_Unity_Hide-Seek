using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarItem : Item
{
    [SerializeField]
    private float _GetItemDist = 5f;
    [SerializeField]
    private AudioClip _SonarSound;
    private AudioSource _audio;
    private GameObject _sonar;
    private bool _used = false;
    private new void Awake()
    {
        base.Awake();

        _audio = GetComponent<AudioSource>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).name == "Sonar")
            {
                _sonar = transform.GetChild(i).gameObject;
                _sonar.SetActive(false);
            }
        }
    }

    private new void Update()
    {
        base.Update();
        DeactiveAfterUse();
    }

    private new void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);

        if (other.tag != "Player")
        {
            return;
        }

        Vector3 distance = other.transform.position - transform.position;
        if (distance.sqrMagnitude < _GetItemDist && !_used)
        {
            _sonar?.SetActive(true);
            _used = true;
            _audio.PlayOneShot(_SonarSound);
        }
    }
    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    public void DeactiveAfterUse()
    {
        if (_used && !_sonar.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
