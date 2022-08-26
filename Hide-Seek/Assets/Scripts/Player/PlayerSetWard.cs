using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetWard : MonoBehaviour
{
    [SerializeField]
    private GameObject _Ward;
    private PlayerInput _input;
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();

        _input.UseWard -= SetWard;
        _input.UseWard += SetWard;
    }

    public void SetWard()
    {
        if(!_Ward.activeSelf)
        {
            _Ward.transform.position = transform.position;
            _Ward.SetActive(true);
        }
    }
}
