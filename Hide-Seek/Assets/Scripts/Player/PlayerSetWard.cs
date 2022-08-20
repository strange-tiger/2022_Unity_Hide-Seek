using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetWard : MonoBehaviour
{
    public GameObject Ward;

    private PlayerInput _input;
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();

        _input.UseWard -= SetWard;
        _input.UseWard += SetWard;
    }

    public void SetWard()
    {
        if(!Ward.activeSelf)
        {
            Ward.transform.position = transform.position;
            Ward.SetActive(true);
        }
    }
}
